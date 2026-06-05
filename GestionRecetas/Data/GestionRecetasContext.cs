using Microsoft.EntityFrameworkCore;

public class GestionRecetasContext : DbContext
{
    public GestionRecetasContext(DbContextOptions<GestionRecetasContext> options) : base(options) { }

    public DbSet<Receta> Recetas { get; set; } = null!;
    public DbSet<Ingrediente> Ingredientes { get; set; } = null!;
    public DbSet<RecetaIngrediente> RecetaIngredientes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.ToTable("Recetas");
            entity.HasKey(e => e.RecetaId);
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.TiempoPreparacion).HasColumnName("tiempo_preparacion");
            entity.Property(e => e.Dificultad).HasColumnName("dificultad");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.ToTable("Ingredientes");
            entity.HasKey(e => e.IngredienteId);
            entity.Property(e => e.IngredienteId).HasColumnName("ingrediente_id");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Unidad).HasColumnName("unidad");
            entity.Property(e => e.Calorias).HasColumnName("calorias");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
        });

        modelBuilder.Entity<RecetaIngrediente>(entity =>
        {
            entity.ToTable("RecetaIngredientes");
            entity.HasKey(e => e.RecetaIngredienteId);
            entity.Property(e => e.RecetaIngredienteId).HasColumnName("receta_ingrediente_id");
            entity.Property(e => e.RecetaId).HasColumnName("receta_id");
            entity.Property(e => e.IngredienteId).HasColumnName("ingrediente_id");
            entity.Property(e => e.CantidadUtilizada).HasColumnName("cantidad_utilizada");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
        });

        modelBuilder.Entity<RecetaIngrediente>()
            .HasOne(ri => ri.Receta)
            .WithMany(r => r.Ingredientes)
            .HasForeignKey(ri => ri.RecetaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecetaIngrediente>()
            .HasOne(ri => ri.Ingrediente)
            .WithMany(i => i.Recetas)
            .HasForeignKey(ri => ri.IngredienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecetaIngrediente>()
            .HasIndex(ri => new { ri.RecetaId, ri.IngredienteId })
            .IsUnique();
    }
}
