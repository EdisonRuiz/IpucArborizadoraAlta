namespace IPUC.AA.Back.Models
{
    public class PaymentModel
    {
        public string Name { get; set; } = string.Empty;
        public int DocumentNumber { get; set; }
        public byte CampSpace { get; set; }
        public long Value { get; set;}
        public int TotalDebit { get; set; }
    }
}
