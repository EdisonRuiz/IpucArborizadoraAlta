﻿namespace IPUC.AA.Back.Models
{
    public class PaymentModel
    {
        public string Name { get; set; } = string.Empty;
        public byte CampSpace { get; set; }
        public int Value { get; set;}
        public int TotalDebit { get; set; }
    }
}
