using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.ViewModels
{
    public class CountryAuthorsViewModel
    {
        public CountryDto Country { get; set; }
        public IEnumerable<AuthorDto> Authors { get; set; }
    }
}