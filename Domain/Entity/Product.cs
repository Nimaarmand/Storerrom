using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entity
{
    // ============================== Product ==============================
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string BaseUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinStockLevel { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxStockLevel { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Weight { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        public string Barcode { get; set; }

        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public int? Number { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    // ============================== Category ==============================
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }

    // ============================== Customer ==============================
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }
    }

    // ============================== User ==============================
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;
    }

    // ============================== IssueType Enum ==============================
    public enum IssueType
    {
        Sale = 1,
        InternalUse = 2,
        Donation = 3,
        Scrap = 4,
        ReturnToSupplier = 5
    }
}

