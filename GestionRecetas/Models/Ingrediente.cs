using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Ingrediente
{
    [Key]
    [Column("ingrediente_id")]
    public int IngredienteId { get; set; }

    [Required]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Column("cantidad")]
    public decimal Cantidad { get; set; }

    [Required]
    [StringLength(20)]
    [Column("unidad")]
    public string Unidad { get; set; } = string.Empty;

    [Column("calorias")]
    public int? Calorias { get; set; }

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [JsonIgnore]
    public virtual ICollection<RecetaIngrediente> Recetas { get; set; } = new List<RecetaIngrediente>();
}
