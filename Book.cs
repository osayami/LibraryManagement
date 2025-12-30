namespace       LibraryManagement;
 // Book class to represent a library book
    class Book
    {
        public string Title { get; set; }
        public bool IsCheckedOut { get; set; }

        public Book(string title)
        {
            Title = title;
            IsCheckedOut = false;
        }
    }