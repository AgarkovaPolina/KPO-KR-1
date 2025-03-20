using HSEBankApp.Domain.Models;
using System;
using System.Collections.Generic;

namespace HSEBankApp.Domain.Services
{
    public interface IFinancialService
    {
        // Добавление новой операции
        void AddOperation(Operation operation);

        // Обновление существующей операции
        void UpdateOperation(Operation operation);

        // Удаление операции по ID
        void DeleteOperation(Guid operationId);

        // Получение операции по ID
        Operation GetOperationById(Guid operationId);

        // Получение всех операций по ID счёта
        IEnumerable<Operation> GetOperationsByAccount(Guid accountId);
    }
}