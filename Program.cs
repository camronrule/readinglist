using System;
using System.Collections.Generic;
using System.Linq;


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
        Console.WriteLine($"\n{book.Title} by {book.Author} has been added to your readinglist.\n");
    }

    public static bool RemoveBook(Book book, List<Book> list){
        bool success = list.Remove(book);
        if (list.Count() < 1){ empty = true;}
        return success;
    }

}

//deals with CLI and displaying options
class Menu{
    static string currentOption = ""; //user selection from menu
    private static bool optionSelected = false; //whether or not the user has selected an option
    
    private static void MenuAddBook(List<Book> list){
        Book BookToAdd = new Book();
        bool successfulParse = false;
        int progress;

        Console.WriteLine("Please enter the title: ");
        BookToAdd.Title = Console.ReadLine();

        Console.WriteLine("Please enter the author's name: ");
        BookToAdd.Author = Console.ReadLine();

        do {
            Console.WriteLine("Please enter your progress in the book: ");
            successfulParse = Int32.TryParse(Console.ReadLine(), out progress);
        } while (!successfulParse);
        BookToAdd.Progress = progress;
        
        //ask user if this info is correct before adding
        ConfirmBookInfo(BookToAdd, list);

    }

    private static void ConfirmBookInfo(Book book, List<Book> list){
        Console.WriteLine("\nIs the following information about this book correct?");
        Console.WriteLine("============================================");
        Console.WriteLine($"{book}");
        Console.WriteLine("============================================\n");
        if (HelperConfirmationPrompt()){ //information is correct
            ReadingList.AddBookToList(book, list);
        }
        else{  //information was not correct - ask the user to input it again
            Console.WriteLine("Sorry about that!\nPlease try to input the information\nabout that book again.");
            MenuAddBook(list); //try again
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
        Console.WriteLine("Your readinglist is empty! \nWould you like to add a book?");
        
        if (HelperConfirmationPrompt()){ //add first book
            MenuAddBook(list);}  

        
        else{ //do not add a book
            //TODO:
            //what to add here?
        }
    }

    private static void MenuRemoveBook(List<Book> list){
        Console.WriteLine("Please enter the number of the book which you would like to remove:");

        //find the book and send to the ReadingList method
        ReadingList.RemoveBook(list[FindTargetBook(list)], list);
    }

    /*
    Returns the index of the book which is being edited/removed
    */
    private static int FindTargetBook(List<Book> list){
        bool successfulParse = false; //whether or not an int input was received
        int targetIndex; //index of book to be removed
        int attempts = 0; //the number of times they have tried to input a number

        
        foreach (var book in list){
            Console.WriteLine($"{list.IndexOf(book)+1} : {book.Title}");
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
        return targetIndex-1;
    }

    public static void ShowBooks(List<Book> list){
        //prompt user to add books in order to show book list
        if (list.Count() == 0) { 
            AddFirstBook(list);
            return;
        }

        foreach(var book in list){
            Console.WriteLine($"{list.IndexOf(book)+1} : {book}"); //number of book : title \n author \n progress
            Console.WriteLine("============================================\n");

        }
    }

    private static void MenuEditBook(List<Book> list){
        int targetIndex;
        bool successfulParse = false;
        int attempts = 0;
        int optionSelected;

        Console.WriteLine("Please enter the number of the book which you would like to edit:");
        targetIndex = FindTargetBook(list);
        Console.WriteLine($"What would you like to edit from {list[targetIndex].Title}?");

        //obtain user option of which to edit
        Console.WriteLine("1 : Title\n2: Author\n3: Progress");
        do{
            //prompt invalid input 
            if (attempts>0){ Console.WriteLine("Invalid input."); }
            successfulParse = Int32.TryParse(Console.ReadLine(), out optionSelected);
            if (targetIndex > list.Count() || targetIndex < 1){ successfulParse = false; } //index out of range
            attempts++;
        } while (!successfulParse);

        
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
                MenuEditBook(list);
                break;

            case "D": //delete a book
                MenuRemoveBook(list);
                break;  

            case "Q": //quit program
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
                break;  
        }

    }

    class Program{
        static void Main(string[] args){
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





