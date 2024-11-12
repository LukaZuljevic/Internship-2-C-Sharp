using System;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Transactions;

Dictionary<int, (Tuple<string, string, DateTime> userInfo, Dictionary<string, double> accounts)> users = new Dictionary<int, (Tuple<string, string, DateTime>, Dictionary<string, double>)>();

users.Add(1, (new Tuple<string, string, DateTime>("Marko", "Livaja", new DateTime(1995, 10, 2)),
    new Dictionary<string, double>
    {
        { "Tekuci", 696.22 },
        { "Ziro", -101.13 },
        { "Prepaid", 120.1 }
    }
));

users.Add(2, (new Tuple<string, string, DateTime>("Ante", "Antic", new DateTime(2003, 10, 6)),
    new Dictionary<string, double>
    {
        { "Tekuci", 100.00 },
        { "Ziro", 200 },
        { "Prepaid", -12 }
    }
));

Dictionary<int, (int, string, double, string, string, string, DateTime)> transactions = new Dictionary<int, (int, string, double, string, string, string, DateTime)>();

transactions.Add(1, (1, "Tekuci", 122.22, "Standarna transakcija", "prihod", "honorar", new DateTime(2022, 1, 20)));
transactions.Add(2, (1, "Tekuci", 318, "Standarna transakcija", "rashod", "sport", new DateTime(2022, 1, 22)));
transactions.Add(3, (1, "Tekuci", 10, "Online transakcija", "rashod", "sport", new DateTime(2001, 12, 23)));
transactions.Add(4, (1, "Tekuci", 1022.1, "Standarna transakcija", "prihod", "placa", new DateTime(2023, 5, 11)));
transactions.Add(5, (1, "Tekuci", 120.1, "Online transakcija", "rashod", "hrana", new DateTime(2013, 5, 1)));
transactions.Add(6, (1, "Ziro", 101.13, "Income transakcija", "rashod", "placa", new DateTime(2022, 10, 10)));
transactions.Add(7, (1, "Prepaid", 120.1, "Standarna transakcija", "rashod", "hrana", new DateTime(2022, 11, 11)));
transactions.Add(8, (2, "Ziro", 200, "Standarna transakcija", "prihod", "honorar", new DateTime(2024, 4, 22)));
transactions.Add(9, (2, "Prepaid", 12, "Standarna transakcija", "rashod", "poklon", new DateTime(2020, 3, 25)));

while (true)
{
    Console.Clear();

    Console.WriteLine("1 - Korisnici\n2 - Računi\n3 - Izlaz iz aplikacije");
    var menuSelection = Console.ReadLine();

    switch (menuSelection)
    {
        case "1":
            UsersMenu();
            break;

        case "2":
            AccountsMenu();
            break;

        case "3":
            return;

        default:
            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
            break;
    }
}

void UsersMenu()
{
    Console.Clear();

    while (true)
    {
        Console.WriteLine("1 - Unos novog korisnika\n2 - Brisanje korisnika\n3 - Uređivanje korisnika\n4 - Pregled korisnika\n5 - Povratak na main menu");
        var usersMenuSelection = Console.ReadLine();

        switch (usersMenuSelection)
        {
            case "1":
                NewUserEntry();
                break;

            case "2":
                DeleteUser();
                break;

            case "3":
                EditUser();
                break;

            case "4":
                ReviewUser();
                break;

            case "5":
                Console.Clear();
                return;

            default:
                Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
                break;
        }
    }
}

