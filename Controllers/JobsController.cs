using ChoETL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendingDataByEmail.Data.Models;
using SendingDataByEmail.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SendingDataByEmail.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IApisService _apisService;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public JobsController(IApisService apisService)
        {
            _apisService = apisService;
        }


        [HttpGet]
        public ActionResult GetJobs()
        {       
            List<Job> jobs = new List<Job>();
            string sqlExpression = "SELECT * FROM Job";
            using (var connection = new SqliteConnection(connectionString))
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

                            Job job = new Job(id, name, description, momentTime, periodicity,apiId);
                            jobs.Add(job);

                        }
                    }
                }
            }
            return new JsonResult(jobs);
        }

        [HttpGet("{id}")]
        public ActionResult GetJob(int id)
        {
            Job job = null;
            string sqlExpression = "SELECT * FROM Job where id=(@id)";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);
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
                            job = new Job(id,name, description, momentTime, periodicity, apiId);                     
                        }
                    }
                }
            }
            return new JsonResult(job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(int id, Job job)
        {
            if (id != job.Id)
            {
                return BadRequest();
            }

            string sqlExpression = "UPDATE Job SET Name=(@name), Description=(@desc), " +
                $"MomentTime=(@moment), Periodicity=(@periodicity), ApiId=(@apiId) WHERE Id=(@id)";


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);

                SqliteParameter nameParam = new SqliteParameter("@name", job.Name);
                SqliteParameter descParam = new SqliteParameter("@desc", job.Description);
                SqliteParameter timeParam = new SqliteParameter("@moment", job.MomentTime);
                SqliteParameter periodicityParam = new SqliteParameter("@periodicity", job.Periodicity);
                SqliteParameter apiIdParam = new SqliteParameter("@apiId", job.ApiId);
                SqliteParameter idParam = new SqliteParameter("@id", id);

                SqliteParameter[] sqliteParameters = new SqliteParameter[]
                {nameParam, descParam, timeParam, periodicityParam, apiIdParam,idParam };
                command.Parameters.AddRange(sqliteParameters);

                int i = command.ExecuteNonQuery();
                ApiHelper apiHelper = new ApiHelper(_apisService);
                await  apiHelper.GetApiData(job);
                EmailSheduler.Start(job.Name, job.MomentTime, job.Periodicity);
            }
            return NoContent();
        }



        [HttpPost]
        public async Task<IActionResult> PostJob(Job job)
        {
            string sqlExpression = "INSERT INTO Job (Name, Description, MomentTime, Periodicity, ApiId) " +
                $"VALUES (@name, @desc, @moment, @periodicity, @apiId)";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression,connection);

                SqliteParameter nameParam = new SqliteParameter("@name", job.Name);
                SqliteParameter descParam = new SqliteParameter("@desc", job.Description);
                SqliteParameter timeParam = new SqliteParameter("@moment", job.MomentTime);
                SqliteParameter periodicityParam = new SqliteParameter("@periodicity", job.Periodicity);
                SqliteParameter apiIdParam = new SqliteParameter("@apiId", job.ApiId);

                SqliteParameter[] sqliteParameters = new SqliteParameter[]
                {nameParam, descParam, timeParam, periodicityParam, apiIdParam };
                command.Parameters.AddRange(sqliteParameters);

                ApiHelper apiHelper = new ApiHelper(_apisService);
                await apiHelper.GetApiData(job);
                EmailSheduler.Start(job.Name, job.MomentTime, job.Periodicity);
            }
            return CreatedAtAction("GetJob", new { id = job.Id }, job);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteJob(int id)
        {
              string sqlExpression = "DELETE FROM Job WHERE Id=@id";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                JobHelper jobHelper = new JobHelper();
                string JobName = jobHelper.GetJob(id).Name;
                EmailSheduler.Delete(JobName);
                int number = command.ExecuteNonQuery();
            }
            return NoContent();
        }

        [HttpPost] 
        [Route("IsDupeField")]
        public bool IsDupeField(int jobId, string fieldName, string fieldValue) 
        {
            JobHelper jobHelper = new JobHelper();
            List<Job> jobs = jobHelper.GetJobs();
            switch (fieldName)
            {
                case "name": return jobs.Any(c => c.Name == fieldValue && c.Id != jobId);
                case "description": return jobs.Any(c => c.Description == fieldValue && c.Id != jobId);
                case "momentTime": return jobs.Any(c => c.MomentTime == fieldValue && c.Id != jobId);
                case "periodicity": return jobs.Any(c => c.Periodicity == Int32.Parse(fieldValue) && c.Id != jobId);
                case "ApiId": return jobs.Any(c => c.ApiId == Int32.Parse(fieldValue) && c.Id != jobId);
                default:
                    return false;
            }
           
        }

      

  


    }
}
