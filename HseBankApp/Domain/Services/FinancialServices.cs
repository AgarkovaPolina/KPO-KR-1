using HSEBankApp.Domain.Models;
using HSEBankApp.Infrastructure.Interfaces;
using HSEBankApp.Exceptions;
using HSEBankApp.Domain.Enums;

using System;
using System.Collections.Generic;

namespace HSEBankApp.Domain.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly IOperationDataAccess _operationDataAccess;
        private readonly IBankAccountDataAccess _bankAccountDataAccess;

        public FinancialService(
            IOperationDataAccess operationDataAccess,
            IBankAccountDataAccess bankAccountDataAccess)
        {
            _operationDataAccess = operationDataAccess;
            _bankAccountDataAccess = bankAccountDataAccess;
        }

        // Добавление новой операции
        public void AddOperation(Operation operation)
        {
            // Получаем счет и проверяем его существование
            var account = _bankAccountDataAccess.GetById(operation.BankAccountId);
            if (account == null)
                throw new ValidationException("Счет не найден");

            // Проверка баланса для расходных операций
            if (operation.Type == OperationType.Expense && account.Balance < operation.Amount)
                throw new ValidationException("Недостаточно средств на счете");

            // Добавляем операцию
            _operationDataAccess.Add(operation);

            // Обновляем баланс счета
            account.Balance += operation.Type == OperationType.Income 
                ? operation.Amount 
                : -operation.Amount;
            
            _bankAccountDataAccess.Update(account);
        }

        // Обновление существующей операции
        public void UpdateOperation(Operation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            _operationDataAccess.Update(operation);
        }

        // Удаление операции по ID
        public void DeleteOperation(Guid operationId)
        {
            if (operationId == Guid.Empty)
                throw new ArgumentException("ID операции не может быть пустым.");

            _operationDataAccess.Delete(operationId);
        }

        // Получение операции по ID
        public Operation GetOperationById(Guid operationId)
        {
            if (operationId == Guid.Empty)
                throw new ArgumentException("ID операции не может быть пустым.");

            return _operationDataAccess.GetById(operationId);
        }

        // Получение всех операций по ID счёта
        public IEnumerable<Operation> GetOperationsByAccount(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("ID счёта не может быть пустым.");

            return _operationDataAccess.GetByAccountId(accountId);
        }
    }
}