void AccountsMenu() 
{
    Console.Write("Unesi ime i prezime: ");
    var fullName = Console.ReadLine();

    if (fullName.Split(" ").Length < 2)
    {
        Console.WriteLine("Unesi puno ime i prezime");
        AccountsMenu();
    }

    var firstName = fullName.Split(" ")[0];
    var lastName = fullName.Split(" ")[1];

    var accountUser = users.FirstOrDefault(user =>
        user.Value.userInfo.Item1.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
        user.Value.userInfo.Item2.Equals(lastName, StringComparison.OrdinalIgnoreCase));

    if (accountUser.Key == 0)
    {
        Console.WriteLine("Taj korisnik ne postoji!");
        AccountsMenu();
    }

    Console.Clear();
    Console.WriteLine("Odaberi racun:\n1 - Tekuci\n2 - Ziro\n3 - Prepaid");
    var accountType = Console.ReadLine();

    while(accountType != "1" && accountType != "2" && accountType != "3")
    {
        Console.WriteLine("Krivi unos! Odaberi jednu od ponudenih opcija.");
        Console.WriteLine("Odaberi racun:\n1 - Tekuci\n2 - Ziro\n3 - Prepaid");
        accountType = Console.ReadLine();
    }

    accountType = accountType switch
    {
        "1" => "Tekuci",
        "2" => "Ziro",
        "3" => "Prepaid"
    };

    Console.Clear();

    while (true)
    {
        Console.WriteLine("1 - Unos nove transakcije\n2 - Brisanje transakcije\n3 - Uređivanje transakcije\n4 - Pregled transakcija\n5 - Financijsko izvješće\n6 - Povratak na main menu");
        var usersMenuSelection = Console.ReadLine();

        switch (usersMenuSelection)
        {
            case "1":
                NewTransaction(accountType, accountUser.Key);
                break;

            case "2":
                DeleteTransaction(accountType, accountUser.Key);
                break;

            case "3":
                EditTransaction(accountType, accountUser.Key);
                break;

            case "4":
                ReviewTransaction(accountType, accountUser.Key);
                break;

            case "5":
                FinancialReport(accountType, accountUser.Key);
                break;

            case "6":
                return;

            default:
                Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
                break;
        }
    }
}

void PrintUsers()
{
    foreach (var user in users)
    {
        Console.WriteLine($"ID: {user.Key} IME: {user.Value.userInfo.Item1} PREZIME: {user.Value.userInfo.Item2} DATUM ROĐENJA: {user.Value.userInfo.Item3}");
    }
}

void NewUserEntry()
{
    Console.Write("Unesi Id: ");
    var userInput = Console.ReadLine();

    bool isId = int.TryParse(userInput, out int userId);

    if (isId && users.ContainsKey(userId))
    { 
         Console.WriteLine("Taj Id je zauzet!");
        return;
    }

    Console.Write("Unesi ime: ");
    var firstName = Console.ReadLine();

    Console.Write("Unesi prezime: ");
    var lastName = Console.ReadLine();

    Console.Write("Unesi datum rođenja(yyyy-mm-dd): ");
    DateTime birthDate = DateTime.Parse(Console.ReadLine());

    var userInfo = Tuple.Create(firstName, lastName, birthDate);

    var accounts = new Dictionary<string, double>
    {
        { "Tekuci", 100 },
        { "Ziro", 0 },
        { "Prepaid", 0 }
    };

    users[userId] = (userInfo, accounts);
}

void DeleteUser()
{
    Console.Clear();

    Console.WriteLine("a) Po ID-u\nb) Po imenu i prezimenu");
    var idSelection = Console.ReadLine();

    if (idSelection == "a")
    {
        PrintUsers();

        Console.Write("Unesi ID: ");
        var userInput = Console.ReadLine();

        bool isId = int.TryParse(userInput, out int userId);

        if (isId)
        {
            if (users.ContainsKey(userId))
            {
                users.Remove(userId);
                PrintUsers();
            }
            else
            {
                Console.WriteLine("Taj korisnik ne postoji");
            }
        }
    }
    else if (idSelection == "b")
    {
        PrintUsers();

        Console.Write("Unesi ime i prezime: ");
        var fullName = Console.ReadLine();

        if (fullName.Split(" ").Length < 2)
        {
            Console.WriteLine("Unesi puno ime i prezime");
            DeleteUser();
        }

        var firstName = fullName.Split(" ")[0];
        var lastName = fullName.Split(" ")[1];

        var userToDelete = users.FirstOrDefault(user =>
            user.Value.userInfo.Item1.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
            user.Value.userInfo.Item2.Equals(lastName, StringComparison.OrdinalIgnoreCase));

        if (userToDelete.Key != 0)
        {
            users.Remove(userToDelete.Key);
            PrintUsers();
        }
        else
        {
            Console.WriteLine("Taj korisnik ne postoji");
        }
    }
    else
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        DeleteUser();
    }
}

