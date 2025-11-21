using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActivity
{
    public class UserActivityLogCommand : IRequest
    {
        public string UserId { get; }
        public string Activity { get; }

        public UserActivityLogCommand(string userId, string activity)
        {
            UserId = userId;
            Activity = activity;
        }
    }
}
