using Med.Application.Extensions;
using Med.Application.Interfaces.Services;
using Med.Application.Models.Inputs;
using Med.Domain.Entites;
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
.WithName("CreateUser") 
.Produces<CreateUserResponse>(StatusCodes.Status201Created) 
.Produces<IEnumerable<string>>(StatusCodes.Status400BadRequest); 

group.MapGet("GetPatient", async (string cpf, IUserService userService) =>
{
    if (string.IsNullOrWhiteSpace(cpf))
        return Results.BadRequest();

    var cpfDto = new CPF(cpf);

    var result = await userService.GetPatientByCpf(cpfDto);

    if (result == null)
        return Results.NotFound();

    return Results.Ok(result);
})
.WithTags("User")
.WithName("GetPatient")
.Produces<Doctor>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

group.MapGet("GetDoctor", async (string crm, IUserService userService) =>
{
    if (string.IsNullOrWhiteSpace(crm))
        return Results.BadRequest();

    var crmDto = new CRM(crm);

    var result = await userService.GetDoctorByCrm(crmDto);

    if (result == null)
        return Results.NotFound();

    return Results.Ok(result);
})
.WithTags("User")
.WithName("GetDoctor")
.Produces<Doctor>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

app.UseAuthentication();
app.UseAuthorization();

app.Run();
