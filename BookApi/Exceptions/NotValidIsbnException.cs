namespace Books.Exceptions
{
    public class NotValidIsbnException:Exception
    {
        public NotValidIsbnException(string message) : base(message) { }
    }
}
