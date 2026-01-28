using KRT.Application.DTOs;
using KRT.Application.Interfaces;
using KRT.Application.Services;
using KRT.Domain.Entities;
using Moq;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Xunit;
using Document = KRT.Domain.Entities.Document;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _accountService = new AccountService(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAccountAsync_Should_Return_Account_When_Found()
    {
        var document = "12345678900";
        var accountNumber = "12345";
        var account = new Account(new Document(document), accountNumber, "000465", 1000);

        _accountRepositoryMock
            .Setup(r => r.GetAccount(It.IsAny<Document>(), accountNumber))
            .ReturnsAsync(account);

        var result = await _accountService.GetAccountAsync(document, accountNumber, false);

        Assert.NotNull(result);
        Assert.Equal(accountNumber, result.AccountNumber);
    }

    [Fact]
    public async Task GetAccountAsync_Should_Throw_When_NotFound_And_CreationFalse()
    {
        var document = "12345678900";
        var accountNumber = "12345";

        _accountRepositoryMock
            .Setup(r => r.GetAccount(It.IsAny<Document>(), accountNumber))
            .ReturnsAsync((Account)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _accountService.GetAccountAsync(document, accountNumber, false));
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Account()
    {
        var account = new Account(new Document("111"), "0001", "123", 1000);

        _accountRepositoryMock
            .Setup(r => r.GetAccount(It.IsAny<Document>(), "123"))
            .ReturnsAsync(account);

        var result = await _accountService.UpdateAsync("111", "123", 200);

        _accountRepositoryMock.Verify(r => r.UpdateAsync(account), Times.Once);
        Assert.Equal(account, result);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Account()
    {
        var account = new Account(new Document("111"), "0001", "123", 1000);

        _accountRepositoryMock
            .Setup(r => r.GetAccount(It.IsAny<Document>(), "123"))
            .ReturnsAsync(account);

        await _accountService.DeleteAsync("111", "123");

        _accountRepositoryMock.Verify(r => r.DeleteAsync(account), Times.Once);
    }
}