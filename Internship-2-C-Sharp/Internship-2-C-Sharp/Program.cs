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
                        var user = Console.ReadLine();
                        DeleteUser(user);
                        break;
                    case "3":
                        Console.Write("Unesi ID korisnika: ");
                        var userId = Console.ReadLine();
                        EditUser(userId);
                        break;
                    case "4":
                        Console.WriteLine("a) ispis svih korisnika abecedno po prezimenu\nb) svih onih koji imaju više od 30 godina\nc) svih onih koji imaju barem jedan račun u minusu\n");
                        var reviewSelection = Console.ReadLine();

                        Console.WriteLine(reviewSelection);
                         
                        while(reviewSelection != "a)" && reviewSelection != "b)" && reviewSelection != "c)")
                        {
                            Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                            reviewSelection = Console.ReadLine();
                        }

                        ReviewUser(reviewSelection);
                        return;
                     default:
                        Console.WriteLine("Krivi unos! Unesite jednu od ponuđenih opcija. ");
                        break;
                }
            }
        }

        static void AccountsMenu()
        {
           
        }

        static void NewUserEntry()
        {

        }

        static void DeleteUser(string user)
        {

        }

        static void EditUser(string userId)
        {

        }

        static void ReviewUser(string reviewSelection)       
        {

        }

    }
}
