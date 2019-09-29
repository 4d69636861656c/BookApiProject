using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookGUI.Models
{
    public class CountrySelectList
    {
        public int CountryId { get; set; }
        public SelectList CountriesList { get; set; }
    }
}