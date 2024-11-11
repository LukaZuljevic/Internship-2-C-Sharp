using System;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Collections.Generic;

Dictionary<int, (Tuple<string, string, DateTime> userInfo, Dictionary<string, double> accounts)> users = new Dictionary<int, (Tuple<string, string, DateTime>, Dictionary<string, double>)>();

users.Add(1, (new Tuple<string, string, DateTime>("Marko", "Livaja", new DateTime(1995, 10, 2)),
    new Dictionary<string, double>
    {
        { "Tekuci", 100.00 },
        { "Ziro", 0.00 },
        { "Prepaid", 0.00 }
    }
));

users.Add(2, (new Tuple<string, string, DateTime>("Ante", "Antic", new DateTime(2003, 10, 6)),
    new Dictionary<string, double>
    {
        { "Tekuci", 100.00 },
        { "Ziro", 0.00 },
        { "Prepaid", 0.00 }
    }
));

Dictionary<int, (int, double, string, string, string, DateTime)> transactions = new Dictionary<int, (int, double, string, string, string, DateTime)>();

transactions.Add(1, (1, 122.22, "Standarna transakcija", "prihod", "honorar", new DateTime(2022, 1, 20)));
transactions.Add(2, (1, 10.1, "Standarna transakcija", "prihod", "placa", new DateTime(2023, 5, 1)));
transactions.Add(3, (2, 200, "Standarna transakcija", "prihod", "honorar", new DateTime(2024, 4, 22)));
transactions.Add(4, (2, 12, "Standarna transakcija", "rashod", "poklon", new DateTime(2020, 3, 25)));

while (true)
{
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
    while (true)
    {
        Console.WriteLine("1 - Unos novog korisnika\n2 - Brisanje korisnika\n3 - Uređivanje korisnika\n4 - Pregled korisnika");
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

    Console.WriteLine("1 - Tekuci\n2 - Ziro\n3 - Prepaid");
    var accountType = Console.ReadLine();

    while (true)
    {
        Console.WriteLine("1 - Unos nove transakcije\n2 - Brisanje transakcije\n3 - Uređivanje transakcije\n4 - Pregled transakcija\n5 - Financijsko izvješće");
        var usersMenuSelection = Console.ReadLine();

        switch (usersMenuSelection)
        {
            case "1":
                NewTransaction(accountType, accountUser.Key);
                break;
            case "2":
                Console.WriteLine("a) po ID-u\nb) ispod unesenog iznosa\nc) iznad unesenog iznosa\nd) svih prihoda\ne) svih rashoda\nf) svih transakcija za odabranu kategoriju");
                var deleteType = Console.ReadLine();

                while (deleteType != "a" && deleteType != "b" && deleteType != "c" && deleteType != "d" && deleteType != "e" && deleteType != "f")
                {
                    Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
                    deleteType = Console.ReadLine();
                }

                DeleteTransaction(deleteType);
                break;
            case "3":
                Console.Write("Unesi ID transakcije");
                var transactionId = Console.ReadLine();

                EditTransaction(transactionId);
                break;
            case "4":
                Console.WriteLine("a) sve transakcije kako su spremljene\nb) sve transakcije sortirane po iznosu uzlazno\nc) sve transakcije sortirane po iznosu silazno\nd) sve transakcije sortirane po opisu abecedno\ne) sve transakcije sortirane po datumu uzlazno\nf) sve transakcije sortirane po datumu silazno\ng) svi prihodi\nh) svi rashodi\ni) sve transakcije za odabranu kategoriju\nj) sve transakcije za odabrani tip i kategoriju");
                var reviewType = Console.ReadLine();

                while (reviewType != "a" && reviewType != "b" && reviewType != "c" && reviewType != "d" && reviewType != "e" && reviewType != "f" && reviewType != "g" && reviewType != "h" && reviewType != "i" && reviewType != "j")
                {
                    Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
                    reviewType = Console.ReadLine();
                }

                ReviewTransaction(accountUser.Key);
                break;
            case "5":
                Console.WriteLine("a) trenutno stanje računa\nb) broj ukupnih transakcija\nc) ukupan iznos prihoda i rashoda za odabrani mjesec i godinu\nd) postotak udjela rashoda za odabranu kategoriju\ne) prosječni iznos transakcije za odabrani mjesec i godinu\nf) prosječni iznos transakcije za odabranu kategoriju");
                var financialReportType = Console.ReadLine();

                while (financialReportType != "a" && financialReportType != "b" && financialReportType != "c" && financialReportType != "d" && financialReportType != "e" && financialReportType != "f")
                {
                    Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
                    financialReportType = Console.ReadLine();
                }
                
                FinancialReport(financialReportType);
                break;
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
    var userId = int.Parse(Console.ReadLine());

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
    Console.WriteLine("a) po ID-u\nb) po imenu i prezimenu");
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

void NewTransaction(string accountType, int accountUser)
{
    Console.WriteLine("a) Trenutno izvršena transakcija\nb) Ranije izvršena transakcija");
    var transactionTimeType = Console.ReadLine();

    while (transactionTimeType != "a" && transactionTimeType != "b")
    {
        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija.");
        transactionTimeType = Console.ReadLine();
    }

    Console.Write("Iznos transakcije: ");
    var transactionAmount = int.Parse(Console.ReadLine());

    while(transactionAmount <= 0)
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

    Console.Write(transactionType == "prihod"
        ? "Kategorija transakcije(placa, honorar, poklon itd.): "
        : "Kategorija transakcije(hrana, prijevoz, sport itd.): ");
    var transactionCategory = Console.ReadLine();

    if(transactionType == "rashod")
    {
        transactionAmount = -transactionAmount;
    }

    if (accountType == "1")
    {
        users[accountUser].accounts["Tekuci"] += transactionAmount;
    }
    else if (accountType == "2")
    {
        users[accountUser].accounts["Ziro"] += transactionAmount;
    }
    else
    {
        users[accountUser].accounts["Prepaid"] += transactionAmount;
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

    int transactionId = transactions.Count + 1; 
    transactions.Add(transactionId, (accountUser, transactionAmount, transactionDescription, transactionType, transactionCategory, transactionTime));

    ReviewTransaction(accountUser);

    Console.WriteLine($"Stanje racuna\nTekuci: {users[accountUser].accounts["Tekuci"]}\nZiro: {users[accountUser].accounts["Ziro"]}\nPrepaid: {users[accountUser].accounts["Prepaid"]}");

}

void DeleteTransaction(string deleteType)
{
}

void EditTransaction(string transactionId)
{
}

void ReviewTransaction(int accountUser)
{
    foreach(var transaction in transactions)
    {
        if (transaction.Value.Item1 == accountUser)
        {
            Console.WriteLine(transaction);
        }
    }
}

void FinancialReport(string financialReport)
{
}
