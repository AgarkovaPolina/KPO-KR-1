using HSEBankApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace HSEBankApp.Infrastructure.Interfaces
{
    public interface IOperationDataAccess
    {
        void Add(Operation operation);
        void Update(Operation operation);
        void Delete(Guid operationId);
        Operation GetById(Guid operationId);
        IEnumerable<Operation> GetByAccountId(Guid accountId);
    }
}