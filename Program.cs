using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement;

class Program
{
    static void Main()
    {
        var manager = LibraryManager.Instance;

        while (true)
        {
            Console.WriteLine("\nLibrary Management System (max 5 books)");
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Add book");
            Console.WriteLine("2. Remove book");
            Console.WriteLine("3. Display books");
            Console.WriteLine("4. Search book by title");
            Console.WriteLine("5. Borrow book");
            Console.WriteLine("6. Return (check-in) book");
            Console.WriteLine("7. Toggle checked-out flag (check out / check in)");
            Console.WriteLine("8. Exit");
            Console.Write("Choose option (1-8): ");

            var choice = Console.ReadLine()?.Trim();
            switch (choice)
            {
                case "1":
                    manager.AddBook();
                    break;
                case "2":
                    manager.RemoveBook();
                    break;
                case "3":
                    manager.DisplayBooks();
                    break;
                case "4":
                    manager.SearchBook();
                    break;
                case "5":
                    manager.BorrowBook();
                    break;
                case "6":
                    manager.ReturnBook();
                    break;
                case "7":
                    manager.ToggleCheckedFlag();
                    break;
                case "8":
                    Console.WriteLine("Exiting. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option — please choose 1-8.");
                    break;
            }
        }
    }
}
