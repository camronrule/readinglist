namespace VSreadinglist
{
    //maintains the list of books, adds and changes info of books
    class ReadingList
    {
        public static bool empty;
        public static List<Book> BookList;


        public ReadingList()
        {
            empty = true;
            BookList = new();
        }

        public static void AddBookToList(Book book)
        {
            BookList.Add(book);
            empty = false;
        }

        public static void RemoveBook(Book book)
        {
            bool success = BookList.Remove(book);
            if (BookList.Count < 1) { empty = true; }
            if (success)
            {
                Console.WriteLine($"\n{book.Title} by {book.Author} has been removed from your readinglist.\n");
            }
        }

    }
}





