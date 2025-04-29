using EnvDTE;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderingSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; } = true;
        public IEnumerable<SelectListItem>? Categories { get; set; }



    }
}
