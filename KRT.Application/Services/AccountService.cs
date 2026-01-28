using KRT.Application.DTOs;
using KRT.Application.Interfaces;
using KRT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KRT.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task CreateAsync(AccountDto dto)
        {
            var documentObj = new Document(dto.Document);

            var account = new Account(documentObj, dto.AgencyNumber, dto.AccountNumber, dto.PixLimit);

            await _accountRepository.SaveAsync(account);
        }

        public async Task<Account> GetAccountAsync(string document, string accountNumber, bool creation)
        {
           var documentObj = new Document(document);

            var account = await _accountRepository.GetAccount(documentObj, accountNumber);

            if (account == null && creation == false)
                throw new KeyNotFoundException("Conta não encontrada em nossos registros, por favor verifique as informações passadas");

            return account;
        }

        public async Task<Account> TransferPixAsync(string fromDocument, string fromAccountNumber,
                                                       string toDocument, string toAccountNumber,
                                                       decimal amount)
        {
            var toAccount = await GetAccountAsync(toDocument, toAccountNumber, false);
            var fromAccount = await GetAccountAsync(fromDocument, fromAccountNumber, false);

            if (fromAccount == null || toAccount == null)
                throw new KeyNotFoundException("Conta não encontrada");

            fromAccount.TransferPix(amount);
            toAccount.SumPix(amount);

            await _accountRepository.UpdateAsync(fromAccount);
            await _accountRepository.UpdateAsync(toAccount);

            return fromAccount;
        }


        public async Task<Account> UpdateAsync(string document, string accountNumber, decimal transferPix)
        {
            var account = await GetAccountAsync(document, accountNumber, false);

            account.UpdatePix(transferPix);

            await _accountRepository.UpdateAsync(account);

            return account;
        }

        public async Task DeleteAsync(string document, string accountNumber)
        {
            var account = await GetAccountAsync(document, accountNumber, false);

            await _accountRepository.DeleteAsync(account);
        }
    }
}
