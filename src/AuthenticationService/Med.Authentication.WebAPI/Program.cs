using Med.Application.Extensions;
using Med.Application.Interfaces.Services;
using Med.Authentication.WebAPI.Inputs;
using Med.Infrastructure.Data;
using Med.Infrastructure.Extensions;
using Med.MessageBus.Extensions;
using Med.Migrator;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, true);
builder.Services.AddMessageBus();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081);
});

var app = builder.Build();

DatabaseMigrator.MigrateDatabase<AuthContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var group = app.MapGroup("/api/auth");

group.MapPost("Login", async (AuthenticateInput input, IAuthService authService) =>
{
    if (input is null)
        return Results.BadRequest();

    var token = await authService.Authenticate(input.UsernameOrEmail, input.Password);

    if(token is null)
        return Results.Unauthorized();
    
    return Results.Ok(token);
})
.WithTags("Auth")
.WithName("Login")
.Produces<Ok>()
.Produces<UnauthorizedHttpResult>()
.Produces<BadRequest>();

app.Run();