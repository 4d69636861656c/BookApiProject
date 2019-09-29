using BookApiProject.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookGUI.Components
{
    public class CategoriesList
    {
        private List<CategoryDto> _allCategories = new List<CategoryDto>();

        public CategoriesList(List<CategoryDto> allCategories)
        {
            _allCategories = allCategories;
        }

        // For creating a new book. 
        public List<SelectListItem> GetCategoriesList()
        {
            var items = new List<SelectListItem>();
            foreach (var category in _allCategories)
            {
                items.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                    Selected = false
                });
            }

            return items;
        }

        // For updating an existing book item. 
        public List<SelectListItem> GetCategoriesList(List<int> selectedCategories)
        {
            var items = new List<SelectListItem>();
            foreach (var category in _allCategories)
            {
                items.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                    Selected = selectedCategories.Contains(category.Id) ? true : false
                });
            }

            return items;
        }
    }
}