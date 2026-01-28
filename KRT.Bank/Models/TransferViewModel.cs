using KRT.Application.DTOs;

namespace KRT.Bank.Models
{
    public class TransferViewModel
    {
        public List<AccountDto> Accounts { get; set; }

        public string FromDocument { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToDocument { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }

    }
}
