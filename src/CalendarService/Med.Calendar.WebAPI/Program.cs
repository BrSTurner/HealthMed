using Med.Application.Extensions;
using Med.Application.Models;
using Med.Application.Services;
using Med.Infrastructure.Data;
using Med.Infrastructure.Extensions;
using Med.MessageBus.Extensions;
using Med.Migrator;
using Med.SharedAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
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

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8084);
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

DatabaseMigrator.MigrateDatabase<CalendarContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var endpointGroup = app
    .MapGroup("api/calendar");

endpointGroup.MapPost(string.Empty, [Authorize(Roles = "Doctor")] async (CreateDoctorCalendarInput input, ICalendarService calendarService) =>
{
    var dto = await calendarService.CreateDoctorCalendar(input);
    return Results.Created($"/calendar/{dto.Data}", dto);
})
.WithTags("Calendar")
.WithName("CreateDoctorCalendar")
.Produces<Guid>(StatusCodes.Status201Created)
.Produces<string>(StatusCodes.Status400BadRequest);


endpointGroup.MapPut(string.Empty, [Authorize(Roles = "Doctor")]
async (UpdateDoctorCalendarInput input, ICalendarService calendarService) =>
{
    await calendarService.UpdateDoctorCalendar(input);
    return Results.Ok();
})
.WithTags("Calendar")
.WithName("UpdateDoctorCalendar")
.Produces(StatusCodes.Status200OK)
.Produces<string>(StatusCodes.Status400BadRequest);


endpointGroup.MapGet("by-calendar-id/{calendarId:Guid}", [Authorize(Roles = "Doctor,Patient")] async (Guid calendarId, ICalendarService calendarService) =>
{
    var result = await calendarService.GetCalendarById(calendarId);

    if (result == null)
        return Results.NoContent();

    return Results.Ok(result);
})
.WithTags("Calendar")
.WithName("GetCalendarById")
.Produces<CalendarDTO>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status204NoContent);


endpointGroup.MapGet("by-doctor-id/{doctorId:Guid}", [Authorize(Roles = "Doctor,Patient")] async (Guid doctorId, ICalendarService calendarService) =>
{
    var result = await calendarService.GetCalendarByDoctorId(doctorId);

    if (result == null)
        return Results.NoContent();

    return Results.Ok(result);
})
.WithTags("Calendar")
.WithName("GetCalendarByDoctorId")
.Produces<CalendarDTO>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status204NoContent);


app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.Run();

