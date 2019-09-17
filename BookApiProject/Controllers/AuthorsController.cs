using BookApiProject.Dtos;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        // Injection of the IAuthorRepository into the controller constructor. 
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private ICountryRepository _countryRepository;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository, ICountryRepository countryRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _countryRepository = countryRepository;
        }

        //api/authors
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAuthors();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

            return Ok(authorsDto);
        }

        //api/authors/authorId
        [HttpGet("{authorId}", Name = "GetAuthor")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var author = _authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorDto = new AuthorDto()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };

            return Ok(authorDto);
        }

        //api/authors/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthorsOfABook(int bookId)
        {
            if (!_bookRepository.BookExists(bookId))
            {
                return NotFound();
            }

            var authors = _authorRepository.GetAuthorsOfABook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

            return Ok(authorsDto);
        }

        //api/authors/authorId/books
        [HttpGet("{authorId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var books = _authorRepository.GetBooksByAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Isbn = book.Isbn,
                    Title = book.Title,
                    DatePublished = book.DatePublished
                });
            }

            return Ok(booksDto);
        }

        //api/authors
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Author))] // Resource created.
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateAuthor([FromBody] Author authorToCreate)
        {
            if (authorToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(authorToCreate.Country.Id))
            {
                ModelState.AddModelError("", "Country doesn't exist!");
                return StatusCode(404, ModelState); // Not found. 
            }

            var author = _authorRepository.GetAuthors().Where(a => a.FirstName.Trim().ToUpper() == authorToCreate.FirstName.Trim().ToUpper()
                                                           && a.LastName.Trim().ToUpper() == authorToCreate.LastName.Trim().ToUpper()).FirstOrDefault();

            if (author != null)
            {
                ModelState.AddModelError("", $"The author {authorToCreate.FirstName} {authorToCreate.LastName} already exists!");
                return StatusCode(422, ModelState);
            }

            authorToCreate.Country = _countryRepository.GetCountry(authorToCreate.Country.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_authorRepository.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating the author {authorToCreate.FirstName} {authorToCreate.LastName}!");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorToCreate.Id }, authorToCreate);
        }

        //api/authors/authorId
        [HttpPut("{authorId}")]
        [ProducesResponseType(204)] // No content. 
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)] // Unprocessable entity. 
        [ProducesResponseType(500)]
        public IActionResult UpdateAuthor(int authorId, [FromBody]Author updatedAuthorInfo)
        {
            if (updatedAuthorInfo == null)
            {
                return BadRequest(ModelState);
            }

            if (authorId != updatedAuthorInfo.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_authorRepository.AuthorExists(authorId))
            {
                ModelState.AddModelError("", "The author you are trying to update does not exist!");
            }

            if (!_countryRepository.CountryExists(updatedAuthorInfo.Country.Id))
            {
                ModelState.AddModelError("", "Country doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(404, ModelState); // Not found. 
            }

            if (_authorRepository.IsDuplicateAuthorName(authorId, updatedAuthorInfo.FirstName, updatedAuthorInfo.LastName))
            {
                ModelState.AddModelError("", $"The author {updatedAuthorInfo.FirstName} {updatedAuthorInfo.LastName} already exists!");
                return StatusCode(422, ModelState);
            }

            updatedAuthorInfo.Country = _countryRepository.GetCountry(updatedAuthorInfo.Country.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_authorRepository.UpdateAuthor(updatedAuthorInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating the author {updatedAuthorInfo.FirstName} {updatedAuthorInfo.LastName}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/authors/authorId
        [HttpDelete("{authorId}")]
        [ProducesResponseType(204)] // No content. 
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)] // Conflict. 
        [ProducesResponseType(500)]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var authorToDelete = _authorRepository.GetAuthor(authorId);

            if (_authorRepository.GetBooksByAuthor(authorId).Count() > 0)
            {
                ModelState.AddModelError("", $"Author {authorToDelete.FirstName} {authorToDelete.LastName} cannot be deleted because it is associated with at least one book object.");
                return StatusCode(409, ModelState); // Conflict. 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_authorRepository.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the author {authorToDelete.FirstName} {authorToDelete.LastName}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}