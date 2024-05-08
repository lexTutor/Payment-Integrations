using System.Collections.Generic;

namespace Integrations.Model.Api.Response
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
        public string Logo { get; set; }
    }
}
