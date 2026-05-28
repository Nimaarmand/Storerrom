using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    // ============================== Category ==============================
    // مدل صحیح پیشنهادی:
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
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // تغییر به DateTime
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>(); // تغییر به ICollection
    }
}

