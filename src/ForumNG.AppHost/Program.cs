var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.ForumNG_Api>("api");

builder
    .AddProject<Projects.ForumNG_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(api);

builder.Build().Run();
