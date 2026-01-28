using KRT.Application.DTOs;
using KRT.Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KRT.Bank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult GetAccount()
        {
            if (TempData["Account"] != null)
            {
                var account = JsonConvert.DeserializeObject<AccountDto>((string)TempData["Account"]);
                ViewBag.Account = account;
                TempData.Remove("Account");
            }
            return View();
        }

        public IActionResult UpdateAccount()
        {
            return View();
        }

        public IActionResult DeleteAccount()
        {
            return View();
        }

        public IActionResult TransferPix()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
