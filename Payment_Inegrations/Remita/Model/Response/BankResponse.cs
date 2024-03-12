using System.Collections.Generic;

namespace Remita.Model.Response
{
    public class BankResponse
    {
        public List<Bank> Banks { get; set; }
    }

    public class Bank
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankAccronym { get; set; }
        public string Type { get; set; }
    }
}
