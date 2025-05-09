using Med.Application.Models;
using Med.Application.Services;
using Med.Application.Extensions;
using Med.MessageBus.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Med.Infrastructure.Extensions;
using Med.SharedAuth;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Med.Infrastructure.Data;
using Med.Migrator;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calendar API", Version = "v1" });

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

var app = builder.Build();

DatabaseMigrator.MigrateDatabase<CalendarContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var endpointGroup = app
    .MapGroup("api/calendar");

endpointGroup.MapPost(string.Empty, (CreateDoctorCalendarInput createDoctorCalendarInput, ICalendarService calendarService) =>
{
    calendarService.CreateDoctorCalendar(createDoctorCalendarInput);
    return Results.Created();
})
.WithTags("Calendar")
.WithName("Create Calendar For the Doctor")
.Produces<Created<Guid>>()
.Produces<BadRequest>();

endpointGroup.MapPut(string.Empty, (UpdateDoctorCalendarInput updateDoctorCalendarInput, ICalendarService calendarService) =>
{
    calendarService.UpdateDoctorCalendar(updateDoctorCalendarInput);
    return Results.Ok();
})
.WithTags("Calendar")
.WithName("Update Calendar For the Doctor")
.Produces<Created<Guid>>()
.Produces<BadRequest>();


endpointGroup.MapGet("/{doctorId:Guid}", [Authorize(Roles = "Doctor,Patient")] async (Guid doctorId, ICalendarService calendarService) =>
{
    var result = await calendarService.GetAvailableCalendarsByDoctor(doctorId);

    if (result == null)
        return Results.NoContent();

    return Results.Ok(result);

})
.WithTags("Calendars")
.WithName("Get Available Calendars By DoctorId")
.Produces<Ok>()
.Produces<NoContent>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

