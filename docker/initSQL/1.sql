CREATE DATABASE Thesis;
CREATE USER IF NOT EXISTS 'server_access'@'%' IDENTIFIED BY 'test_password';
GRANT ALL PRIVILEGES ON *.* TO 'server_access'@'%';
FLUSH PRIVILEGES;