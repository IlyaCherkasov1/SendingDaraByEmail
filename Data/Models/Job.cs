using Microsoft.AspNetCore.Razor.Language.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendingDataByEmail.Data.Models
{
    public class Job
    {
        public Job(int id, string name, string description, string momentTime, int periodicity, int apiId)
        {
            Id = id;
            Name = name;
            Description = description;
            MomentTime = momentTime;
            Periodicity = periodicity;
            ApiId = apiId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MomentTime { get; set; }
        public int Periodicity { get; set; }       
        public int ApiId { get; set; }
       
    }
}