void EditUser()
{
    Console.Clear();

    PrintUsers();

    Console.Write("Unesi ID korisnika: ");
    var userInput = Console.ReadLine();

    bool isId = int.TryParse(userInput, out int userId);

    if (isId)
    {
        if (users.ContainsKey(userId))
        {
            Console.WriteLine("Unesi nove informacije o korisniku: ");

            Console.Write("Unesi ime: ");
            var firstName = Console.ReadLine();

            Console.Write("Unesi prezime: ");
            var lastName = Console.ReadLine();

            Console.Write("Unesi datum rođenja(yyyy-mm-dd): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine());

            var userInfo = Tuple.Create(firstName, lastName, birthDate);

            var currentUser = users[userId];

            users[userId] = (userInfo, currentUser.accounts);

            PrintUsers();
        }
        else
        {
            Console.WriteLine("Taj korisnik ne postoji");
        }
    }
}

void ReviewUser()
{
    Console.Clear();

    Console.WriteLine("a) ispis svih korisnika abecedno po prezimenu\nb) svih onih koji imaju više od 30 godina\nc) svih onih koji imaju barem jedan račun u minusu\n");
    var reviewType = Console.ReadLine();

    while (reviewType != "a" && reviewType != "b" && reviewType != "c")
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        reviewType = Console.ReadLine();
    }

    switch (reviewType)
    {
        case "a":
           var sortedByLastName = users.OrderBy(user => user.Value.userInfo.Item2);
            
           foreach(var user in sortedByLastName)
           {
                Console.WriteLine($"ID: {user.Key} IME: {user.Value.userInfo.Item1} PREZIME: {user.Value.userInfo.Item2} DATUM ROĐENJA: {user.Value.userInfo.Item3}");
           }
           break;

        case "b":
            foreach(var user in users)
            {
                var userBirthDate = user.Value.userInfo.Item3;
                DateTime today = DateTime.Today;
                int age = today.Year - userBirthDate.Year;

                if (userBirthDate > today.AddYears(-age))
                {
                    age--;
                }

                if(age > 30)
                {
                    Console.WriteLine($"ID: {user.Key} IME: {user.Value.userInfo.Item1} PREZIME: {user.Value.userInfo.Item2} DATUM ROĐENJA: {user.Value.userInfo.Item3}");
                }

            }
            break;

        case "c":
            
            foreach(var user in users)
            {
                var userAccounts = user.Value.accounts;
                foreach(var account in userAccounts)
                {
                    if(account.Value < 0)
                    {
                        Console.WriteLine($"ID: {user.Key} IME: {user.Value.userInfo.Item1} PREZIME: {user.Value.userInfo.Item2} DATUM ROĐENJA: {user.Value.userInfo.Item3}");
                        break;
                    }
                }
            }
            break;

        default:
            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
            break;

    }
}

