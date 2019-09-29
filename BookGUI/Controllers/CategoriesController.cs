using BookApiProject.Dtos;
using BookApiProject.Models;
using BookGUI.Services;
using BookGUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;

namespace BookGUI.Controllers
{
    public class CategoriesController : Controller
    {
        private ICategoryRepositoryGUI _categoryRepository;

        public CategoriesController(ICategoryRepositoryGUI categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetCategories();
            // var categories = new List<CategoryDto>();

            if (categories.Count() <= 0)
            {
                ViewBag.Message = "There was a problem retrieving categories or no categories exist!";
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(categories);
        }

        public IActionResult GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            //category = null;

            if (category == null)
            {
                ModelState.AddModelError("", "Error getting a category!");
                ViewBag.Message = $"There was a problem retrieving the category with ID {categoryId} from the database or no category with that ID exists.";
                category = new CategoryDto();
            }

            var books = _categoryRepository.GetAllBooksForCategory(categoryId);

            if (books.Count() <= 0)
            {
                ViewBag.BookMessage = $"The category \"{category.Name}\" has no books.";
            }

            var bookCategoryViewModel = new CategoryBooksViewModel()
            {
                Category = category,
                Books = books
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(bookCategoryViewModel);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.PostAsJsonAsync("categories", category);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var newCategoryTask = result.Content.ReadAsAsync<Category>();
                    newCategoryTask.Wait();

                    var newCategory = newCategoryTask.Result;
                    TempData["SuccessMessage"] = $"Category {newCategory.Name} was created successfully.";

                    return RedirectToAction("GetCategoryById", new { categoryId = newCategory.Id });
                }

                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "This category already exists!");
                }
                else
                {
                    ModelState.AddModelError("", "Some kind of error occurred! Category wasn't created!");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult UpdateCategory(int categoryId)
        {
            var categoryToUpdate = _categoryRepository.GetCategoryById(categoryId);
            if (categoryToUpdate == null)
            {
                ModelState.AddModelError("", "An error occurred while trying to retrieve the category!");
                categoryToUpdate = new CategoryDto();
            }

            return View(categoryToUpdate);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category categoryToUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.PutAsJsonAsync($"categories/{categoryToUpdate.Id}", categoryToUpdate);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"The category {categoryToUpdate.Name} was updated successfully.";

                    return RedirectToAction("GetCategoryById", new { categoryId = categoryToUpdate.Id });
                }

                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "Category already exists!");
                }
                else
                {
                    ModelState.AddModelError("", "Some kind of error occurred! As a result, category was not updated!");
                }
            }

            var categoryDto = _categoryRepository.GetCategoryById(categoryToUpdate.Id);

            return View(categoryDto);
        }

        [HttpGet]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);

            if (category == null)
            {
                ModelState.AddModelError("", "An error occurred! The category does not exist!");
                category = new CategoryDto();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId, string categoryName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.DeleteAsync($"categories/{categoryId}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Category {categoryName} was deleted successfully!";

                    return RedirectToAction("Index");
                }

                if ((int)result.StatusCode == 409)
                {
                    ModelState.AddModelError("", $"Category {categoryName} cannot be deleted as it is associated to at least one book object!");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred! Category was not deleted!");
                }
            }

            // If the deletetion was not successful, the same view for the category that the user is trying to delete is returned. 
            var categoryDto = _categoryRepository.GetCategoryById(categoryId);

            return View(categoryDto);
        }
    }
}