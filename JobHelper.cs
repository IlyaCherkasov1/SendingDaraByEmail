using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendingDataByEmail
{
    public class JobHelper
    {

        public List<Job> GetJobs()
        {
            List<Job> jobs = new List<Job>();
            string sqlExpression = "SELECT * FROM Job";
            using (var connection = new SqliteConnection("Data Source=API.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string description = reader.GetString(2);
                            string momentTime = reader.GetString(3);
                            int periodicity = reader.GetInt32(4);
                            int apiId = reader.GetInt32(5);

                            Job job1 = new Job(id, name, description, momentTime, periodicity, apiId);
                            jobs.Add(job1);

                        }
                    }
                }
            }
            return jobs;
        }

        public Job GetJob(int id)
        {
            Job job = null;
            string sqlExpression = $"SELECT * FROM Job where id={id}";
            using (var connection = new SqliteConnection("Data Source=API.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            string name = reader.GetString(1);
                            string description = reader.GetString(2);
                            string momentTime = reader.GetString(3);
                            int periodicity = reader.GetInt32(4);
                            int apiId = reader.GetInt32(5);
                            job = new Job(id, name, description, momentTime, periodicity, apiId);


                        }
                    }
                }
            }
            return job;
        }
    }
}
