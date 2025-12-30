
    namespace LibraryManagement;
    // LibraryManager Singleton - encapsulates all library logic
    class LibraryManager
    {
        private const int LIBRARY_CAPACITY = 5;
        private const int BORROW_LIMIT = 3;
        
        private List<Book> library = new List<Book>();
        private string? borrow1, borrow2, borrow3;
        
        private static LibraryManager? _instance;
        
        private LibraryManager() { }
        
        public static LibraryManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LibraryManager();
                return _instance;
            }
        }
        
        public void AddBook()
        {
            Console.Write("Enter book title: ");
            var title = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Title cannot be empty.");
                return;
            }

            if (library.Any(b => string.Equals(b.Title, title, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Book already exists in library.");
                return;
            }

            if (library.Count >= LIBRARY_CAPACITY)
            {
                Console.WriteLine($"Library is full ({LIBRARY_CAPACITY} books). Remove a book first.");
                return;
            }

            library.Add(new Book(title));
            Console.WriteLine("Book added.");
        }

        public void RemoveBook()
        {
            if (library.Count == 0)
            {
                Console.WriteLine("Library is empty. Nothing to remove.");
                return;
            }

            Console.WriteLine("Current books:");
            for (int i = 0; i < library.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {library[i].Title}{(library[i].IsCheckedOut ? " (checked out)" : "")}");
            }

            Console.Write("Enter index (1-{0}) or exact title to remove: ", library.Count);
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Aborted.");
                return;
            }

            // Try parsing as index
            if (int.TryParse(input, out var idx))
            {
                if (idx < 1 || idx > library.Count)
                {
                    Console.WriteLine("Index out of range.");
                    return;
                }
                library.RemoveAt(idx - 1);
                Console.WriteLine("Book removed.");
                return;
            }

            // Remove by title (case-insensitive)
            var book = library.FirstOrDefault(b => string.Equals(b.Title, input, StringComparison.OrdinalIgnoreCase));
            if (book != null)
            {
                library.Remove(book);
                Console.WriteLine("Book removed.");
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public void DisplayBooks()
        {
            Console.WriteLine("\nAvailable books in library:");
            if (library.Count == 0)
            {
                Console.WriteLine("(none)");
            }
            else
            {
                for (int i = 0; i < library.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {library[i].Title}{(library[i].IsCheckedOut ? " (checked out)" : "")}");
                }
            }

            // Show borrowed books
            Console.WriteLine("\nBorrowed books:");
            bool anyBorrowed = false;
            if (!string.IsNullOrWhiteSpace(borrow1)) { Console.WriteLine($"- {borrow1}"); anyBorrowed = true; }
            if (!string.IsNullOrWhiteSpace(borrow2)) { Console.WriteLine($"- {borrow2}"); anyBorrowed = true; }
            if (!string.IsNullOrWhiteSpace(borrow3)) { Console.WriteLine($"- {borrow3}"); anyBorrowed = true; }
            if (!anyBorrowed) Console.WriteLine("(none)");
        }

        public void SearchBook()
        {
            Console.Write("Enter title or part of title to search: ");
            var q = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(q))
            {
                Console.WriteLine("Empty query. Aborted.");
                return;
            }

            var found = false;
            Console.WriteLine("Matches:");
            for (int i = 0; i < library.Count; i++)
            {
                if (library[i].Title.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine($"{i + 1}: {library[i].Title}{(library[i].IsCheckedOut ? " (checked out)" : "")}");
                    found = true;
                }
            }

            if (!found) Console.WriteLine("(no matches)");
        }

        // Private helper methods for borrowing
        private int BorrowCount()
        {
            var c = 0;
            if (!string.IsNullOrWhiteSpace(borrow1)) c++;
            if (!string.IsNullOrWhiteSpace(borrow2)) c++;
            if (!string.IsNullOrWhiteSpace(borrow3)) c++;
            return c;
        }

        private bool HasBorrowedTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            if (string.Equals(borrow1, title, StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(borrow2, title, StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(borrow3, title, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private bool AddBorrowed(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            if (string.IsNullOrWhiteSpace(borrow1)) { borrow1 = title; return true; }
            if (string.IsNullOrWhiteSpace(borrow2)) { borrow2 = title; return true; }
            if (string.IsNullOrWhiteSpace(borrow3)) { borrow3 = title; return true; }
            return false;
        }

        private bool RemoveBorrowedByTitle(string title)
        {
            if (string.Equals(borrow1, title, StringComparison.OrdinalIgnoreCase)) { borrow1 = null; return true; }
            if (string.Equals(borrow2, title, StringComparison.OrdinalIgnoreCase)) { borrow2 = null; return true; }
            if (string.Equals(borrow3, title, StringComparison.OrdinalIgnoreCase)) { borrow3 = null; return true; }
            return false;
        }

        public void BorrowBook()
        {
            if (BorrowCount() >= BORROW_LIMIT)
            {
                Console.WriteLine($"You've reached the borrow limit ({BORROW_LIMIT}). Return a book first.");
                return;
            }

            if (library.Count == 0)
            {
                Console.WriteLine("No books available to borrow.");
                return;
            }

            Console.WriteLine("Available books:");
            for (int i = 0; i < library.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {library[i].Title}{(library[i].IsCheckedOut ? " (checked out)" : "")}");
            }

            Console.Write("Enter index (1-{0}) or exact title to borrow: ", library.Count);
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Aborted.");
                return;
            }

            Book? bookToBorrow = null;

            // Try parsing as index
            if (int.TryParse(input, out var idx))
            {
                if (idx < 1 || idx > library.Count)
                {
                    Console.WriteLine("Index out of range.");
                    return;
                }
                bookToBorrow = library[idx - 1];
            }
            else
            {
                // Find by title (case-insensitive)
                bookToBorrow = library.FirstOrDefault(b => string.Equals(b.Title, input, StringComparison.OrdinalIgnoreCase));
            }

            if (bookToBorrow == null)
            {
                Console.WriteLine("Book not found in library.");
                return;
            }

            if (HasBorrowedTitle(bookToBorrow.Title))
            {
                Console.WriteLine("You already borrowed this book.");
                return;
            }

            if (!AddBorrowed(bookToBorrow.Title))
            {
                Console.WriteLine("Could not borrow (limit reached).");
                return;
            }

            library.Remove(bookToBorrow);
            Console.WriteLine("Book borrowed.");
        }

        public void ReturnBook()
        {
            if (BorrowCount() == 0)
            {
                Console.WriteLine("You have no borrowed books.");
                return;
            }

            Console.WriteLine("Borrowed books:");
            Console.WriteLine($"1: {(string.IsNullOrWhiteSpace(borrow1) ? "(empty)" : borrow1)}");
            Console.WriteLine($"2: {(string.IsNullOrWhiteSpace(borrow2) ? "(empty)" : borrow2)}");
            Console.WriteLine($"3: {(string.IsNullOrWhiteSpace(borrow3) ? "(empty)" : borrow3)}");
            Console.Write($"Enter index (1-{BORROW_LIMIT}) or exact title to return: ");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Aborted.");
                return;
            }

            string? titleToReturn = null;
            if (int.TryParse(input, out var idx))
            {
                if (idx < 1 || idx > BORROW_LIMIT)
                {
                    Console.WriteLine("Index out of range.");
                    return;
                }
                titleToReturn = idx switch { 1 => borrow1, 2 => borrow2, 3 => borrow3, _ => null };
                if (string.IsNullOrWhiteSpace(titleToReturn))
                {
                    Console.WriteLine("Slot is empty.");
                    return;
                }
            }
            else
            {
                // By title
                if (!HasBorrowedTitle(input))
                {
                    Console.WriteLine("You haven't borrowed that book.");
                    return;
                }
                titleToReturn = input;
            }

            // Try to place back into library
            if (library.Count >= LIBRARY_CAPACITY)
            {
                Console.WriteLine($"Library is full. Cannot check-in the book. Remove a library book first.");
                return;
            }

            library.Add(new Book(titleToReturn));
            RemoveBorrowedByTitle(titleToReturn);
            Console.WriteLine("Book returned (checked in).");
        }

        public void ToggleCheckedFlag()
        {
            if (library.Count == 0)
            {
                Console.WriteLine("Library is empty.");
                return;
            }

            Console.WriteLine("Current books:");
            for (int i = 0; i < library.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {library[i].Title}{(library[i].IsCheckedOut ? " (checked out)" : "")}");
            }

            Console.Write("Enter index (1-{0}) or exact title to toggle checked-out flag: ", library.Count);
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Aborted.");
                return;
            }

            Book? book = null;

            // Try parsing as index
            if (int.TryParse(input, out var idx))
            {
                if (idx < 1 || idx > library.Count)
                {
                    Console.WriteLine("Index out of range.");
                    return;
                }
                book = library[idx - 1];
            }
            else
            {
                // Find by title (case-insensitive)
                book = library.FirstOrDefault(b => string.Equals(b.Title, input, StringComparison.OrdinalIgnoreCase));
            }

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            book.IsCheckedOut = !book.IsCheckedOut;
            Console.WriteLine(book.IsCheckedOut ? "Book flagged as checked out." : "Checked-in (flag removed).");
        }
    }