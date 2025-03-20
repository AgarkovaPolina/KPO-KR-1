using HSEBankApp.Domain.Enums;
using HSEBankApp.Domain.Models;
using HSEBankApp.Domain.Services;
using HSEBankApp.Infrastructure.DataAccess;
using HSEBankApp.Infrastructure.Interfaces;
using HSEBankApp.Exceptions;

namespace HSEBankApp
{
    class Program
    {
        private static IFinancialService _financialService;
        private static IBankAccountDataAccess _bankAccountDataAccess;
        private static ICategoryDataAccess _categoryDataAccess;
        private static IUserDataAccess _userDataAccess;

        static void Main(string[] args)
        {
            InitializeDependencies();
            CreateDefaultCategories();

            while (true)
            {
                MainMenu();
            }
        }

        private static void InitializeDependencies()
        {
            _bankAccountDataAccess = new BankAccountDataAccess();
            _categoryDataAccess = new CategoryDataAccess();
            _userDataAccess = new UserDataAccess();
            _financialService = new FinancialService(new OperationDataAccess(), _bankAccountDataAccess);
        }

        private static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== HSE Bank App ===");
            Console.WriteLine("1. Добавить нового пользователя");
            Console.WriteLine("2. Показать доступные категории");
            Console.WriteLine("3. Создать новую категорию");
            Console.WriteLine("4. Выбрать пользователя для просмотра информации об операциях и счетах");
            Console.WriteLine("5. Выйти из программы");
            HandleMenuChoice(GetValidChoice(1, 5));
        }

