using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace TimeoutSampleCdk
{
    public class TimeoutSampleCdkStack : Stack
    {
        internal TimeoutSampleCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var table = new Table(this, "TimeoutSampleTable", new TableProps
            {
                TableName = "timeout_sample_table",
                PartitionKey = new Attribute { Name = "hk", Type = AttributeType.STRING },
                SortKey = new Attribute { Name = "sk", Type = AttributeType.STRING },
                BillingMode = BillingMode.PAY_PER_REQUEST
            });

            var lambda = new DotNetContainerizedLambdaFunction(this, "tenantsLambda", new DotNetContainerizedLambdaFunctionProps
            {
                FunctionName = "timeout_sample",
                Tracing = Tracing.ACTIVE,
                MemorySize = 1024,
                Timeout = Duration.Minutes(1)
            });
            table.GrantReadWriteData(lambda.Function);

            var restApi = new RestApi(this, "TimeoutSampleApi", new LambdaRestApiProps
            {
            });

            var proxyResource = restApi.Root.AddProxy(new ProxyResourceOptions
            {
                DefaultIntegration = new LambdaIntegration(lambda.Function)
            });
            
        }
    }
}
