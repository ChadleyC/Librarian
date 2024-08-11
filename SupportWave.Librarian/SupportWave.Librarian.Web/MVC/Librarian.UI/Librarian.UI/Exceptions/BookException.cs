namespace Librarian.UI.Exceptions;

public class BookException : Exception
{
    public BookException() : base() { }

    public BookException(string message): base(message) { }

    public BookException(Exception ex) : base(ex.Message, ex){}
}