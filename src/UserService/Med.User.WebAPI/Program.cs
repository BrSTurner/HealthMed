using Med.Application.Extensions;
using Med.Application.Interfaces.Services;
using Med.Application.Models.Inputs;
using Med.Infrastructure.Data;
using Med.Infrastructure.Extensions;
using Med.MessageBus.Extensions;
using Med.MessageBus.Integration.Responses.Users;
using Med.Migrator;
using Med.SharedAuth;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, false);
builder.Services.AddMessageBus();
builder.Services.AddAuthorizationServices(builder.Configuration);

var app = builder.Build();

DatabaseMigrator.MigrateDatabase<UserContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var group = app.MapGroup("/api/user");

group.MapPost("Create", async (CreateUserInput input, IUserService userService) =>
{
    if (input is null)
        return Results.BadRequest();

    var result = await userService.CreateUser(input);

    if (!result.IsSuccess)
        return Results.BadRequest(result?.Errors);

    return Results.Created($"user/{(result?.Data as CreateUserResponse)?.UserId}", result?.Data);
})
.WithTags("User")
.WithName("Login")
.Produces<Created>()
.Produces<BadRequest>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