void PrintTransactions(int accountUser, string accountType)
{
    Console.WriteLine();
    Console.WriteLine($"Transakcije na {accountType} racunu: ");

    foreach (var transaction in transactions)
    {
        if (transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
        {
            Console.WriteLine(transaction);
        }
    }
    Console.WriteLine();
}

void NewTransaction(string accountType, int accountUser)
{
    Console.Clear();

    var transactionTimeType = TransactionTimeInput();

    Console.Write("Iznos transakcije: ");
    var transactionAmount = int.Parse(Console.ReadLine());

    while(transactionAmount < 0)
    {
        Console.WriteLine("Iznos transakcije ne smije biti negativan!");
        Console.Write("Iznos transakcije: ");
        transactionAmount = int.Parse(Console.ReadLine());
    }

    Console.Write("Opis transakcije(po defaultu 'Standarna transakcija'): ");
    var transactionDescription = Console.ReadLine();

    if(transactionDescription == "")
    {
        transactionDescription = "Standardna transakcija";
    }

    Console.Write("Tip transakcije(prihod ili rashod): ");
    var transactionType = Console.ReadLine();

    while(transactionType != "prihod" && transactionType != "rashod")
    {
        Console.Write("Unesi 'prihod' ili 'rashod': ");
        transactionType = Console.ReadLine();
    }

    var transactionCategory = "";
    do
    {
        Console.Write(transactionType == "prihod"
        ? "Kategorija transakcije (placa, honorar, poklon): "
        : "Kategorija transakcije (hrana, prijevoz, sport): ");
        transactionCategory = Console.ReadLine();

    }
    while (transactionType == "prihod"
           ? (transactionCategory != "placa" && transactionCategory != "honorar" && transactionCategory != "poklon")
           : (transactionCategory != "hrana" && transactionCategory != "prijevoz" && transactionCategory != "sport")) ;

    if (transactionType == "rashod")
    {
        transactionAmount = -transactionAmount;
    }

    if (accountType == "Tekuci")
    {
        users[accountUser].accounts[accountType] += transactionAmount;
    }
    else if (accountType == "Ziro")
    {
        users[accountUser].accounts[accountType] += transactionAmount;
    }
    else
    {
        users[accountUser].accounts[accountType] += transactionAmount;
    }

    DateTime transactionTime;
    if(transactionTimeType == "b")
    {
        Console.Write("Unesi vrijeme transakcije(yyyy-mm-dd): ");
        transactionTime = DateTime.Parse(Console.ReadLine());
    }
    else
    {
        transactionTime = DateTime.Today;
    }

    if (transactionType == "rashod")
    {
        transactionAmount = -transactionAmount;
    }

    int transactionId = transactions.Keys.Count > 0 ? transactions.Keys.Max() + 1 : 1;
    transactions.Add(transactionId, (accountUser, accountType, transactionAmount, transactionDescription, transactionType, transactionCategory, transactionTime));

    PrintTransactions(accountUser, accountType);

    Console.WriteLine($"Stanje racuna {accountType}: {users[accountUser].accounts[accountType]}");

}

void DeleteTransaction(string accountType, int accountUser)
{
    Console.Clear();

    Console.WriteLine("a) po ID-u\nb) ispod unesenog iznosa\nc) iznad unesenog iznosa\nd) svih prihoda\ne) svih rashoda\nf) svih transakcija za odabranu kategoriju");
    var deleteType = Console.ReadLine();

    while (deleteType != "a" && deleteType != "b" && deleteType != "c" && deleteType != "d" && deleteType != "e" && deleteType != "f")
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        deleteType = Console.ReadLine();
    }

    PrintTransactions(accountUser, accountType);

    double transactionAmount;

    switch (deleteType)
    {
        case "a":
            Console.Write("Unesi Id transakcije koju zelis obrisat: ");
            var userInput = Console.ReadLine();

            bool isId = int.TryParse(userInput, out int transactionId);

            if (isId)
            {
                if (!VerifyTransDeletion())
                {
                    Console.WriteLine("Prekinili se brisanje transakcije");
                    break;
                }

                if (transactions.ContainsKey(transactionId) && transactions[transactionId].Item2 == accountType)
                {
                    transactions.Remove(transactionId);
                    Console.WriteLine("Transakcija uspjesno izbrisana!");
                }
                else
                {
                    Console.WriteLine("Ta transakcija ne postoji!");
                }
            }
            break;

        case "b":
            Console.Write("Unesi iznos ispod kojeg zelis izbrisati sve transakcije: ");
            transactionAmount = double.Parse(Console.ReadLine());

            if (!VerifyTransDeletion())
            {
                Console.WriteLine("Prekinili se brisanje transakcije");
                break;
            }
            else
            {
                Console.WriteLine("Transakcija uspjesno izbrisana!");
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item1 == accountUser && transaction.Value.Item3 < transactionAmount) 
                {
                    transactions.Remove(transaction.Key);
                }
            }
            break;

        case "c":
            Console.Write("Unesi iznos iznad kojeg zelis izbrisati sve transakcije: ");
            transactionAmount = double.Parse(Console.ReadLine());

            if (!VerifyTransDeletion())
            {
                Console.WriteLine("Prekinili se brisanje transakcije");
                break;
            }
            else
            {
                Console.WriteLine("Transakcija uspjesno izbrisana!");
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item1 == accountUser && transaction.Value.Item3 > transactionAmount)
                {
                    transactions.Remove(transaction.Key);
                }
            }
            break;

        case "d":
            if (!VerifyTransDeletion())
            {
                Console.WriteLine("Prekinili se brisanje transakcije");
                break;
            }
            else
            {
                Console.WriteLine("Transakcija uspjesno izbrisana!");
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item5 == "prihod")
                {
                    transactions.Remove(transaction.Key);
                }
            }
            break;

        case "e":
            if (!VerifyTransDeletion())
            {
                Console.WriteLine("Prekinili se brisanje transakcije");
                break;
            }
            else
            {
                Console.WriteLine("Transakcija uspjesno izbrisana!");
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item5 == "rashod")
                {
                    transactions.Remove(transaction.Key);
                }
            }
            break;

        case "f":
            Console.Write("Unesi kategoriju za koju zelis izbrisat sve transakcije: ");
            var transactionCategory = Console.ReadLine();

            if (!VerifyTransDeletion())
            {
                Console.WriteLine("Prekinili se brisanje transakcije");
                break;
            }
            else
            {
                Console.WriteLine("Transakcija uspjesno izbrisana!");
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item6 == transactionCategory)
                {
                    transactions.Remove(transaction.Key);
                }
            }
            break;

        default:
            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
            break;
    }

    PrintTransactions(accountUser, accountType);
}

