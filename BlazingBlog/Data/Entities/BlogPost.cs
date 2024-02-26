using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Data.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(125)]
        public string Slug { get; set; }

        [MaxLength(100)]
        public string Image { get; set; }

        [Required, MaxLength(500)]
        public string Introduction { get; set; }

        //[Required]
        public string Content { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid catigory")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public bool IsPublished { get; set; }

        public int ViewCount { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        // Navigation property to represent the relationship with Category
        public Category Category { get; set; }

        // Navigation property to represent the relationship with ApplicationUser
        public ApplicationUser User { get; set; }
    }
}
