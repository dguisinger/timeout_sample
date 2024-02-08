using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FluentResults;
using MediatR;

namespace timeout_sample.Commands
{
	public class CreateTestCommand(string testValue) : IRequest<Result<string>>
	{
		public string TestValue => testValue;
	}

	public class CreateTestCommandHandler(
		IAmazonDynamoDB dynamoDb,
		TableConfig tableConfig
		) : IRequestHandler<CreateTestCommand,Result<string>>
	{
		public async Task<Result<string>> Handle(CreateTestCommand request, CancellationToken cancellationToken)
		{
			string id = Guid.NewGuid().ToString();
			return await Result.Try(async Task<string> () =>
			{
				await dynamoDb.PutItemAsync(new PutItemRequest
				{
					TableName = tableConfig.TableName,
					Item = new Dictionary<string, AttributeValue>
				{
					{ "hk", new AttributeValue { S = id } },
					{ "testValue", new AttributeValue { S = request.TestValue } }
				}
				});

				return id;
			});
		}
	}
}

