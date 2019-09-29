using BookApiProject.Dtos;
using BookApiProject.Models;
using BookGUI.Services;
using BookGUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace BookGUI.Controllers
{
    public class AuthorsController : Controller
    {
        private IAuthorRepositoryGUI _authorRepository;
        private ICountryRepositoryGUI _countryRepository;
        private ICategoryRepositoryGUI _categoryRepository;

        public AuthorsController(IAuthorRepositoryGUI authorRepository, ICountryRepositoryGUI countryRepository, ICategoryRepositoryGUI categoryRepository)
        {
            _authorRepository = authorRepository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var authors = _authorRepository.GetAuthors();

            if (authors.Count() <= 0)
            {
                ViewBag.Message = "There was a problem retrieving authors from the database or no authors exist!";
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(authors);
        }

        public IActionResult GetAuthorById(int authorId)
        {
            var author = _authorRepository.GetAuthorById(authorId);

            if (author == null)
            {
                ModelState.AddModelError("", "An error occured while trying to retrieve the author!");
                ViewBag.Message = $"There was a problem retrieving the author with ID {authorId} from the database or no author with that ID exists."; ;
                author = new AuthorDto();
            }

            var country = _countryRepository.GetCountryOfAnAuthor(authorId);

            if (country == null)
            {
                ModelState.AddModelError("", "An error occured while trying to retrieve the country!");
                ViewBag.Message += $"There was a problem retrieving the country from the database or no country for the author with ID {authorId} exists.";
                country = new CountryDto();
            }

            IDictionary<BookDto, IEnumerable<CategoryDto>> bookCategories = new Dictionary<BookDto, IEnumerable<CategoryDto>>();
            IEnumerable<BookDto> books = _authorRepository.GetBooksByAuthor(authorId);

            if (books.Count() <= 0)
            {
                ViewBag.BookMessage = $"No books for author {author.FirstName} {author.LastName} exist.";
            }

            foreach (var book in books)
            {
                var categories = _categoryRepository.GetAllCategoriesOfABook(book.Id);
                bookCategories.Add(book, categories);
            }

            AuthorCountryBooksCategoriesViewModel authorCountryBooksCategoriesViewModel = new AuthorCountryBooksCategoriesViewModel()
            {
                Author = author,
                Country = country,
                BookCategories = bookCategories
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(authorCountryBooksCategoriesViewModel);
        }

        [HttpGet]
        public IActionResult CreateAuthor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAuthor(int CountryId, Author author)
        {
            using (HttpClient client = new HttpClient())
            {
                var countryDto = _countryRepository.GetCountryById(CountryId);
                if (countryDto == null || author == null)
                {
                    ModelState.AddModelError("", "Country or author is not valid! Couldn't create author object!");
                    return View(author);
                }

                author.Country = new Country()
                {
                    Id = countryDto.Id,
                    Name = countryDto.Name
                };

                client.BaseAddress = new Uri("http://localhost:61942/api/");
                var responseTask = client.PostAsJsonAsync("authors", author);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var newAuthorTask = result.Content.ReadAsAsync<Author>();
                    newAuthorTask.Wait();

                    var newAuthor = newAuthorTask.Result;
                    TempData["SuccessMessage"] = $"Author {newAuthor.FirstName} {newAuthor.LastName} was created successfully!";

                    return RedirectToAction("GetAuthorById", new { authorId = newAuthor.Id });
                }

                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "This author already exists!");
                }
                else
                {
                    ModelState.AddModelError("", "Some kind of error occurred! Author was not added!");
                }
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult UpdateAuthor(int authorId, int CountryId)
        {
            var authorDto = _authorRepository.GetAuthorById(authorId);
            var countryDto = _countryRepository.GetCountryOfAnAuthor(authorId);

            Author author = null;
            if (authorDto == null || countryDto == null)
            {
                ModelState.AddModelError("", "An error occurred! Could not update author!");
                author = new Author();
            }
            else
            {
                author = new Author()
                {
                    Id = authorDto.Id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Country = new Country()
                    {
                        Id = countryDto.Id,
                        Name = countryDto.Name
                    }
                };
            }

            return View(author);
        }

        [HttpPost]
        public IActionResult UpdateAuthor(int CountryId, Author authorToUpdate)
        {
            var countryDto = _countryRepository.GetCountryById(CountryId);
            if (countryDto == null || authorToUpdate == null)
            {
                ModelState.AddModelError("", "Invalid country or author! Cannot update!");
            }
            else
            {
                authorToUpdate.Country = new Country()
                {
                    Id = countryDto.Id,
                    Name = countryDto.Name
                };

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:61942/api/");
                    var responseTask = client.PutAsJsonAsync($"authors/{authorToUpdate.Id}", authorToUpdate);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = $"Author {authorToUpdate.FirstName} {authorToUpdate.LastName} was updated.";
                        return RedirectToAction("GetAuthorById", new { authorId = authorToUpdate.Id });
                    }

                    if ((int)result.StatusCode == 422)
                    {
                        ModelState.AddModelError("", "This author already exists!");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Some kind of error occurred! Author was not updated!");
                    }
                }
            }

            return View(authorToUpdate);
        }

        [HttpGet]
        public IActionResult DeleteAuthor(int authorId)
        {
            var authorDto = _authorRepository.GetAuthorById(authorId);
            var countryDto = _countryRepository.GetCountryOfAnAuthor(authorId);

            Author author = null;
            if (authorDto == null || countryDto == null)
            {
                ModelState.AddModelError("", "Could not retrieve author!");
                author = new Author();
            }
            else
            {
                author = new Author()
                {
                    Id = authorDto.Id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Country = new Country()
                    {
                        Id = countryDto.Id,
                        Name = countryDto.Name
                    }
                };
            }

            return View(author);
        }

        [HttpPost]
        public IActionResult DeleteAuthor(int authorId, string authorFirstName, string authorLastName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.DeleteAsync($"authors/{authorId}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Author {authorFirstName} {authorLastName} was deleted successfully!";
                    return RedirectToAction("Index");
                }

                if ((int)result.StatusCode == 409)
                {
                    ModelState.AddModelError("", $"Author {authorFirstName} {authorLastName} could not be deleted because it is associated with at least one book object!");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred! Deletion did not succeed!");
                }
            }

            var authorDto = _authorRepository.GetAuthorById(authorId);
            var countryDto = _countryRepository.GetCountryOfAnAuthor(authorId);

            Author author = null;
            if (authorDto == null || countryDto == null)
            {
                ModelState.AddModelError("", "Could not retrieve author!");
                author = new Author();
            }
            else
            {
                author = new Author()
                {
                    Id = authorDto.Id,
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Country = new Country()
                    {
                        Id = countryDto.Id,
                        Name = countryDto.Name
                    }
                };
            }

            return View(author);
        }
    }
}