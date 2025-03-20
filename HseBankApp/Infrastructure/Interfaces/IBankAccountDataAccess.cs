using HSEBankApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace HSEBankApp.Infrastructure.Interfaces;

public interface IBankAccountDataAccess
{
    void Add(BankAccount account);
    void Update(BankAccount account);
    void Delete(Guid accountId);
    BankAccount GetById(Guid accountId);
    IEnumerable<BankAccount> GetByUserId(Guid userId);
}