void EditTransaction(string accountType, int accountUser)
{
    Console.Clear();

    PrintTransactions(accountUser, accountType);

    Console.Write("Unesi ID transakcije koju zelis editat: ");
    var userInput = Console.ReadLine();

    bool isId = int.TryParse(userInput, out int transactionId);

    if (isId)
    {
        if (transactions.ContainsKey(transactionId))
        {
            var transaction = transactions[transactionId];

            var transactionTimeType = TransactionTimeInput();

            Console.Write("Iznos transakcije: ");
            transaction.Item3 = int.Parse(Console.ReadLine());

            while (transaction.Item3 <= 0)
            {
                Console.WriteLine("Iznos transakcije ne smije biti negativan!");
                Console.Write("Iznos transakcije: ");
                transaction.Item3 = int.Parse(Console.ReadLine());
            }

            Console.Write("Opis transakcije(po defaultu 'Standarna transakcija'): ");
            transaction.Item4 = Console.ReadLine();

            if (transaction.Item4 == "")
            {
                transaction.Item4 = "Standardna transakcija";
            }

            Console.Write("Tip transakcije(prihod ili rashod): ");
            transaction.Item5 = Console.ReadLine();

            while (transaction.Item5 != "prihod" && transaction.Item5 != "rashod")
            {
                Console.Write("Unesi 'prihod' ili 'rashod': ");
                transaction.Item5 = Console.ReadLine();
            }

            Console.Write(transaction.Item5 == "prihod"
                ? "Kategorija transakcije(placa, honorar, poklon): "
                : "Kategorija transakcije(hrana, prijevoz, sport): ");
            transaction.Item6 = Console.ReadLine();

            if (transactionTimeType == "b")
            {
                Console.Write("Unesi vrijeme transakcije(yyyy-mm-dd): ");
                transaction.Item7 = DateTime.Parse(Console.ReadLine());
            }
            else
            {
                transaction.Item7 = DateTime.Today;
            }

            transactions[transactionId] = transaction;

            Console.WriteLine($"Editana transakcija: {transactions[transactionId]}");
        }
        else
        {
            Console.WriteLine("Ta transakcija ne postoji!");
        }
    }
}