        private static void HandleMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1: AddUser(); break;
                case 2: ShowAllCategories(); break;
                case 3: CreateCategory(); break;
                case 4: SelectUser(); break;
                case 5: Environment.Exit(0); break;
            }
        }
        
        private static void ShowAllCategories()
        {
            var incomeCategories = _categoryDataAccess.GetByType(OperationType.Income).ToList();
            var expenseCategories = _categoryDataAccess.GetByType(OperationType.Expense).ToList();

            Console.WriteLine("\nКатегории доходов:");
            for (int i = 0; i < incomeCategories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {incomeCategories[i].Name}");
            }

            Console.WriteLine("\nКатегории расходов:");
            for (int i = 0; i < expenseCategories.Count; i++)
            {
                Console.WriteLine($"{incomeCategories.Count + i + 1}. {expenseCategories[i].Name}");
            }
            
            Console.ReadKey();
        }
        
        private static void AddUser()
        {
            var email = GetValidInput("Введите email: ", ValidateEmail, "Некорректный email");
            if (_userDataAccess.GetByEmail(email) != null)
            {
                ShowError("Пользователь с таким email уже существует");
                return;
            }

            var name = GetValidInput("Введите имя: ", ValidateName, "Имя не может быть пустым");
            
            try
            {
                _userDataAccess.Add(new User(name, email));
                ShowSuccess("Пользователь создан!");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private static void SelectUser()
        {
            var users = _userDataAccess.GetAll().ToList();
            if (!users.Any())
            {
                ShowError("Нет доступных пользователей");
                return;
            }

            Console.WriteLine("Список пользователей:");
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {users[i].Email} ({users[i].Name})");
            }

            var choice = GetValidChoice(1, users.Count, "Выберите пользователя: ");
            UserMenu(users[choice - 1]);
        }

        private static void CreateDefaultCategories()
        {
            var defaultCategories = new List<Category>
            {
                new(OperationType.Income, "Зарплата"),
                new(OperationType.Income, "Кэшбэк"),
                new(OperationType.Expense, "Здоровье"),
                new(OperationType.Expense, "Кафе"),
                new(OperationType.Expense, "Транспорт"),
                new(OperationType.Expense, "Развлечения"),
                new(OperationType.Expense, "Образование"),
                new(OperationType.Expense, "Коммунальные услуги"),
                new(OperationType.Expense, "Одежда"),
                new(OperationType.Expense, "Спорт")
            };

            defaultCategories.ForEach(c => _categoryDataAccess.Add(c));
            ShowSuccess("Базовые категории созданы!");
        }

        private static void CreateCategory()
        {
            var type = (OperationType)GetValidChoice(0, 1, "Тип категории (0-Доход, 1-Расход): ");
            var name = GetValidInput("Название категории: ", ValidateName, "Название не может быть пустым");
            
            _categoryDataAccess.Add(new Category(type, name));
            ShowSuccess("Категория создана!");
        }

        private static void UserMenu(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Работа с пользователем: {user.Email}");
                Console.WriteLine("1. Создать счет");
                Console.WriteLine("2. Список счетов");
                Console.WriteLine("3. Добавить операцию");
                Console.WriteLine("4. История операций");
                Console.WriteLine("5. Аналитика");
                Console.WriteLine("6. Назад");
                
                var choice = GetValidChoice(1, 6);
                if (choice == 6) return;

                try
                {
                    HandleUserMenuChoice(choice, user);
                    if (!AskToContinue()) return;
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        private static void HandleUserMenuChoice(int choice, User user)
        {
            switch (choice)
            {
                case 1: CreateAccount(user); break;
                case 2: ShowAccounts(user); break;
                case 3: AddTransaction(user); break;
                case 4: ShowTransactions(user); break;
                case 5: ShowAnalytics(user); break;
            }
        }

        private static void CreateAccount(User user)
        {
            var name = GetValidInput("Название счета: ", ValidateName, "Название не может быть пустым");
            var balance = GetValidDecimal("Начальный баланс: ");
            
            _bankAccountDataAccess.Add(new BankAccount(name, balance, user.Id));
            ShowSuccess($"Счет {name} создан у пользователя {user.Name} с первоначальным балансом {balance}.");
        }

        private static void ShowAccounts(User user)
        {
            var accounts = GetUserAccounts(user);
            if (!accounts.Any())
            {
                ShowError("У пользователя нет счетов");
                return;
            }
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].Name} ({accounts[i].Balance} ед.)");
            }
        }

        private static void AddTransaction(User user)
        {
            var accounts = _bankAccountDataAccess.GetByUserId(user.Id).ToList();
    
            if (!accounts.Any())
            {
                ShowError("У пользователя нет счетов");
                return;
            }

            Console.WriteLine("Доступные счета:");
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].Name} (Баланс: {accounts[i].Balance} ₽)");
            }

            var accountNumber = GetValidChoice(1, accounts.Count, "Выберите счет: ");
            var selectedAccount = accounts[accountNumber - 1];

            var type = (OperationType)GetValidChoice(0, 1, "Тип операции (0-Доход, 1-Расход): ");

            var category = SelectCategory(type);

            var amount = GetValidDecimal("Сумма: ");

            var description = GetOptionalInput("Описание: ");

            try
            {
                _financialService.AddOperation(new Operation(
                    type,
                    selectedAccount.Id,
                    amount,
                    DateTime.Now,
                    category.Id,
                    description
                ));
        
                ShowSuccess($"Операция успешно добавлена на счет '{selectedAccount.Name}'!");
            }
            catch (ValidationException ex)
            {
                ShowError(ex.Message);
            }
        }

        private static void ShowTransactions(User user)
        {
            var accounts = GetUserAccounts(user);
            if (!accounts.Any())
            {
                ShowError($"На данный момент у клиента {user.Name} отсутвуют счета.");
                return;
            }

            Console.WriteLine("Список счетов:");
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].Name}");
            }

            var accountChoice = GetValidChoice(1, accounts.Count, "Выберите счет: ");
            var selectedAccount = accounts[accountChoice - 1];

            var operations = _financialService.GetOperationsByAccount(selectedAccount.Id);
            
            Console.WriteLine($"\nОперации по счету {selectedAccount.Name}:");
            foreach (var op in operations)
            {
                var category = _categoryDataAccess.GetById(op.CategoryId);
                Console.WriteLine($"{op.Date:g} | {op.Type} | {category.Name} | {op.Amount} ед.");
            }
        }

        private static void ShowAnalytics(User user)
        {
            var accounts = GetUserAccounts(user);
            if (!accounts.Any())
            {
                Console.WriteLine("На данный момент мне нечего анализировать:(");
            }
            foreach (var account in accounts)
            {
                var operations = _financialService.GetOperationsByAccount(account.Id);
                var income = operations.Where(o => o.Type == OperationType.Income).Sum(o => o.Amount);
                var expense = operations.Where(o => o.Type == OperationType.Expense).Sum(o => o.Amount);
                
                Console.WriteLine($"\nСчет: {account.Name}");
                Console.WriteLine($"Доходы: {income} ед.");
                Console.WriteLine($"Расходы: {expense} ед.");
                Console.WriteLine($"Баланс: {account.Balance} ед.");
                Console.WriteLine($"Чистая прибыль: {income - expense} ед.");
            }
        }

        private static List<BankAccount> GetUserAccounts(User user) => 
            _bankAccountDataAccess.GetByUserId(user.Id).ToList();

        private static BankAccount SelectAccount(User user, string prompt)
        {
            var accounts = GetUserAccounts(user);
            for (int i = 0; i < accounts.Count; i++)
                Console.WriteLine($"{i + 1}. {accounts[i].Name}");

            return accounts[GetValidChoice(1, accounts.Count, prompt) - 1];
        }

        private static Category SelectCategory(OperationType type)
        {
            var categories = _categoryDataAccess.GetByType(type).ToList();
            for (int i = 0; i < categories.Count; i++)
                Console.WriteLine($"{i + 1}. {categories[i].Name}");

            return categories[GetValidChoice(1, categories.Count, "Выберите категорию: ") - 1];
        }

        private static bool AskToContinue()
        {
            Console.Write("\nПродолжить работу с текущим пользователем? (y/n): ");
            return Console.ReadLine()?.ToLower() == "y";
        }

        private static int GetValidChoice(int min, int max, string prompt = "Выберите: ")
        {
            int choice;
            do
            {
                Console.Write(prompt);
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max);
            
            return choice;
        }

        private static string GetValidInput(string prompt, Func<string, bool> validator, string errorMessage)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (validator(input)) break;
                ShowError(errorMessage);
            } while (true);
            
            return input;
        }

        private static decimal GetValidDecimal(string prompt)
        {
            decimal value;
            do
            {
                Console.Write(prompt);
            } while (!decimal.TryParse(Console.ReadLine(), out value) || value < 0);
            
            return value;
        }

        private static string GetOptionalInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static bool ValidateEmail(string email) => 
            !string.IsNullOrWhiteSpace(email) && email.Contains("@");

        private static bool ValidateName(string name) => 
            !string.IsNullOrWhiteSpace(name);

        private static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.ReadKey();
        }

        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка: " + message);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}