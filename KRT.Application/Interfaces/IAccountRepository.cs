using KRT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRT.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task SaveAsync(Account account);
        Task<Account?> GetAccount(Document document, string accountNumber);
        Task UpdateAsync(Account account);
        Task DeleteAsync(Account account);
    }
}
