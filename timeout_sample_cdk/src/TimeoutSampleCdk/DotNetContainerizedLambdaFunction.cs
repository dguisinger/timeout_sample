using System;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using System.Collections.Generic;

namespace TimeoutSampleCdk
{
    public class DotNetContainerizedLambdaFunctionProps
    {
        public string ImageAssetPath { get; set; } = "../";
        public Tracing Tracing { get; set; }
        public string FunctionName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public double? MemorySize { get; set; }
        public IDictionary<string, string>? Environment { get; set; }
        public Architecture Architecture { get; set; } = Architecture.X86_64;
        public Duration? Timeout { get; set; }
    }

    public class DotNetContainerizedLambdaFunction : Construct
    {
        public Function Function { get; private set; }
        public DotNetContainerizedLambdaFunction(Construct scope, string id, DotNetContainerizedLambdaFunctionProps props) : base(scope, id)
        {
            var dockerImageCode = DockerImageCode.FromImageAsset(props.ImageAssetPath, new AssetImageCodeProps
            {
                BuildArgs = new Dictionary<string, string> {
                //{ "AWS_CONTAINER_CREDENTIALS_RELATIVE_URI", System.Environment.GetEnvironmentVariable("AWS_CONTAINER_CREDENTIALS_RELATIVE_URI")},
                //{ "AWS_REGION", System.Environment.GetEnvironmentVariable("AWS_REGION") }
            },
                Exclude = new[] { "cdk.out" },
                IgnoreMode = IgnoreMode.DOCKER
            });

            this.Function = new DockerImageFunction(this, "lambdaFunction", new DockerImageFunctionProps()
            {
                Description = props.Description,
                FunctionName = props.FunctionName,
                Tracing = props.Tracing,
                MemorySize = props.MemorySize,
                Code = dockerImageCode,
                Environment = props.Environment,
                Architecture = props.Architecture,
                Timeout = props.Timeout
            });
        }

    }

}

