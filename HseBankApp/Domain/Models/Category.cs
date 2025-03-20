using HSEBankApp.Exceptions;
using HSEBankApp.Domain.Enums;

namespace HSEBankApp.Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public OperationType Type { get; set; }
        public string Name { get; set; }

        public Category(OperationType type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Название категории не может быть пустым.");

            Id = Guid.NewGuid();
            Type = type;
            Name = name;
        }
    }
}