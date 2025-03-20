using HSEBankApp.Domain.Models;
using HSEBankApp.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HSEBankApp.Infrastructure.Interfaces
{
    public interface ICategoryDataAccess
    {
        void Add(Category category);
        void Update(Category category);
        void Delete(Guid categoryId);
        Category GetById(Guid categoryId);
        IEnumerable<Category> GetByType(OperationType type);
    }
}