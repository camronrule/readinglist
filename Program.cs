using System.Configuration;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

namespace VSreadinglist
{
    //information for each individual book


    public class Book
    { //book

        public string Title { get; set; }
        public string Author { get; set; }
        public int Progress { get; set; }

        public Book()
        {

        }


        public override string ToString()
        {
            return ($"Title: {Title} \nAuthor: {Author} \nProgress: {Progress}");
        }
    }


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

    //deals with CLI and displaying options
    class Menu
    {
        static string currentOption = ""; //user selection from menu
        private static bool optionSelected = false; //whether or not the user has selected an option
                                                    //whether or not the user is editing information about an entry
        private static bool CurrentlyEditing = false;

        private static void MenuAddBook(List<Book> list)
        {
            Book BookToAdd = new();

            Console.WriteLine("Please enter the title: ");
            BookToAdd.Title = Console.ReadLine();

            Console.WriteLine("Please enter the author's name: ");
            BookToAdd.Author = Console.ReadLine();

            BookToAdd.Progress = ObtainProgress();

            //ask user if this info is correct before adding
            ConfirmBookInfo(BookToAdd, list);

        }

        private static int ObtainProgress()
        {
            int progress;
            bool successfulParse;
            do
            {
                Console.WriteLine("Please enter your progress in the book: ");
                successfulParse = Int32.TryParse(Console.ReadLine(), out progress);
            } while (!successfulParse);
            return progress;
        }

        private static void ConfirmBookInfo(Book book, List<Book> list)
        {
            Console.WriteLine("\nIs the following information about this book correct?");
            Console.WriteLine("............................................");
            Console.WriteLine($"{book}");
            Console.WriteLine("............................................\n");
            if (HelperConfirmationPrompt())
            { //information is correct

                //we are confirming that the edited information is correct,
                //therefore we do not want to add another new book - we want
                //to edit a book in place -- do not add a new one
                if (CurrentlyEditing)
                {
                    Console.WriteLine($"\nYour reading list has been successfully updated.\n");
                    CurrentlyEditing = false; //exiting the editing phase
                    return;
                }
                ReadingList.AddBookToList(book);
                Console.WriteLine($"\n{book.Title} by {book.Author} has been added to your readinglist.\n");
            }
            else
            {  //information was not correct - send them to edit function to make changes
                Console.WriteLine("\nSorry about that!\nPlease try to input the information about that book again.");
                MenuEditBook(book, list); //the boolean shows that the user denied the information

            }
        }

        private static bool HelperConfirmationPrompt()
        {
            string UserOption;
            Console.WriteLine("Y   /   N");

            bool optionSelected;
            do
            {
                UserOption = Console.ReadLine().ToUpper();
                optionSelected = (UserOption.Contains('Y') || UserOption.Contains('N'));
            } while (!optionSelected);

            if (UserOption.Contains('Y'))
            {
                return true; ;
            }
            else
            {
                return false;
            }
        }

        private static void AddFirstBook(List<Book> list)
        {
            Console.WriteLine("Your readinglist is empty! \nPlease add your first book.\n");
            MenuAddBook(list);
        }

        private static void MenuRemoveBook(List<Book> list)
        {
            Console.WriteLine("Please enter the number of the book which you would like to remove:");

            //find the book and send to the ReadingList method
            ReadingList.RemoveBook(FindTargetBook(list));
        }

        /*
        Returns the index of the book which is being edited/removed
        */
        private static Book FindTargetBook(List<Book> list)
        {
            bool successfulParse; //whether or not an int input was received
            int targetIndex; //index of book to be removed
            int attempts = 0; //the number of times they have tried to input a number

            foreach (var book in list)
            {
                Console.WriteLine($"{list.IndexOf(book) + 1} : {book.Title} by {book.Author}");
                //prints: 1 : {title} // 2 : {title} ...
            }

            //take user input on which book to remove by title
            do
            {
                //prompt invalid input - this is their second attempts
                if (attempts > 0) { Console.WriteLine("Invalid input."); }
                successfulParse = Int32.TryParse(Console.ReadLine(), out targetIndex);
                if (targetIndex > list.Count || targetIndex < 1) { successfulParse = false; } //index out of range
                attempts++;
            } while (!successfulParse);
            return list[targetIndex - 1];
        }

