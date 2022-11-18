using Domain.MainModule.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.MainModule.EntityConfig;

public class PostulantConfig : IEntityTypeConfiguration<Postulant>
{
    public void Configure(EntityTypeBuilder<Postulant> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Tasks)
            .WithOne(d => d.Postulant);

    }
}