using NttBank.QueryAgent.Api.Extensions;

var builder = WebApplication
    .CreateSlimBuilder(args);

builder.Configure();

var app = builder.Build();

app.Configure();

await app.RunAsync();
