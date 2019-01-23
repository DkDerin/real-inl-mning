using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceWeb.ViewModels
{
    public class CategoryCreateViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "This field has to be filled.")]
        [StringLength(20), MinLength(2, ErrorMessage = "A category has to be minimum 2 characters and maximum 20")]
        public string Name { get; set; }
    }

    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "This field has to be filled.")]
        [StringLength(20), MinLength(2, ErrorMessage ="A category has to be minimum 2 characters and maximum 20")]
        public string Name { get; set; }
    }
}