void ReviewTransaction(string accountType, int accountUser)
{
    Console.Clear();

    Console.WriteLine("a) sve transakcije kako su spremljene\nb) sve transakcije sortirane po iznosu uzlazno\nc) sve transakcije sortirane po iznosu silazno\nd) sve transakcije sortirane po opisu abecedno\ne) sve transakcije sortirane po datumu uzlazno\nf) sve transakcije sortirane po datumu silazno\ng) svi prihodi\nh) svi rashodi\ni) sve transakcije za odabranu kategoriju\nj) sve transakcije za odabrani tip i kategoriju");
    var reviewType = Console.ReadLine();

    while (reviewType != "a" && reviewType != "b" && reviewType != "c" && reviewType != "d" && reviewType != "e" && reviewType != "f" && reviewType != "g" && reviewType != "h" && reviewType != "i" && reviewType != "j")
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        reviewType = Console.ReadLine();
    }

    IEnumerable<KeyValuePair<int, (int, string, double, string, string, string, DateTime)>> sortedTransactions;

    Console.Clear();

    var transactionsCount = 0;
    string tranCategory;

    switch (reviewType)
    {
        case "a":
            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                {
                    Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
                }
            }
            break;

        case "b":
            sortedTransactions = transactions
                .Where(transaction => transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                .OrderBy(transaction => transaction.Value.Item3);

            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
            }
            break;

        case "c":
            sortedTransactions = transactions
               .Where(transaction => transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
               .OrderByDescending(transaction => transaction.Value.Item3);

            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
            }
            break;

        case "d":
            sortedTransactions = transactions
                .Where(transaction => transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                .OrderBy(transaction => transaction.Value.Item4);
            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
            }
            break;

        case "e":
            sortedTransactions = transactions
                .Where(transaction => transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                .OrderBy(transaction => transaction.Value.Item7);
            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
            }
            break;

        case "f":
            sortedTransactions = transactions
                .Where(transaction => transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                .OrderByDescending(transaction => transaction.Value.Item7);
            foreach (var transaction in sortedTransactions)
            {
                Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
            }
            break;

        case "g":
            foreach(var transaction in transactions)
            {
                if(transaction.Value.Item5 == "prihod" && transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                {
                    Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
                }
            }
            break;

        case "h":
            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item5 == "rashod" && transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                {
                    Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
                }
            }
            break;

        case "i":
            Console.Write("Unesi kategoriju koju zelis vidit(placa, honorar, poklon, hrana, prijevoz, sport): ");
            tranCategory = Console.ReadLine();

            while(tranCategory != "placa" && tranCategory != "honorar" && tranCategory != "poklon" && tranCategory != "hrana" && tranCategory != "prijevoz" && tranCategory != "sport")
            {
                Console.WriteLine("Ta kategorija ne postoji!");
                Console.Write("Unesi kategoriju koju zelis vidit(placa, honorar, poklon, hrana, prijevoz, sport): ");
                tranCategory = Console.ReadLine();
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item6 == tranCategory && transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                {
                    Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
                }
            }

            break;

        case "j":
            Console.Write("Unesi kategoriju koju zelis vidit(placa, honorar, poklon, hrana, prijevoz, sport): ");
            tranCategory = Console.ReadLine();

            while (tranCategory != "placa" && tranCategory != "honorar" && tranCategory != "poklon" && tranCategory != "hrana" && tranCategory != "prijevoz" && tranCategory != "sport")
            {
                Console.WriteLine("Ta kategorija ne postoji!");
                Console.Write("Unesi kategoriju koju zelis vidit(placa, honorar, poklon, hrana, prijevoz, sport): ");
                tranCategory = Console.ReadLine();
            }

            Console.Write("Unesi tip koji zelis vidit(prihod ili rashod): ");
            var tranType = Console.ReadLine();

            while (tranType != "prihod" && tranType != "rashod" )
            {
                Console.WriteLine("Krivi unos, izaberi 'prihod' ili 'rashod'");
                Console.Write("Unesi tip koji zelis vidit(prihod ili rashod): ");
                tranType = Console.ReadLine();
            }

            foreach (var transaction in transactions)
            {
                if (transaction.Value.Item6 == tranCategory && transaction.Value.Item5 == tranType && transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
                {
                    Console.WriteLine($"{transaction.Value.Item5}-{transaction.Value.Item3}-{transaction.Value.Item4}-{transaction.Value.Item6}-{transaction.Value.Item7}");
                }
            }

            break;

        default:
            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
            break;
    }
}

