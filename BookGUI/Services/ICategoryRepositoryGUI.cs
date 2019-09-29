using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.Services
{
    public interface ICategoryRepositoryGUI
    {
        IEnumerable<CategoryDto> GetCategories();
        CategoryDto GetCategoryById(int categoryId);
        IEnumerable<CategoryDto> GetAllCategoriesOfABook(int bookId);
        IEnumerable<BookDto> GetAllBooksForCategory(int categoryId);
    }
}