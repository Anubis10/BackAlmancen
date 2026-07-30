using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackAlmancen.Domain.Models;

public class Producto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres")]
    public string? Description { get; set; }

    [Range(0, 100, ErrorMessage = "La restricción de edad debe estar entre 0 y 100")]
    public int? AgeRestriction { get; set; }

    [Required(ErrorMessage = "La compañía es obligatoria")]
    [MaxLength(50, ErrorMessage = "La compañía no puede exceder los 50 caracteres")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(1.00, 1000.00, ErrorMessage = "El precio debe estar entre $1 y $1000")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }
}
