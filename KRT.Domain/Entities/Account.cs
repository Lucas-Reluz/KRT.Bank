using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRT.Domain.Entities
{
    public class Account
    {
        public Document Document { get; private set; }
        public string AgencyNumber { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal PixLimit { get; private set; }

        protected Account() { }

        public Account(Document document, string accountNumber, string agencyNumber, decimal pixLimit)
        {
            Document = document;
            AgencyNumber = agencyNumber;
            AccountNumber = accountNumber;
            PixLimit = pixLimit;
        }

        public void SumPix(decimal newTransaction)
        {
            PixLimit += newTransaction;
        }
        
        public void UpdatePix(decimal newTransaction)
        {
            PixLimit = newTransaction;
        }

        public void TransferPix(decimal newTransaction)
        {
            if (newTransaction <= 0)
                throw new ArgumentException("Valor da transferência deve ser maior que zero");

            if (PixLimit < newTransaction)
                throw new InvalidOperationException("Limite menor do que o requerido para transferir, transferência negada!");

            PixLimit -= newTransaction;
        }
    }
}
