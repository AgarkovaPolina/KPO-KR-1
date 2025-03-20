using HSEBankApp.Domain.Models;
using HSEBankApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSEBankApp.Infrastructure.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly List<User> _users = new();

        public void Add(User user)
        {
            if (_users.Any(u => u.Email == user.Email))
            {
                throw new ArgumentException("Пользователь с таким e-mail уже существует");
            }
            _users.Add(user);
        }

        public void Update(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
            }
        }

        public void Delete(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        public User GetById(Guid userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }
    }
}