﻿using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
