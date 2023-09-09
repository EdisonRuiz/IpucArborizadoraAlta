namespace IPUC.AA.Back.Models
{
    public class DebitResponseModel
    {       
        public string Message { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public long Phone { get; set; }
        public bool IsOK { get; set; }
    }
}
