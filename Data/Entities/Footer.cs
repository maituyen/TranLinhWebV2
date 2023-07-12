using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyProject.Data.Interfaces;

namespace MyProject.Data.Entities;

public class Footer : IDateTracking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string? Content { get; set; }

    /// <summary>
    /// 1. Footer main
    /// 2. Footer blog
    /// </summary>
    public int Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
