using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Data.Entities;

public class ProductHastag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public int HasTagId { get; set; }
    public virtual Hastag? Hastag { get; set; }
}
