using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPUC.AA.Back.DataBase.Entities;

public partial class TransportType
{
    [Key]
    public byte Id { get; set; }

    [StringLength(70)]
    public string Name { get; set; } = null!;

    [InverseProperty("TypeTransport")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
