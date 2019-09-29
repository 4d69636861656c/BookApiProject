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
    public class CountriesController : Controller
    {
        // Dependency injection. 
        private ICountryRepositoryGUI _countryRepository;

        public CountriesController(ICountryRepositoryGUI countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IActionResult Index()
        {
            var countries = _countryRepository.GetCountries();
            //var countries = new List<CountryDto>();

            if (countries.Count() <= 0)
            {
                ViewBag.Message = "There was a problem retrieving countries from the database or no countries exist!";
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(countries);
        }

        public IActionResult GetCountryById(int countryId)
        {
            var country = _countryRepository.GetCountryById(countryId);
            //country = null;

            if (country == null)
            {
                ModelState.AddModelError("", "Error getting a country!");
                ViewBag.Message = $"There was a problem retrieving the country with ID {countryId} + from the database or no country with that ID exists.";
                country = new CountryDto();
            }

            var authors = _countryRepository.GetAuthorsFromACountry(countryId);

            if (authors.Count() <= 0)
            {
                ViewBag.AuthorMessage = $"There are no authors from the country with ID {country.Id}.";
            }

            var countryAuthorsViewModel = new CountryAuthorsViewModel()
            {
                Country = country,
                Authors = authors
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(countryAuthorsViewModel);
        }

        [HttpGet]
        public IActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCountry(Country country)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.PostAsJsonAsync("countries", country);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var newCountryTask = result.Content.ReadAsAsync<Country>();
                    newCountryTask.Wait();

                    var newCountry = newCountryTask.Result;
                    TempData["SuccessMessage"] = $"Country {newCountry.Name} was successfully created.";

                    return RedirectToAction("GetCountryById", new { countryId = newCountry.Id });
                }

                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "Country already exists!");
                }
                else
                {
                    ModelState.AddModelError("", "Some kind of error occured! Country was not created!");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult UpdateCountry(int countryId)
        {
            var countryToUpdate = _countryRepository.GetCountryById(countryId);
            if (countryToUpdate == null)
            {
                ModelState.AddModelError("", "There was an error getting the country!");
                countryToUpdate = new CountryDto();
            }

            return View(countryToUpdate);
        }

        [HttpPost]
        public IActionResult UpdateCountry(Country countryToUpdate)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.PutAsJsonAsync($"countries/{countryToUpdate.Id}", countryToUpdate);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Country {countryToUpdate.Name} was successfully updated.";

                    return RedirectToAction("GetCountryById", new { countryId = countryToUpdate.Id });
                }

                if ((int)result.StatusCode == 422)
                {
                    ModelState.AddModelError("", "Country already exists!");
                }
                else
                {
                    ModelState.AddModelError("", "Some kind of error occured! Country not updated!");
                }
            }

            var countryDto = _countryRepository.GetCountryById(countryToUpdate.Id);

            return View(countryDto);
        }

        [HttpGet]
        public IActionResult DeleteCountry(int countryId)
        {
            var country = _countryRepository.GetCountryById(countryId);

            if (country == null)
            {
                ModelState.AddModelError("", "An error occured! The country doesn't exist!");
                country = new CountryDto();
            }

            return View(country);
        }

        [HttpPost]
        public IActionResult DeleteCountry(int countryId, string countryName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                var responseTask = client.DeleteAsync($"countries/{countryId}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = $"Country {countryName} was successfully deleted.";

                    return RedirectToAction("Index");
                }

                if ((int)result.StatusCode == 409) // Conflict. 
                {
                    ModelState.AddModelError("", $"Country {countryName} cannot be deleted because it is associated with at least one author object!");
                }
                else
                {
                    ModelState.AddModelError("", "An error occured! Country was not deleted!");
                }
            }

            // If the deletetion was not successful, the same view for the country that the user is trying to delete is returned. 
            var countryDto = _countryRepository.GetCountryById(countryId);

            return View(countryDto);
        }
    }
}