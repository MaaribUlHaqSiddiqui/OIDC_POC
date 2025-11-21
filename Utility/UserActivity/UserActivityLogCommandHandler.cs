using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActivity
{
    public class UserActivityLogCommandHandler: IRequestHandler<UserActivityLogCommand>
    {
        private readonly IUserActivity _userActivity;

        public UserActivityLogCommandHandler(IUserActivity userActivity)
        {
            _userActivity = userActivity;
        }

        public Task Handle(UserActivityLogCommand request, CancellationToken cancellationToken)
        {
            _userActivity.LogActivityAsync(request.UserId, request.Activity);
            return Task.CompletedTask;
        }
    }
}
