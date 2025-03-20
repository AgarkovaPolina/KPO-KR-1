using HSEBankApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace HSEBankApp.Infrastructure.Interfaces
{
    public interface IUserDataAccess
    {
        void Add(User user);
        void Update(User user);
        void Delete(Guid userId);
        User GetById(Guid userId);
        User GetByEmail(string email);
        IEnumerable<User> GetAll();
    }
}