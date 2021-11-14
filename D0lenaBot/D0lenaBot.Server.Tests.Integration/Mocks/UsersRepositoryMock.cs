using D0lenaBot.Server.App.Application.Infrastructure;
using D0lenaBot.Server.App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D0lenaBot.Server.Tests.Integration.Mocks
{
    public class UsersRepositoryMock : IUsers
    {
        private List<User> users = new List<User>();
        public async Task Delete(User user)
        {
            this.users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return this.users;
        }

        public async Task<User> GetByChatId(string chatId)
        {
            return this.users.Where(u => u.Id == chatId).SingleOrDefault();
        }

        public async Task Save(User user)
        {
            this.users.Add(user);
        }
    }
}
