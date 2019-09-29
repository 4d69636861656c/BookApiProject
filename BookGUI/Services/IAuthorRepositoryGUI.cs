using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.Services
{
    public interface IAuthorRepositoryGUI
    {
        IEnumerable<AuthorDto> GetAuthors();
        AuthorDto GetAuthorById(int authorId);
        IEnumerable<BookDto> GetBooksByAuthor(int authorId);
        IEnumerable<AuthorDto> GetAuthorsOfABook(int bookId);
    }
}