using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApiProject.Models
{
    // Code-first approach. 
    public class Country
    {
        // Data Annotation. 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Country Name cannot contain more than 50 characters!")]
        public string Name { get; set; }

        // Navigational property. 
        // This allows Entity Framework to override the property and create a proxy-class at runtime. 
        // This, in turn, allows for lazy-loading (avoid loading the complete tree of dependent objects that are not always needed). 
        // The entity framework tracks the relationships and changes in the database using DbContext, and with lazy-loading, we will have access to the tree of classes and we'll be able to interact with the ones we need. 
        public virtual ICollection<Author> Authors { get; set; }
    }
}