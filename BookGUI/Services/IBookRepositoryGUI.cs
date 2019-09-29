using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.Services
{
    public interface IBookRepositoryGUI
    {
        IEnumerable<BookDto> GetBooks();
        BookDto GetBookById(int bookId);
        BookDto GetBookByIsbn(string bookIsbn);
        decimal GetBookRating(int bookId);
    }
}