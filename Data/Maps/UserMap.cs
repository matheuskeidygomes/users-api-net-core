using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API_Rest_ASP_Core.Models;

namespace API_Rest_ASP_Core.Data.Maps
{
    public class UserMap : IEntityTypeConfiguration<User> 
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(60).HasColumnType("varchar(60)");
            builder.Property(x => x.Email).IsRequired().HasMaxLength(60).HasColumnType("varchar(60)");
            builder.Property(x => x.Password).IsRequired().HasMaxLength(60).HasColumnType("varchar(60)");
            builder.Property(x => x.AcessToken).HasMaxLength(1000).HasColumnType("varchar(1000)");
            builder.Property(x => x.RefreshToken).HasMaxLength(1000).HasColumnType("varchar(1000)");
        }
    }
}
