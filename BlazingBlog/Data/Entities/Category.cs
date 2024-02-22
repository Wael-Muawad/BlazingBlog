using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(75)]
        public string Slug { get; set; }

        public bool ShowOnNavbar { get; set; }
    }
}
