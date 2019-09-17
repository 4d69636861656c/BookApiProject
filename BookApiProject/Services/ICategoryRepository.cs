using BookApiProject.Models;
using System.Collections.Generic;

namespace BookApiProject.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetAllCategoriesOfABook(int bookId);
        ICollection<Book> GetAllBooksForCategory(int categoryId);
        bool IsDuplicateCategoryName(int categoryId, string categoryName);
        bool CategoryExists(int categoryId);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}