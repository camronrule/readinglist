﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//information for each individual book

class Book{ //book

    public string Title { get; set; }
    public string Author{ get; set; }
    public int Progress{ get; set; }
    
    public Book(){

    }
    public Book (string title, string author, int progress){
        this.Title = title;
        this.Author = author;
        this.Progress = progress;
    }

    public override string ToString()
    {
        return ($"Title: {Title} \nAuthor: {Author} \nProgress: {Progress}");
    }
}


//maintains the list of books, adds and changes info of books
class ReadingList{
    public static bool empty = true;
    public static List<Book> BookList = new List<Book>();

    public static void AddBookToList(Book book, List<Book> list){
        BookList.Add(book);
        empty = false;
    }

    public static void RemoveBook(Book book, List<Book> list){
        bool success = list.Remove(book);
        if (list.Count() < 1){ empty = true;}
        if (success){
            Console.WriteLine($"\n{book.Title} by {book.Author} has been removed from your readinglist.\n");
        }
    }

}

//deals with CLI and displaying options
class Menu{
    static string currentOption = ""; //user selection from menu
    private static bool optionSelected = false; //whether or not the user has selected an option
    //whether or not the user is editing information about an entry
    private static bool CurrentlyEditing = false; 
    
    private static void MenuAddBook(List<Book> list){
        Book BookToAdd = new Book();

        Console.WriteLine("Please enter the title: ");
        BookToAdd.Title = Console.ReadLine();

        Console.WriteLine("Please enter the author's name: ");
        BookToAdd.Author = Console.ReadLine();

        BookToAdd.Progress = ObtainProgress();
        
        //ask user if this info is correct before adding
        ConfirmBookInfo(BookToAdd, list);

    }

    private static int ObtainProgress(){
        int progress;
        bool successfulParse = false;
        do {
            Console.WriteLine("Please enter your progress in the book: ");
            successfulParse = Int32.TryParse(Console.ReadLine(), out progress);
        } while (!successfulParse);
        return progress;
    }

    private static void ConfirmBookInfo(Book book, List<Book> list){
        Console.WriteLine("\nIs the following information about this book correct?");
        Console.WriteLine("............................................");
        Console.WriteLine($"{book}");
        Console.WriteLine("............................................\n");
        if (HelperConfirmationPrompt()){ //information is correct

            //we are confirming that the edited information is correct,
            //therefore we do not want to add another new book - we want
            //to edit a book in place -- do not add a new one
            if(CurrentlyEditing){
                Console.WriteLine($"\nYour reading list has been successfully updated.\n");
                CurrentlyEditing = false; //exiting the editing phase
                return;
            }
            ReadingList.AddBookToList(book, list);
            Console.WriteLine($"\n{book.Title} by {book.Author} has been added to your readinglist.\n");
        }
        else{  //information was not correct - send them to edit function to make changes
            Console.WriteLine("\nSorry about that!\nPlease try to input the information about that book again.");
            MenuEditBook(book, list); //the boolean shows that the user denied the information

        }
    }

    private static bool HelperConfirmationPrompt(){
        string UserOption = "";
        bool optionSelected = false;

        Console.WriteLine("Y   /   N");

        do{
            UserOption = Console.ReadLine().ToUpper();
            optionSelected = (UserOption.Contains('Y') || UserOption.Contains('N'));
        } while (!optionSelected);

        if (UserOption.Contains('Y')){
            return true;;
        }        
        else{
            return false;
        }
    }

    private static void AddFirstBook(List<Book> list){
        Console.WriteLine("Your readinglist is empty! \nPlease add your first book.\n");
        MenuAddBook(list);
    }

    private static void MenuRemoveBook(List<Book> list){
        Console.WriteLine("Please enter the number of the book which you would like to remove:");

        //find the book and send to the ReadingList method
        ReadingList.RemoveBook(FindTargetBook(list), list);
    }

