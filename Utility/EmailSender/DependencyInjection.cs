using Helpers.EmailSender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenders
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
           
            string senderEmail = Environment.GetEnvironmentVariable("SenderEmail")??"NA@NA.com";
            
            string senderPassword = Environment.GetEnvironmentVariable("SenderPassword")?? "NA";
            
            string EmailHost = Environment.GetEnvironmentVariable("EmailHost")?? "NA";
            
            string EmailPort = Environment.GetEnvironmentVariable("EmailPort")?? "525";
            

            services.TryAddSingleton<IEmailService>(x => new EmailService(senderEmail, senderPassword, EmailHost, int.Parse(EmailPort)));
            Console.WriteLine($"[Info]----->{nameof(AddEmailSender)} service added");

            return services;
        }

        
    }
}
