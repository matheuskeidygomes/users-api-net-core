using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API_Rest_ASP_Core.Models;

namespace API_Rest_ASP_Core.Data.Maps
{
    public class ProductMap : IEntityTypeConfiguration<Product> // MAP 
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired().HasColumnType("int");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(60).HasColumnType("varchar(60)");
            builder.Property(x => x.Description).IsRequired().HasMaxLength(60).HasColumnType("varchar(60)");
            builder.Property(x => x.Available).IsRequired().HasMaxLength(60).HasColumnType("varchar(60)");
        }
    }
}
