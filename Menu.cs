﻿namespace VSreadinglist
{
    //deals with CLI and displaying options
    public class Menu
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
                Console.WriteLine("Please enter your progress in the book (as a percentage, from 0-100): ");
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


        public static void DisplayMenu(List<Book> list)
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
    }
}