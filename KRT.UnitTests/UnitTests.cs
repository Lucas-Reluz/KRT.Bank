using KRT.Application.DTOs;
using KRT.Application.Interfaces;
using KRT.Application.Services;
using System.Threading.Tasks;
using Xunit;

public class AccountServiceTests
{
    [Fact]
    public async Task TransferPixAsync_DeveRetornarErro_QuandoContaNaoExiste()
    {

        var repo = new FakeAccountRepository();
        var service = new AccountService(repo);


        var result = await service.TransferPixAsync("12345678900", "0001", "98765432100", "0002", 100);


        Assert.False(result.Success);
        Assert.Equal("Conta não encontrada.", result.Message);
    }

    [Fact]
    public async Task TransferPixAsync_DeveRetornarErro_QuandoSaldoInsuficiente()
    {
        var repo = new Moq.Mock<IAccountRepository>();
        repo.SetupAdd(new AccountDto { Document = "12345678900", AccountNumber = "0001", AgencyNumber = "456456", PixLimit = 50 });
        repo.SetupAdd(new AccountDto { Document = "98765432100", AccountNumber = "0002", AgencyNumber = "234334", PixLimit = 200 });

        var service = new AccountService(repo);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.TransferPixAsync("12345678900", "0001", "98765432100", "0002", 100));

        Assert.Equal("Limite menor do que o requerido para transferir, transferência negada", exception.Message);
    }

    [Fact]
    public async Task TransferPixAsync_DeveTransferir_QuandoSaldoSuficiente()
    {
        var repo = new FakeAccountRepository();
        repo.AddAccount(new AccountDto { Document = "12345678900", AccountNumber = "0001", PixLimit = 500 });
        repo.AddAccount(new AccountDto { Document = "98765432100", AccountNumber = "0002", PixLimit = 200 });

        var service = new AccountService(repo);

        var result = await service.TransferPixAsync("12345678900", "0001", "98765432100", "0002", 100);

        Assert.True(result.Success);
        Assert.Equal("Transferência concluída.", result.Message);

        var fromAccount = await repo.GetAccountAsync("12345678900", "0001");
        var toAccount = await repo.GetAccountAsync("98765432100", "0002");

        Assert.Equal(400, fromAccount.PixLimit);
        Assert.Equal(300, toAccount.PixLimit);
    }
}