namespace HSEBankApp.Domain.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }

        public BankAccount(string name, decimal initialBalance, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Название счета не может быть пустым.");

            if (initialBalance < 0)
                throw new Exception("Начальный баланс не может быть отрицательным.");

            Id = Guid.NewGuid();
            Name = name;
            Balance = initialBalance;
            UserId = userId;
        }
    }
}