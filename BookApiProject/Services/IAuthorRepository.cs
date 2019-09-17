using BookApiProject.Models;
using System.Collections.Generic;

namespace BookApiProject.Services
{
    public interface IAuthorRepository
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int authorId);
        ICollection<Author> GetAuthorsOfABook(int bookId);
        ICollection<Book> GetBooksByAuthor(int authorId);
        bool AuthorExists(int authorId);
        bool IsDuplicateAuthorName(int authorId, string authorFirstName, string authorLastName);

        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool Save();
    }
}