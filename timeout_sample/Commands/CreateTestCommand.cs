using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FluentResults;
using MediatR;
using TimeoutSample.DataLayer;

namespace timeout_sample.Commands
{
	public class CreateTestCommand(string testValue) : IRequest<Result<string>>
	{
		public string TestValue => testValue;
	}

	public class CreateTestCommandHandler(
		Repository repository
		) : IRequestHandler<CreateTestCommand,Result<string>>
	{
		public async Task<Result<string>> Handle(CreateTestCommand request, CancellationToken cancellationToken)
		{
			string id = Guid.NewGuid().ToString();

			var model = new TestModel()
			{
				Id = id,
				TestValue = request.TestValue
			};

			return await repository.CreateTest(model, cancellationToken);
		}
	}
}

