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
            return ($"Title: {Title} \nAuthor: {Author} \nProgress: {Progress}%");
        }
    }

}