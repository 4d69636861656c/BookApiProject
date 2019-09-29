using BookApiProject.Dtos;
using System.Collections.Generic;

namespace BookGUI.Services
{
    public interface ICountryRepositoryGUI
    {
        IEnumerable<CountryDto> GetCountries();
        CountryDto GetCountryById(int countryId);
        CountryDto GetCountryOfAnAuthor(int authorId);
        IEnumerable<AuthorDto> GetAuthorsFromACountry(int countryId);
    }
}