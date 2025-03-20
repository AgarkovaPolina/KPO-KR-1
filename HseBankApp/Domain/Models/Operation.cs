using HSEBankApp.Exceptions;
using HSEBankApp.Domain.Enums;

namespace HSEBankApp.Domain.Models
{
    public class Operation
    {
        public Guid Id { get; set; }
        public OperationType Type { get; set; }
        public Guid BankAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }

        public Operation(OperationType type, Guid bankAccountId, decimal amount, DateTime date, Guid categoryId, string description = null)
        {
            if (amount <= 0)
                throw new ValidationException("Сумма операции должна быть положительной.");

            if (bankAccountId == Guid.Empty || categoryId == Guid.Empty)
                throw new ValidationException("Идентификаторы счета и категории должны быть указаны.");

            Id = Guid.NewGuid();
            Type = type;
            BankAccountId = bankAccountId;
            Amount = amount;
            Date = date;
            CategoryId = categoryId;
            Description = description;
        }
    }
}