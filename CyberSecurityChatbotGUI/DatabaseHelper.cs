using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CyberSecurityChatbotGUI
{
    public static class DatabaseHelper
    {
        private static string _connectionString
            = "Server =.\\SQLEXPRESS;Database=CyberBotDB;Trusted_Connection=True;TrustServerCertificate=True;";
        // ── Test the connection 
        // Returns true if the connection works, false if something is wrong
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ── Add a new task to the database
        // Returns true if the task was saved successfully
        public static bool AddTask(string title, string description, string reminderDate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    string query =
                        "INSERT INTO Tasks (Title, Description, ReminderDate) " +
                        "VALUES (@title, @description, @reminder)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@reminder", reminderDate);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ── Get all tasks from the database
        // Returns a list of all CyberTask objects
        public static List<CyberTask> GetAllTasks()
        {
            List<CyberTask> tasks = new List<CyberTask>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CyberTask task = new CyberTask();
                        task.TaskId = Convert.ToInt32(reader["TaskId"]);
                        task.Title = reader["Title"].ToString() ?? string.Empty;
                        task.Description = reader["Description"].ToString() ?? string.Empty;
                        task.ReminderDate = reader["ReminderDate"].ToString() ?? string.Empty;
                        task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                        task.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm");

                        tasks.Add(task);
                    }
                }
            }
            catch (Exception)
            {
                // Return whatever was collected before the error
            }

            return tasks;
        }

        // ── Mark a task as completed 
        // Returns true if the update was successful
        public static bool CompleteTask(int taskId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    string query = "UPDATE Tasks SET IsCompleted = 1 WHERE TaskId = @id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", taskId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ── Delete a task from the database 
        // Returns true if the delete was successful
        public static bool DeleteTask(int taskId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    string query = "DELETE FROM Tasks WHERE TaskId = @id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@id", taskId);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ── Get only pending tasks
        // Returns a list of tasks that have not been completed yet
        public static List<CyberTask> GetPendingTasks()
        {
            List<CyberTask> tasks = new List<CyberTask>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM Tasks WHERE IsCompleted = 0 ORDER BY CreatedAt DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CyberTask task = new CyberTask();
                        task.TaskId = Convert.ToInt32(reader["TaskId"]);
                        task.Title = reader["Title"].ToString() ?? string.Empty;
                        task.Description = reader["Description"].ToString() ?? string.Empty;
                        task.ReminderDate = reader["ReminderDate"].ToString() ?? string.Empty;
                        task.IsCompleted = false;
                        task.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm");

                        tasks.Add(task);
                    }
                }
            }
            catch (Exception)
            {
                // Return whatever was collected before the error
            }

            return tasks;
        }
    }
}
