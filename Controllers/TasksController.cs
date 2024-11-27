using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Immutable;
using Thesis;
using Thesis_backend.Data_Structures;

namespace Thesis_backend.Controllers
{
    public record TaskRequest
    {
        public required string TaskName { get; set; }
        public string Description { get; set; } = String.Empty;
        public required int PeriodRate { get; set; }
        public required bool TaskType { get; set; }
    }

    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ThesisControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public TasksController(ThesisDbContext database, ILogger<UsersController> logger) : base(database)
        {
            _logger = logger;
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetTask(string ID)
        {
            long convertedID;
            if (!long.TryParse(ID, out convertedID))
            {
                return NotFound("Incorrect ID format");
            }

            Data_Structures.PlayerTask? task = await Database.Tasks.All.Include(u => u.TaskOwner).SingleOrDefaultAsync(x => x.ID == convertedID && x.TaskOwner.ID == GetLoggedInUser());
            if (task is null)
            {
                return NotFound("No task with the following id");
            }
            return Ok(task.Serialize);
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetTaskHistories()
        {
            if (!CheckUserLoggedIn())
            {
                return NotFound("Not logged in");
            }

            long loggedInUserId = (long)(this.GetLoggedInUser()!);

            List<TaskHistory> taskHistories = await Database.TaskHistories.All.Include(t => t.CompletedTask).Where(x => x.OwnerId == loggedInUserId).Take(Config.TASK_HISTORY_SIZE).OrderByDescending(x => x.Completed).ToListAsync();

            if (taskHistories is null)
            {
                return NotFound("Can't find the task history");
            }

            return Ok(taskHistories.Select(x => x.Serialize));
        }

        [HttpPost("Cheat")]
        public async Task<IActionResult> CheatTaskScore([FromBody] string password, int amount)
        {
            if (password != Config.TASK_SCORE_CHEAT_PASSWORD)
            {
                return BadRequest("Incorrect cheat password");
            }

            long? loggedInUser = GetLoggedInUser();
            if (loggedInUser == 0 || loggedInUser is null)
            {
                return NotFound("Not logged in");
            }
            User? User = await Database.Users.Get(loggedInUser.Value);

            if (User is null)
            {
                return NotFound("Can't find the user");
            }
            User.CurrentTaskScore += amount;
            bool userScoreUpdate = await Update<Data_Structures.User>(User);

            if (userScoreUpdate)
            {
                return Ok(User.Serialize);
            }
            else
            {
                return NotFound("Couldn't cheat the task score for the user");
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllTasks()
        {
            User? user = await Database.Users.All.Include(u => u.UserTasks).SingleOrDefaultAsync(x => x.ID == Convert.ToInt64(HttpContext.Session.GetString("UserId")));

            if (user == null)
            {
                return NotFound("No user is logged in");
            }
            return Ok(user.UserTasks?.Where(x => !x.Deleted)?.Select(x => x.Serialize));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTask([FromBody] TaskRequest request)
        {
            User? user = await Database.Users.All.Include(u => u.UserTasks).SingleOrDefaultAsync(x => x.ID == Convert.ToInt64(HttpContext.Session.GetString("UserId")));

            if (user == null)
            {
                return NotFound("No user is logged in");
            }
            if (request.TaskName == "")
            {
                return BadRequest("Please give the task a name");
            }

            Data_Structures.PlayerTask taskToSave = new Data_Structures.PlayerTask()
            {
                TaskName = request.TaskName,
                Description = request.Description,
                Updated = DateTime.UtcNow,
                TaskType = request.TaskType,
                TaskOwner = user,
                Completed = false,
                PeriodRate = request.PeriodRate,
                Deleted = false,
            };

            Data_Structures.PlayerTask? existingTask = user?.UserTasks?.Find(x => x.TaskName == request.TaskName && x.TaskType == request.TaskType && !x.Deleted);

            if (existingTask is null)
            {
                if (!await Create(taskToSave))
                {
                    return Conflict("Can't create this task");
                }
                return CreatedAtAction(nameof(GetTask), new { id = taskToSave.ID }, taskToSave.Serialize);
            }
            else
            {
                return Conflict($"Task in this tasktype with the following name: {existingTask.TaskName} already exist");
            }
        }

        [HttpPatch("{ID}/Complete")]
        public async Task<IActionResult> CompleteTask(string ID)
        {
            long? loggedInUser = GetLoggedInUser();
            if (loggedInUser == 0)
            {
                return NotFound("Not logged in");
            }

            Data_Structures.PlayerTask? task = await Database.Tasks.All.Include(x => x.TaskOwner).SingleOrDefaultAsync(x => x.ID.ToString() == ID);
            if (task is null)
            {
                return NotFound("No task found with this Id");
            }

            if (task.LastCompleted.AddMinutes(task.PeriodRate) >= DateTime.UtcNow)
            {
                return BadRequest("The task can't be completed yet");
            }

            task.Completed = true;
            task.LastCompleted = DateTime.UtcNow;

            bool taskUpdateResult = await Update<Data_Structures.PlayerTask>(task);
            if (task.TaskType)
            {
                task.TaskOwner.TotalScore -= DetermineTaskScore(task.PeriodRate);
                task.TaskOwner.CurrentTaskScore -= DetermineTaskScore(task.PeriodRate);
                task.TaskOwner.CompletedBadTasks++;
            }
            else
            {
                task.TaskOwner.TotalScore += DetermineTaskScore(task.PeriodRate);
                task.TaskOwner.CurrentTaskScore += DetermineTaskScore(task.PeriodRate);
                task.TaskOwner.CompletedGoodTasks++;
            }

            bool userScoreUpdate = await Update<Data_Structures.User>(task.TaskOwner);

            TaskHistory taskHistory = new TaskHistory() { CompletedTask = task, TaskId = task.ID, Owner = task.TaskOwner, Completed = DateTime.UtcNow };

            bool taskHistoryCreate = await Create(taskHistory);

            if (taskUpdateResult && userScoreUpdate && taskHistoryCreate)
            {
                return Ok(task.Serialize);
            }
            else
            {
                return NotFound("Couldn't update the task");
            }
        }

        [HttpPatch("{ID}/Update")]
        public async Task<IActionResult> UpdateTask(string ID, [FromBody] TaskRequest request)
        {
            long? loggedInUser = GetLoggedInUser();
            if (loggedInUser == 0)
            {
                return NotFound("Not logged in");
            }

            Data_Structures.PlayerTask? task = await Database.Tasks.All.Include(x => x.TaskOwner).SingleOrDefaultAsync(x => x.ID.ToString() == ID && x.TaskOwner.ID == loggedInUser);
            if (task is null)
            {
                return NotFound("No task found with this Id");
            }
            task.PeriodRate = request.PeriodRate;
            task.TaskName = request.TaskName;
            task.TaskType = request.TaskType;
            task.Description = request.Description;

            if (await Update<Data_Structures.PlayerTask>(task))
            {
                return Ok(task.Serialize);
            }
            else
            {
                return NotFound("Couldn't update the task");
            }
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteTask(string ID)
        {
            long? loggedInUser = GetLoggedInUser();
            if (loggedInUser == 0)
            {
                return NotFound("Not logged in");
            }

            Data_Structures.PlayerTask? task = await Database.Tasks.All.Include(x => x.TaskOwner).SingleOrDefaultAsync(x => x.ID.ToString() == ID && x.TaskOwner.ID == loggedInUser);
            if (task is null)
            {
                return NotFound("No task found with this Id");
            }
            task.Deleted = true;
            if (await Update<Data_Structures.PlayerTask>(task))
            {
                return Ok("Deleted");
            }
            else
            {
                return NotFound("Couldn't delete the task");
            }
        }

        private long DetermineTaskScore(int periodRate)
        {
            switch (periodRate)
            {
                case 60:
                    return (long)TaskScores.Hourly;

                case 120:
                    return (long)TaskScores.EveryTwoHours;

                case 240:
                    return (long)TaskScores.EveryFourHours;

                case 1440:
                    return (long)TaskScores.Daily;

                case 2880:
                    return (long)TaskScores.EveryTwoDays;

                case 10080:
                    return (long)TaskScores.Weekly;

                case 20160:
                    return (long)TaskScores.EveryTwoWeeks;

                case 40320:
                    return (long)TaskScores.Monthly;

                default:
                    Console.WriteLine("Can't find such period rate to score");
                    return 0;
                    break;
            }
        }
    }
}