﻿using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.OpenApi.Models;
using timeout_sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.EnableAnnotations();
    cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddTransient<TableConfig>(_ => new TableConfig("timeout_sample_table"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)//app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseXRay("timeout_issue");
AWSSDKHandler.RegisterXRayForAllServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
