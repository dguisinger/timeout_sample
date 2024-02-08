using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FluentResults;

namespace TimeoutSample.DataLayer
{
	public class Repository(IAmazonDynamoDB dynamoDbClient, TableConfig tableConfig)
	{

		public async Task<Result<string>> CreateTest(TestModel testModel, CancellationToken cancellationToken = default)
		{
            return await Result.Try(async Task<string> () =>
            {
                await dynamoDbClient.PutItemAsync(new PutItemRequest
                {
                    TableName = tableConfig.TableName,
                    Item = new Dictionary<string, AttributeValue>
                {
                    { "hk", new AttributeValue { S = testModel.Id } },
                    { "testValue", new AttributeValue { S = testModel.TestValue } }
                }
                });

                return testModel.Id;
            });
        }
	}
}

