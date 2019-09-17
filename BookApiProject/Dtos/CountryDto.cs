using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProject.Dtos
{
    // Country Data Transfer Object. 
    // Dto (Data Transfer Object) - simplified objects with limited properties used to transfer only the needed data. 
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