        public static void ShowBooks(List<Book> list)
        {
            //prompt user to add books in order to show book list
            if (list.Count == 0)
            {
                AddFirstBook(list);
                return;
            }

            //not the user's first book:
            //some readability and formatting before printing the list:
            Console.WriteLine("Here is your readinglist:\n");
            Console.WriteLine("............................................");

            //printing the list with a dividing line under each entry
            foreach (var book in list)
            {
                Console.WriteLine($"{list.IndexOf(book) + 1} : {book}"); //number of book : title \n author \n progress
                Console.WriteLine("............................................");

            }

            //print how many books are in the list
            string titlesPlurality = "titles"; //to adjust for if there is only one book in the list
            if (list.Count == 1) { titlesPlurality = "title"; }
            Console.WriteLine($"\nYour readinglist contains {list.Count} {titlesPlurality}.\n");
        }

        private static void MenuEditBook(Book book, List<Book> list)
        {
            bool successfulParse;
            int attempts = 0;
            int optionSelected;
            CurrentlyEditing = true;

            //check if list is empty
            if (list.Count == 0 && !CurrentlyEditing)
            {
                AddFirstBook(list);
                return;
            }

            Console.WriteLine($"\nWhat would you like to edit from {book.Title} by {book.Author}?");

            //obtain user option of which to edit
            Console.WriteLine("1 : Title\n2 : Author\n3 : Progress\n");
            do
            {
                //prompt invalid input 
                if (attempts > 0) { Console.WriteLine("Invalid input."); }
                successfulParse = Int32.TryParse(Console.ReadLine(), out optionSelected);
                if (optionSelected > 3 || optionSelected < 1) { successfulParse = false; } //index out of range
                attempts++;
            } while (!successfulParse);

            switch (optionSelected)
            {
                case 1: //title
                    MenuEditTitle(book);
                    break;
                case 2: //author
                    MenuEditAuthor(book);
                    break;
                case 3: //progress
                    MenuEditProgress(book);
                    break;
            }

            ConfirmBookInfo(book, list);
        }

        private static void MenuEditTitle(Book book)
        {
            Console.WriteLine("\nWhat would you like to change the title of this entry to?");
            book.Title = Console.ReadLine();
        }

        private static void MenuEditAuthor(Book book)
        {
            Console.WriteLine("\nWhat would you like to change the author of this entry to?");
            book.Author = Console.ReadLine();
        }

        private static void MenuEditProgress(Book book)
        {
            Console.WriteLine("\n");
            book.Progress = ObtainProgress();
        }


        private static void DisplayMenu(List<Book> list)
        {

            do
            {
                //show options
                Console.WriteLine("============================================");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("S      Show your book list");
                Console.WriteLine("A      Add a book to your list");
                Console.WriteLine("E      Edit a book in your list");
                Console.WriteLine("D      Delete a book from your list");
                Console.WriteLine("Q      Quit this program");
                Console.WriteLine("============================================\n");

                //take in user option
                currentOption = Console.ReadLine().ToUpper();

                if (currentOption.Contains('S') || currentOption.Contains('A') || currentOption.Contains('E') || currentOption.Contains('D') || currentOption.Contains('Q'))
                {
                    optionSelected = true;
                }


            } while (!optionSelected);

            switch (currentOption)
            {
                case "S": //show list
                    ShowBooks(list);
                    break;

                case "A": //add book
                    MenuAddBook(list);
                    break;

                case "E": //edit a book
                    Console.WriteLine("Please enter the number of the book which you would like to edit:");
                    MenuEditBook(FindTargetBook(list), list);
                    break;

                case "D": //delete a book
                    MenuRemoveBook(list);
                    break;

                case "Q": //quit program
                    Console.WriteLine("Exiting...");
                    Program.run = false;
                    break;
            }

        }

        /*
        class UserData
        {
            public UserData data;
           
            public void ClosingHandler(List<Book> list)
            {
                Properties.Settings1.Default.DefaultBooks = list;
                Properties.Settings1.Default.DefaultBooks.Save();
            }


  
            public static List<Book> OpeningHandler() { 
                if (Properties.Settings1.Default.DefaultBooks == null)
                {
                    return new List<Book>();
                }
                return Properties.Settings1.Default.DefaultBooks;
            }
        }

        */
        
        class Program
        {
            public static bool run = true;

            static void Main()
            {
                _ = new ReadingList();

                while (run)
                {
                    while (ReadingList.empty && run)
                    {
                        Menu.DisplayMenu(ReadingList.BookList);
                    }
                    while (!ReadingList.empty && run)
                    {
                        //Menu.ShowBooks(ReadingList.BookList);
                        Menu.DisplayMenu(ReadingList.BookList);
                    }
                }
            }
        }

    }
}





