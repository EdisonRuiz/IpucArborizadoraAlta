namespace IPUC.AA.Back.Models
{
    public class AddUserModel
    {
        public string Name { get; set; } = string.Empty;
        public int DocumentNumber { get; set; }
        public Int64 Phone { get; set; }
        public byte CampSpace { get; set; }
        public TypeTransports TypeTransportId { get; set; }
    }
}
