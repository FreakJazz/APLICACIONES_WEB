using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Receta
{
    [Key]
    [Column("receta_id")]
    public int RecetaId { get; set; }

    [Required]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Required]
    [Column("tiempo_preparacion")]
    public int TiempoPreparacion { get; set; }

    [Required]
    [StringLength(20)]
    [Column("dificultad")]
    public string Dificultad { get; set; } = string.Empty;

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [JsonIgnore]
    public virtual ICollection<RecetaIngrediente> Ingredientes { get; set; } = new List<RecetaIngrediente>();
}