void FinancialReport(string accountType, int accountUser)
{
    Console.Clear();

    Console.WriteLine("a) trenutno stanje računa\nb) broj ukupnih transakcija\nc) ukupan iznos prihoda i rashoda za odabrani mjesec i godinu\nd) postotak udjela rashoda za odabranu kategoriju\ne) prosječni iznos transakcije za odabrani mjesec i godinu\nf) prosječni iznos transakcije za odabranu kategoriju");
    var financialReportType = Console.ReadLine();

    while (financialReportType != "a" && financialReportType != "b" && financialReportType != "c" && financialReportType != "d" && financialReportType != "e" && financialReportType != "f")
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        financialReportType = Console.ReadLine();
    }

    switch (financialReportType)
    {
        case "a":
            var accountState = AccountState(accountType, accountUser);

            if(accountState < 0)
            {
                Console.WriteLine("Upozorenje! Racun vam je u minusu.");
            }

            Console.WriteLine($"Stanje racuna {accountType}: {accountState}");
            break;

        case "b":
            var amountOfTransactions = AllTransactionsAmount(accountType, accountUser);

            Console.WriteLine($"Broj transakcija ovog racuna: {amountOfTransactions}");
            break;

        case "c":
            AccountIncomeExpense(accountType, accountUser);
            break;

        case "d":
            CategoryExpensePercentage(accountType, accountUser);
            break;

        case "e":
            AverageAmountForDate(accountType, accountUser);
            break;

        case "f":
            AverageAmountForCategory(accountType, accountUser);
            break;

        default:
            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
            break;
    }
}

double AccountState(string accountType, int accountUser)
{
    double amount = 0;

    foreach(var transaction in transactions)
    {
        if(transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
        {
            amount += transaction.Value.Item5 == "prihod" ? transaction.Value.Item3 : -transaction.Value.Item3;
        }
    }

    return amount;
}

int AllTransactionsAmount(string accountType, int accountUser)
{
    int amount = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
        {
            amount++;
        }
    }

    return amount;
}

void AccountIncomeExpense(string accountType, int accountUser)
{
    var year = EnterYear();
    var month = EnterMonth();

    double income = 0;
    double expense = 0;

    foreach(var transaction in transactions)
    {
        if(transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType && year == transaction.Value.Item7.Year && month == transaction.Value.Item7.Month)
        {
            if (transaction.Value.Item5 == "prihod")
            {
                income += transaction.Value.Item3;
            }
            else
            {
                expense += transaction.Value.Item3;
            }
        }
    }

    Console.WriteLine($"Prihodi: {income}");
    Console.WriteLine($"Rashodi: {expense}");
}

