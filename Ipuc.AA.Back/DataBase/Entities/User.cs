using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IPUC.AA.Back.DataBase.Entities;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public long Phone { get; set; }

    public byte CampSpace { get; set; }

    public byte? TypeTransportId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("TypeTransportId")]
    [InverseProperty("Users")]
    public virtual TransportType? TypeTransport { get; set; }
}
