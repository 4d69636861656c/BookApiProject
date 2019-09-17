﻿using BookApiProject.Models;
using System.Collections.Generic;

namespace BookApiProject.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountryOfAnAuthor(int authorId);
        ICollection<Author> GetAuthorsFromACountry(int countryId);
        bool IsDuplicateCountryName(int countryId, string countryName);
        bool CountryExists(int countryId);

        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
    }
}