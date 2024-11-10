using System;
using System.Net.NetworkInformation;
using System.Net.WebSockets;

namespace InternshipCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            
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
                        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                        break;
                }
            }
        }

        static void UsersMenu()
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
                        Console.Write("Unesi ID ili ime i prezime korisnika: ");
                        var userBadge = Console.ReadLine();

                        DeleteUser(userBadge);
                        break;
                    case "3":
                        Console.Write("Unesi ID korisnika: ");
                        var userId = Console.ReadLine();

                        EditUser(userId);
                        break;
                    case "4":
                        Console.WriteLine("a) ispis svih korisnika abecedno po prezimenu\nb) svih onih koji imaju više od 30 godina\nc) svih onih koji imaju barem jedan račun u minusu\n");
                        var reviewType = Console.ReadLine();
                         
                        while(reviewType != "a)" && reviewType != "b)" && reviewType != "c)")
                        {
                            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                            Console.WriteLine("a) ispis svih korisnika abecedno po prezimenu\nb) svih onih koji imaju više od 30 godina\nc) svih onih koji imaju barem jedan račun u minusu\n");
                            reviewType = Console.ReadLine();
                        }

                        ReviewUser(reviewType);
                        return;
                     default:
                        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                        break;
                }
            }
        }

        static void AccountsMenu()
        {

            Console.Write("Unesi ime i prezime: ");
            var user = Console.ReadLine();

            Console.Write("1 - žiro\n2 - tekući\n3 - prepaid");
            var accountType = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("1 - Unos nove transakcije\n2 - Brisanje transakcije\n3 - Uređivanje transakcije\n4 - Pregled transakcija\n5 - Financijsko izvješće");
                var usersMenuSelection = Console.ReadLine();

                switch (usersMenuSelection)
                {
                    case "1":
                        Console.WriteLine("a) trenutno izvršena transakcija\nb) - ranije izvršena transakcija");
                        var transactionTime = Console.ReadLine();
                        
                        while(transactionTime != "a)" && transactionTime != "b)")
                        {
                            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                            Console.WriteLine("a) trenutno izvršena transakcija\nb) ranije izvršena transakcija(potrebno je upisati datum i vrijeme");
                            transactionTime = Console.ReadLine();
                        }

                        NewTransaction(transactionTime);
                        break;
                    case "2":
                        Console.WriteLine("a) po ID-u\nb) ispod unesenog iznosa\nc) iznad unesenog iznosa\nd) svih prihoda\ne) svih rashoda\nf) svih transakcija za odabranu kategoriju");
                        var deleteType = Console.ReadLine();

                        while (deleteType != "a)" && deleteType != "b)" && deleteType != "c)" && deleteType != "d)" && deleteType != "e)" && deleteType != "f)")
                        {
                            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                            Console.WriteLine("a) po ID-u\nb) ispod unesenog iznosa\nc) iznad unesenog iznosa\nd) svih prihoda\ne) svih rashoda\nf) svih transakcija za odabranu kategoriju");
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

                        while (reviewType != "a)" && reviewType != "b)" && reviewType != "c)" && reviewType != "d)" && reviewType != "e)" && reviewType != "f)" && reviewType != "g)" && reviewType != "h)" && reviewType != "i)" && reviewType != "j)")
                        {
                            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                            Console.WriteLine("a) sve transakcije kako su spremljene\nb) sve transakcije sortirane po iznosu uzlazno\nc) sve transakcije sortirane po iznosu silazno\nd) sve transakcije sortirane po opisu abecedno\ne) sve transakcije sortirane po datumu uzlazno\nf) sve transakcije sortirane po datumu silazno\ng) svi prihodi\nh) svi rashodi\ni) sve transakcije za odabranu kategoriju\nj) sve transakcije za odabrani tip i kategoriju");
                            reviewType = Console.ReadLine();
                        }

                        ReviewTransaction(reviewType);

                        break;
                    case "5":
                        Console.WriteLine("a) trenutno stanje računa\nb) broj ukupnih transakcija\nc) ukupan iznos prihoda i rashoda za odabrani mjesec i godinu\nd) postotak udjela rashoda za odabranu kategoriju\ne) prosječni iznos transakcije za odabrani mjesec i godinu\nf) prosječni iznos transakcije za odabranu kategoriju");
                        var financialReportType = Console.ReadLine();

                        while (financialReportType != "a)" && financialReportType != "b)" && financialReportType != "c)" && financialReportType != "d)" && financialReportType != "e)" && financialReportType != "f)")
                        {
                            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                            Console.WriteLine("a) trenutno stanje računa\nb) broj ukupnih transakcija\nc) ukupan iznos prihoda i rashoda za odabrani mjesec i godinu\nd) postotak udjela rashoda za odabranu kategoriju\ne) prosječni iznos transakcije za odabrani mjesec i godinu\nf) prosječni iznos transakcije za odabranu kategoriju");
                            financialReportType = Console.ReadLine();
                        }

                        FinancialReport(financialReportType);

                        break;
                    default:
                        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                        break;
                }
            }


        }

        static void NewUserEntry()
        {

        }

        static void DeleteUser(string userBadge)
        {

        }

        static void EditUser(string userId)
        {

        }

        static void ReviewUser(string reviewType)       
        {

        }
        static void NewTransaction(string transactionTime)
        {

        }

        static void DeleteTransaction(string deleteType)
        {

        }
        static void EditTransaction(string transactionId)
        {

        }
        static void ReviewTransaction(string reviewType)
        {

        }
        static void FinancialReport(string financialReport)
        {

        }
    }
}
