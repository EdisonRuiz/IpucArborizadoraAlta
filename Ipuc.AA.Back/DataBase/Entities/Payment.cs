using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPUC.AA.Back.DataBase.Entities;

public partial class Payment
{
    [Key]
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public int Value { get; set; }

    [Column(TypeName = "date")]
    public DateTime DateCreated { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Payments")]
    public virtual User User { get; set; } = null!;
}
