using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.SharedModels.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("price")]
        [Required]
        public decimal Price { get; set; }

        [Column("sale_price")]

        public decimal? SalePrice { get; set; }

        [Column("total_sold")]

        public decimal? TotalSold { get; set; }

        [Column("img_url")]

        public string? ImgUrl { get; set; }


        [Column("description")]
        public string? Description { get; set; }

        [Column("status")]
        [EnumDataType(typeof(SharedModel.Enums.ProductStatus))]
        public SharedModel.Enums.ProductStatus Status { get; set; }

        [Column("create_date")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Column("update_date")]
        [Required]
        public DateTime UpdateDate { get; set; }
        [Column("isDeleted")]
        [Required]
        public bool isDeleted { get; set; }

        [JsonIgnore]
        [InverseProperty("Product")]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
