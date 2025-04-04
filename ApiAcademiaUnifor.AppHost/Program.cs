var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ApiAcademiaUnifor_ApiService>("apiservice");

builder.AddProject<Projects.ApiAcademiaUnifor_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
