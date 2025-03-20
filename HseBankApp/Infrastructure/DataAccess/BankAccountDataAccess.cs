using HSEBankApp.Domain.Models;
using HSEBankApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSEBankApp.Infrastructure.DataAccess
{
    public class BankAccountDataAccess : IBankAccountDataAccess
    {
        private readonly List<BankAccount> _accounts = new();

        public void Add(BankAccount account)
        {
            var curr_account = _accounts.FirstOrDefault(a => a.Id == account.Id);
            if (curr_account == null)
            {
                _accounts.Add(account);
            }
            else
            {
                throw new Exception("Такой аккаунт уже создан(");
            }
        }

        public void Update(BankAccount account)
        {
            var existingAccount = _accounts.FirstOrDefault(a => a.Id == account.Id);
            if (existingAccount != null)
            {
                existingAccount.Name = account.Name;
                existingAccount.Balance = account.Balance;
            }
        }

        public void Delete(Guid accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account != null)
            {
                _accounts.Remove(account);
            }
        }

        public BankAccount GetById(Guid accountId)
        {
            return _accounts.FirstOrDefault(a => a.Id == accountId);
        }

        public IEnumerable<BankAccount> GetByUserId(Guid userId)
        {
            return _accounts.Where(a => a.UserId == userId);
        }
    }
}