using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActivity
{
    public interface IUserActivity
    {
        Task LogActivityAsync(string userId, string activity);
        Task<IEnumerable<string>> GetActivityAsync(string userId);
    }
}
