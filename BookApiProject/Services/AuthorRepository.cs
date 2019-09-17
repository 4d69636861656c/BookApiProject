using BookApiProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookApiProject.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        // Injection of the BookDbContext into the class constructor. 
        BookDbContext _authorContext;

        public AuthorRepository(BookDbContext authorContext)
        {
            _authorContext = authorContext;
        }
        public bool AuthorExists(int authorId)
        {
            return _authorContext.Authors.Any(a => a.Id == authorId);
        }

        public bool CreateAuthor(Author author)
        {
            _authorContext.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _authorContext.Remove(author);
            return Save();
        }

        public Author GetAuthor(int authorId)
        {
            return _authorContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.OrderBy(a => a.LastName).ToList();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return _authorContext.BookAuthors.Where(ba => ba.BookId == bookId).Select(a => a.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _authorContext.BookAuthors.Where(ba => ba.AuthorId == authorId).Select(b => b.Book).ToList();
        }

        public bool IsDuplicateAuthorName(int authorId, string authorFirstName, string authorLastName)
        {
            var author = _authorContext.Authors.Where(a => a.Id != authorId && a.FirstName.Trim().ToUpper() == authorFirstName.Trim().ToUpper()
                                                           && a.LastName.Trim().ToUpper() == authorLastName.Trim().ToUpper()).FirstOrDefault();

            return author == null ? false : true;
        }

        public bool Save()
        {
            var saved = _authorContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _authorContext.Update(author);
            return Save();
        }
    }
}