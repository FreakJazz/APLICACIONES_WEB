using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class RecetaIngrediente
{
    [Key]
    [Column("receta_ingrediente_id")]
    public int RecetaIngredienteId { get; set; }

    [Required]
    [ForeignKey("Receta")]
    [Column("receta_id")]
    public int RecetaId { get; set; }

    [Required]
    [ForeignKey("Ingrediente")]
    [Column("ingrediente_id")]
    public int IngredienteId { get; set; }

    [Required]
    [Column("cantidad_utilizada")]
    public decimal CantidadUtilizada { get; set; }

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [JsonIgnore]
    public virtual Receta? Receta { get; set; }
    
    [JsonIgnore]
    public virtual Ingrediente? Ingrediente { get; set; }
}
