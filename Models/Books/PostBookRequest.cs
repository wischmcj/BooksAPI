using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models.Books
{
    public class PostBookRequest : IValidatableObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int? NumberOfPages { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Title.ToLower() == "it" && Author.ToLower() == "king")
            {
                yield return new ValidationResult("That book is too scary", new string[]
                {nameof(Title), nameof(Author)});
            }
        }
    }
}
