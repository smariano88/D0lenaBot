using D0lenaBot.Server.App.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D0lenaBot.Server.App.Application.Infrastructure
{
    public interface IUsers
    {
        Task Save(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByChatId(string chatId);
    }
}