    /*
    Returns the index of the book which is being edited/removed
    */
    private static Book FindTargetBook(List<Book> list){
        bool successfulParse = false; //whether or not an int input was received
        int targetIndex; //index of book to be removed
        int attempts = 0; //the number of times they have tried to input a number

        foreach (var book in list){
            Console.WriteLine($"{list.IndexOf(book)+1} : {book.Title} by {book.Author}");
            //prints: 1 : {title} // 2 : {title} ...
        }

        //take user input on which book to remove by title
        do {
            //prompt invalid input - this is their second attempts
            if (attempts>0){ Console.WriteLine("Invalid input."); }
            successfulParse = Int32.TryParse(Console.ReadLine(), out targetIndex);
            if (targetIndex > list.Count() || targetIndex < 1){ successfulParse = false; } //index out of range
            attempts++;
        } while (!successfulParse);
        return list[targetIndex-1];
    }

    public static void ShowBooks(List<Book> list){
        //prompt user to add books in order to show book list
        if (list.Count() == 0) { 
            AddFirstBook(list);
            return;
        }

        //not the user's first book:
        //some readability and formatting before printing the list:
        Console.WriteLine("Here is your readinglist:\n");
        Console.WriteLine("............................................");

        //printing the list with a dividing line under each entry
        foreach(var book in list){
            Console.WriteLine($"{list.IndexOf(book)+1} : {book}"); //number of book : title \n author \n progress
            Console.WriteLine("............................................");

        }

        //print how many books are in the list
        string titlesPlurality = "titles"; //to adjust for if there is only one book in the list
        if (list.Count() == 1){ titlesPlurality = "title";}
        Console.WriteLine($"\nYour readinglist contains {list.Count()} {titlesPlurality}.\n");
    }

    private static void MenuEditBook(Book book, List<Book> list){
        bool successfulParse = false;
        int attempts = 0;
        int optionSelected;
        CurrentlyEditing = true;

        //check if list is empty
        if (list.Count() == 0){
            AddFirstBook(list);
            return;
        }
        
        Console.WriteLine($"\nWhat would you like to edit from {book.Title} by {book.Author}?");

        //obtain user option of which to edit
        Console.WriteLine("1 : Title\n2 : Author\n3 : Progress\n");
        do{
            //prompt invalid input 
            if (attempts>0){ Console.WriteLine("Invalid input."); }
            successfulParse = Int32.TryParse(Console.ReadLine(), out optionSelected);
            if (optionSelected > 3 || optionSelected < 1){ successfulParse = false; } //index out of range
            attempts++;
        } while (!successfulParse);

        switch (optionSelected){
            case 1: //title
                MenuEditTitle(book, list);
                break;
            case 2: //author
                MenuEditAuthor(book, list);
                break;
            case 3: //progress
                MenuEditProgress(book, list);
                break;
        }

        ConfirmBookInfo(book, list);
    }

    private static void MenuEditTitle(Book book, List<Book> list){
        Console.WriteLine("\nWhat would you like to change the title of this entry to?");
        book.Title = Console.ReadLine();
    }

    private static void MenuEditAuthor(Book book, List<Book> list){
        Console.WriteLine("\nWhat would you like to change the author of this entry to?");
        book.Author = Console.ReadLine();
    }

    private static void MenuEditProgress(Book book, List<Book> list){
        Console.WriteLine("\n");
        book.Progress = ObtainProgress();
    }


    private static void DisplayMenu(List<Book> list){

        do{
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
            
            if (currentOption.Contains('S') || currentOption.Contains('A') || currentOption.Contains('E') || currentOption.Contains('D') || currentOption.Contains('Q')){
                optionSelected = true;
            }


        } while (!optionSelected);

        switch (currentOption){
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
                Environment.Exit(0);
                break;  
        }

        static bool TryReadLine(out string result){
            var buf = new StringBuilder();
            for (; ; ){
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape){
                    result = null;
                    return false;
                }
                else if (key.Key == ConsoleKey.Enter){
                    result = buf.ToString();
                    return true;
                }
                else if (key.Key == ConsoleKey.Backspace && buf.Length > 0){
                    buf.Remove(buf.Length -1, 1);
                    Console.Write("\b \b");
                }
                else if (key.KeyChar != 0){
                    buf.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

    }

    class Program{
        private static bool run = true;
        static void Main(string[] args){
            while (run){
                while (ReadingList.empty){
                    Menu.DisplayMenu(ReadingList.BookList);
                }
                while (!ReadingList.empty){
                    //Menu.ShowBooks(ReadingList.BookList);
                    Menu.DisplayMenu(ReadingList.BookList);
                }
            }
            
        }
    }
    
}





