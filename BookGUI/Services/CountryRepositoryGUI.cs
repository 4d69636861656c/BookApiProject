using BookApiProject.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookGUI.Services
{
    public class CountryRepositoryGUI : ICountryRepositoryGUI
    {
        public IEnumerable<AuthorDto> GetAuthorsFromACountry(int countryId)
        {
            IEnumerable<AuthorDto> authors = new List<AuthorDto>();

            // Since we are calling the API to work and return data, we can wrap everything into a using block. 
            // This way, we don't have to worry about disposing of any objects since the Garbage Collector will do it for us. 
            using (HttpClient client = new HttpClient()) // Using the HttpClient class to call the API. 
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"countries/{countryId}/authors");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<AuthorDto>> readTask = result.Content.ReadAsAsync<IList<AuthorDto>>();
                    readTask.Wait();

                    authors = readTask.Result;
                }
            }

            return authors;
        }

        public IEnumerable<CountryDto> GetCountries()
        {
            IEnumerable<CountryDto> countries = new List<CountryDto>();

            // Since we are calling the API to work and return data, we can wrap everything into a using block. 
            // This way, we don't have to worry about disposing of any objects since the Garbage Collector will do it for us. 
            using (HttpClient client = new HttpClient()) // Using the HttpClient class to call the API. 
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync("countries");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<CountryDto>> readTask = result.Content.ReadAsAsync<IList<CountryDto>>();
                    readTask.Wait();

                    countries = readTask.Result;
                }
            }

            return countries;
        }

        public CountryDto GetCountryById(int countryId)
        {
            CountryDto country = new CountryDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"countries/{countryId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<CountryDto> readTask = result.Content.ReadAsAsync<CountryDto>();
                    readTask.Wait();

                    country = readTask.Result;
                }
            }

            return country;
        }

        public CountryDto GetCountryOfAnAuthor(int authorId)
        {
            CountryDto country = new CountryDto();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61942/api/");

                Task<HttpResponseMessage> response = client.GetAsync($"countries/authors/{authorId}");
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<CountryDto> readTask = result.Content.ReadAsAsync<CountryDto>();
                    readTask.Wait();

                    country = readTask.Result;
                }
            }

            return country;
        }
    }
}