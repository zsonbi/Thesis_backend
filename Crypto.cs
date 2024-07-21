using System;
using System.Security.Cryptography;
using System.Text;

namespace Thesis
{
    internal class Crypto
    {
        public static int ITERATIONS = 600000;

        public static string HashString(string inputString)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                return Convert.ToBase64String(bytes);
            }
        }

        public static string HashPassword(string password)
        {
            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor: 12);
            return hash;
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }

        public static string EncryptedRead(string file, string key)
        {
            try
            {
                string contents = File.ReadAllText(file);
                return Decrypt(contents, key, "");
            }
            catch
            {
                return "";
            }
        }

        public static bool EncryptedWrite(string file, string content, string key)
        {
            try
            {
                string encrypted = Encrypt(content, key, "");
                File.WriteAllText(file, encrypted);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Encrypt(string clearText, string encryptionKey, string salt)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, Encoding.UTF8.GetBytes(salt), ITERATIONS, HashAlgorithmName.SHA512);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine($"CryptographicException: {e.Message}");
                return "";
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"ArgumentException: {e.Message}");
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return "";
            }
        }

        public static string Decrypt(string cipherText, string encryptionKey, string salt)
        {
            try
            {
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, Encoding.UTF8.GetBytes(salt), ITERATIONS, HashAlgorithmName.SHA512);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine($"CryptographicException: {e.Message}");
                return "";
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"ArgumentException: {e.Message}");
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return "";
            }
        }
    }
}