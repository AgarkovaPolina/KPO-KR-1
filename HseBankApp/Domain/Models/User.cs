using HSEBankApp.Exceptions;

namespace HSEBankApp.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<BankAccount> BankAccounts { get; set; } = new();

        public User(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Имя пользователя не может быть пустым.");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ValidationException("Некорректный email.");

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }

        public void AddBankAccount(BankAccount account)
        {
            BankAccounts.Add(account);
        }
    }
}