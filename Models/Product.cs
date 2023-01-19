using System.ComponentModel.DataAnnotations;
using API_Rest_ASP_Core.Models.Enums;

namespace API_Rest_ASP_Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AvailableEnum? Available { get; set; }

        public Product()
        {}

        public Product(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}
