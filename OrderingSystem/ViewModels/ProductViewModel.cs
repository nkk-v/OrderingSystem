﻿using EnvDTE;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderingSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,5)")]
        public decimal Price { get; set; }
        [Required]
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; } = true;
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public List<ProductVariantViewModel> Variants { get; set; } = new();
        public List<int> RemovedVariantIds { get; set; } = new List<int>(); // Track removed variant IDs

    }

    public class ProductVariantViewModel
    {
        public int Id { get; set; }
        public string VariantName { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
