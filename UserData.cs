namespace VSreadinglist
{
    /*
     * UserData handles the persistent storage of the user's
     * reading list. 
     */
    class UserData
    {
        public UserData user;
        private readonly List<Book> new_list; //temporary list to hold loaded books and be handed to ReadingList

        public UserData(List<Book> books) //the passed in list of books is the active reading list in the program
        {
            new_list = books ?? new(); //create a new list if the passed in list is null
        }

        public List<Book> LoadUserData()
        {
            if (Properties.Settings.Default.titles is not null)
            {
                int count = 0;

                //obtain book data from saved user data
                foreach (var b in Properties.Settings.Default.titles)
                {
                    Book temp = new()
                    {
                        Title = Properties.Settings.Default.titles[count],
                        Author = Properties.Settings.Default.authors[count]
                    };

                    Int32.TryParse(Properties.Settings.Default.progresses[count], out int prog);//TODO create a catch here
                    temp.Progress = prog;

                    new_list.Add(temp); //add this book into the temporary list

                    count++; //increment indexer
                }
            }

            return new_list; //return the loaded list to main
        }

        /*
         * UserData.SaveUserData takes in the user's ReadingList in the program
         * as an input. It overwrites the user's list in persistent storage with 
         * this list, and saves the Settings file
         */
        public static void SaveUserData(List<Book> books)
        {
            //remove currently stored data
            if (Properties.Settings.Default.titles is not null)
            {
                Properties.Settings.Default.titles.Clear();
                Properties.Settings.Default.authors.Clear();
                Properties.Settings.Default.progresses.Clear();
            }

            //each field must be initialized to prevent null
            Properties.Settings.Default.titles = new();
            Properties.Settings.Default.authors = new();
            Properties.Settings.Default.progresses = new();


            foreach (var b in books)
            {
                Properties.Settings.Default.titles.Add(b.Title);
                Properties.Settings.Default.authors.Add(b.Author);
                Properties.Settings.Default.progresses.Add(b.Progress.ToString());
            }

            Properties.Settings.Default.Save();
        }
    }
}






