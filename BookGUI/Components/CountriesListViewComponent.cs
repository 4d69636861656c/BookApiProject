using BookGUI.Models;
using BookGUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BookGUI.Components
{
    public class CountriesListViewComponent : ViewComponent
    {
        ICountryRepositoryGUI _countryRepository;

        public CountriesListViewComponent(ICountryRepositoryGUI countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IViewComponentResult Invoke()
        {
            // After selecting an item from the dropdown, the Id of that item will be returned. 
            var countries = _countryRepository.GetCountries().OrderBy(c => c.Name).Select(x => new { Id = x.Id, Value = x.Name });

            var countriesList = new CountrySelectList
            {
                CountriesList = new SelectList(countries, "Id", "Value")
            };

            return View("_CountriesList", countriesList);
        }
    }
}