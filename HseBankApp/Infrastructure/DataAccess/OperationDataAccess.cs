using HSEBankApp.Domain.Models;
using HSEBankApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSEBankApp.Infrastructure.DataAccess
{
    public class OperationDataAccess : IOperationDataAccess
    {
        private readonly List<Operation> _operations = new();

        public void Add(Operation operation)
        {
            _operations.Add(operation);
        }

        public void Update(Operation operation)
        {
            var existingOperation = _operations.FirstOrDefault(o => o.Id == operation.Id);
            if (existingOperation != null)
            {
                existingOperation.Type = operation.Type;
                existingOperation.BankAccountId = operation.BankAccountId;
                existingOperation.Amount = operation.Amount;
                existingOperation.Date = operation.Date;
                existingOperation.Description = operation.Description;
                existingOperation.CategoryId = operation.CategoryId;
            }
        }

        public void Delete(Guid operationId)
        {
            var operation = _operations.FirstOrDefault(o => o.Id == operationId);
            if (operation != null)
            {
                _operations.Remove(operation);
            }
        }

        public Operation GetById(Guid operationId)
        {
            return _operations.FirstOrDefault(o => o.Id == operationId);
        }

        public IEnumerable<Operation> GetByAccountId(Guid accountId)
        {
            return _operations.Where(o => o.BankAccountId == accountId);
        }
    }
}