void CategoryExpensePercentage(string accountType, int accountUser)
{
    Console.Write("Unesi kategoriju(hrana, prijevoz, sport): ");
    var tranCategory = Console.ReadLine();

    while(tranCategory != "hrana" && tranCategory != "prijevoz" && tranCategory != "sport")
    {
        Console.WriteLine("Unesi postojecu kategoriju!");
        Console.Write("Unesi kategoriju(hrana, prijevoz, sport): ");
        tranCategory = Console.ReadLine();
    }

    double fullExpense = 0;
    double categoryExpense = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType)
        {
            if (transaction.Value.Item5 == "rashod")
            {
                fullExpense += transaction.Value.Item3;
            }

            if(transaction.Value.Item5 == "rashod" && transaction.Value.Item6 == tranCategory)
            {
                categoryExpense += transaction.Value.Item3;
            }
        
        }
    }
    Console.WriteLine($"Postotak rashoda kategorije {tranCategory}: {(categoryExpense / fullExpense)}");
}

void AverageAmountForDate(string accountType, int accountUser)
{
    var year = EnterYear();
    var month = EnterMonth();

    double totalAmount = 0;
    int numberOfTransactions = 0;

    foreach (var transaction in transactions)
    {
        if (transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType && year == transaction.Value.Item7.Year && month == transaction.Value.Item7.Month)
        {
            totalAmount += transaction.Value.Item3;
            numberOfTransactions++;
        }
    }

    Console.WriteLine($"Prosjecni iznos transakcije za to razdoblje: {totalAmount/numberOfTransactions}");
}

void AverageAmountForCategory(string accountType, int accountUser)
{
    Console.Write("Unesi kategoriju(hrana, prijevoz, sport, placa, honorar, poklon): ");
    var tranCategory = Console.ReadLine();

    while (tranCategory != "hrana" && tranCategory != "prijevoz" && tranCategory != "sport" && tranCategory != "placa" && tranCategory != "honorar" && tranCategory != "poklon")
    {
        Console.WriteLine("Unesi postojecu kategoriju!");
        Console.Write("Unesi kategoriju(hrana, prijevoz, sport, placa, honorar, poklon): ");
        tranCategory = Console.ReadLine();
    }

    double totalAmount = 0;
    var numberOfTransactions = 0;
    foreach (var transaction in transactions)
    {
        if (transaction.Value.Item1 == accountUser && transaction.Value.Item2 == accountType && transaction.Value.Item6 == tranCategory)
        {
            totalAmount += transaction.Value.Item3;
            numberOfTransactions++;
        }
    }

    double result = totalAmount / numberOfTransactions;

    if (double.IsNaN(result))
    {
        result = 0;
    }

    Console.WriteLine($"Prosjecni iznos transakcije za tu kategoriju: {result}");
}

string TransactionTimeInput()
{
    Console.WriteLine("a) Trenutno izvršena transakcija\nb) Ranije izvršena transakcija");
    var transactionTimeType = Console.ReadLine();

    while (transactionTimeType != "a" && transactionTimeType != "b")
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        transactionTimeType = Console.ReadLine();
    }

    return transactionTimeType;
}

int EnterYear()
{
    Console.Write("Unesi godinu: ");
    var year = int.Parse(Console.ReadLine());

    if (year > 2024)
    {
        Console.WriteLine("Unili ste ne postojecu godinu! ");
        Console.Write("Unesi godinu: ");
        year = int.Parse(Console.ReadLine());
    }

    return year;
}

int EnterMonth()
{
    Console.Write("Unesi mjesec: ");
    var month = int.Parse(Console.ReadLine());

    if (month < 1 || month > 12)
    {
        Console.WriteLine("Unesi postojeci mjesec(1-12)!");
        Console.Write("Unesi mjesec: ");
        month = int.Parse(Console.ReadLine());
    }

    return month;
}

bool VerifyTransDeletion()
{
    Console.Write("Jeste li sigurni da zelite obrisat transakciju?(da ili ne)");
    var verification = Console.ReadLine();

    while(verification!="da" && verification != "ne")
    {
        Console.Write("Unesi da ili ne: ");
        verification = Console.ReadLine();
    }

    if(verification == "da")
    {
        return true;
    }
    return false; 
}