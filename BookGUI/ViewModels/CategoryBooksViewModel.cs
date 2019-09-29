using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.ViewModels
{
    public class CategoryBooksViewModel
    {
        public CategoryDto Category { get; set; }
        public IEnumerable<BookDto> Books { get; set; }
    }
}