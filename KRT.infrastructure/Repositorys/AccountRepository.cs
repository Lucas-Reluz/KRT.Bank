using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using KRT.Application.Interfaces;
using KRT.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KRT.infrastructure.Repositorys
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAmazonDynamoDB _dynamo;
        private readonly string _tableName;

        public AccountRepository(IAmazonDynamoDB dynamo, IConfiguration config)
        {
            _dynamo = dynamo;
            _tableName = config["DynamoDb:TableName"];
        }

        public async Task DeleteAsync(Account account)
        {
            await _dynamo.DeleteItemAsync(new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    ["PK"] = new() { S = $"DOCUMENT#{account.Document.Value}" },
                    ["SK"] = new() { S = $"ACCOUNTNUMBER#{account.AccountNumber}" }
                }
            });
        }

        public async Task<Account?> GetAccount(Document document, string accountNumber)
        {
            var response = await _dynamo.GetItemAsync(new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    ["PK"] = new() { S = $"DOCUMENT#{document.Value}" },
                    ["SK"] = new() { S = $"ACCOUNTNUMBER#{accountNumber}" }
                }
            });

            if (response.Item == null)
                return null;

            return new Account(document, accountNumber, response.Item["AgencyNumber"].S, decimal.Parse(response.Item["PixLimit"].N));
        }

        public async Task SaveAsync(Account account)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    ["PK"] = new() { S = $"DOCUMENT#{account.Document.Value}" },
                    ["SK"] = new() { S = $"ACCOUNTNUMBER#{account.AccountNumber}" },
                    ["AgencyNumber"] = new() { S = account.AgencyNumber },
                    ["PixLimit"] = new() { N = account.PixLimit.ToString() }
                }
            };

            await _dynamo.PutItemAsync(request);
        }

        public async Task UpdateAsync(Account account)
        {
            var request = new UpdateItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    ["PK"] = new() { S = $"DOCUMENT#{account.Document.Value}" },
                    ["SK"] = new() { S = $"ACCOUNTNUMBER#{account.AccountNumber}" },
                },
                UpdateExpression = "SET PixLimit = :pixLimit",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
               {
                   {":pixLimit", new AttributeValue {N = account.PixLimit.ToString()}} }
            };

            var a = account.PixLimit.ToString();

            await _dynamo.UpdateItemAsync(request);
        }
    }
}
