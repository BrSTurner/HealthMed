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
using Med.SharedKernel.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter your JWT token below:",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, false);
builder.Services.AddMessageBus();
builder.Services.AddAuthorizationServices(builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

DatabaseMigrator.MigrateDatabase<UserContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var group = app.MapGroup("/api/user");

group.MapPost("Create", [AllowAnonymous] async (CreateUserInput input, IUserService userService) =>
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

group.MapGet("GetPatient", [Authorize(Roles = "Doctor,Patient")] async (string cpf, IUserService userService) =>
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

group.MapGet("GetDoctor", [Authorize(Roles = "Doctor,Patient")] async (string crm, IUserService userService) =>
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

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.Run();
