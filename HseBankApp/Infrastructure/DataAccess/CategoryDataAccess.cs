using HSEBankApp.Domain.Enums;
using HSEBankApp.Domain.Models;
using HSEBankApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HSEBankApp.Infrastructure.DataAccess
{
    public class CategoryDataAccess : ICategoryDataAccess
    {
        private readonly List<Category> _categories = new();

        public void Add(Category category)
        {
            if (_categories.Any(c => c.Name == category.Name))
            {
                return;
            }
            _categories.Add(category);
        }

        public void Update(Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Type = category.Type;
            }
        }

        public void Delete(Guid categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                _categories.Remove(category);
            }
        }

        public Category GetById(Guid categoryId)
        {
            return _categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public IEnumerable<Category> GetByType(OperationType type)
        {
            return _categories.Where(c => c.Type == type);
        }
    }
}