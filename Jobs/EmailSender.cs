using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quartz;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SendingDataByEmail.Jobs
{
    public class EmailSender :  IJob
    {
       
        public async Task Execute(IJobExecutionContext context)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();

            using (MailMessage message = new MailMessage(config["Smtp:From"], $"{Policies.Email}"))
            {
                message.Subject = "Newsletter";
                message.Body = "Send Data";

                string jobName = context.JobDetail.Key.Name;              
                Attachment data = new Attachment(jobName + ".csv", "text/csv");
                message.Attachments.Add(data);

                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = bool.Parse(config["Smtp:EnableSsl"]),
                    Host = config["Smtp:Host"],
                    Port = int.Parse(config["Smtp:Port"]),
                    Credentials = new NetworkCredential(config["Smtp:From"], config["Smtp:Password"])
                })
                {
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
