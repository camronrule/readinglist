namespace VSreadinglist
{

    class Program
    {
        public static bool run = true;

        static void Main()
        {
            _ = new ReadingList(); //create ReadingList obj to give to UserData
            UserData user = new UserData(ReadingList.BookList); //give reference to ReadingList to UserData
            ReadingList.BookList = user.LoadUserData(); //obtain loaded books and add to user's ReadingList


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
            //exiting program
            UserData.SaveUserData(ReadingList.BookList); //save books to storage
        }
    }

}





