using KRT.Application.DTOs;
using KRT.Application.Services;
using KRT.Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KRT.Bank.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountDto accountDto)
        {
            try
            {

                var dto = new AccountDto(accountDto.Document, accountDto.AccountNumber, accountDto.AgencyNumber, accountDto.PixLimit);

                var accountRegistered = await _service.GetAccountAsync(dto.Document, dto.AccountNumber, true);
                if (accountRegistered != null)
                {
                    TempData["ErrorMessage"] = "Conta já registrada em nosso sistema";
                    return RedirectToAction("CreateAccount", "Home");
                }

                await _service.CreateAsync(dto);


                var account = await _service.GetAccountAsync(dto.Document, dto.AccountNumber, true);
                if (account == null)
                {
                    TempData["ErrorMessage"] = "Erro na criação de conta, por favor verifique as informações preenchidas";
                    return RedirectToAction("CreateAccount", "Home");
                }

                TempData["SuccessMessage"] = "Conta criada com sucesso!";
                return RedirectToAction("CreateAccount", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("CreateAccount", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(string FromDocument, string FromAccountNumber,
                                                  string ToDocument, string ToAccountNumber,
                                                  decimal Amount)
        {
            try
            {
                var result = await _service.TransferPixAsync(
                    FromDocument, FromAccountNumber,
                    ToDocument, ToAccountNumber,
                    Amount
                );

                if (result == null)
                {
                    TempData["ErrorMessage"] = "Erro na realização da transação, tente novamente mais tarde";
                }
                else
                {
                    TempData["SuccessMessage"] = $"Transferência realizada com sucesso! O novo saldo atual é de {result.PixLimit}";
                }

                return RedirectToAction("TransferPix", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro na transferência: {ex.Message}";
                return RedirectToAction("TransferPix", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(string document, string accountNumber, decimal pixLimit)
        {
            try
            {
                var account = await _service.UpdateAsync(document, accountNumber, pixLimit);

                TempData["SuccessMessage"] = $"Atualização feita com sucesso! Novo limite é de {account.PixLimit}";
                return RedirectToAction("UpdateAccount", "Home");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro na atualização da conta: {ex.Message}";
                return RedirectToAction("UpdateAccount", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get(string document, string accountNumber)
        {
            try
            {
                var account = await _service.GetAccountAsync(document, accountNumber, false);
                if (account == null)
                {
                    TempData["ErrorMessage"] = "Conta não encontrada";
                    return View("GetAccount");
                }

                var accountDto = new AccountDto(account.Document.Value, account.AccountNumber, account.AgencyNumber, account.PixLimit);

                TempData["Account"] = JsonConvert.SerializeObject(accountDto);
                return RedirectToAction("GetAccount", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("GetAccount", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string document, string accountNumber)
        {
            try
            {
                await _service.DeleteAsync(document, accountNumber);

                TempData["SuccessMessage"] = "Conta removida com sucesso!";
                return RedirectToAction("DeleteAccount", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("DeleteAccount", "Home");
            }
        }
    }
}
