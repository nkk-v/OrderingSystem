﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingSystem.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrderNum { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public DateTime? ScheduledTimeStart { get; set; }
        public DateTime? ScheduledTimeEnd { get; set; }
        public string Fullname { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string? DeliveryNote { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal SubTotal { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal DeliveryFee { get; set; }
        public string DeliveryStatus { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}
