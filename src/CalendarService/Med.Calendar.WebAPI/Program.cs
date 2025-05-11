using Med.Application.Extensions;
using Med.Application.Models;
using Med.Application.Services;
using Med.Infrastructure.Data;
using Med.Infrastructure.Extensions;
using Med.MessageBus.Extensions;
using Med.Migrator;
using Med.SharedKernel.Enumerations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, true);
builder.Services.AddMessageBus();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DatabaseMigrator.MigrateDatabase<CalendarContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var endpointGroup = app
    .MapGroup("api/calendar");

endpointGroup.MapPost(string.Empty, async (CreateDoctorCalendarInput input, ICalendarService calendarService) =>
{
    var dto = await calendarService.CreateDoctorCalendar(input);
    return Results.Created($"/calendar/{dto.Data}", dto);
})
.WithTags("Calendar")
.WithName("CreateDoctorCalendar")
.Produces<Guid>(StatusCodes.Status201Created)
.Produces<string>(StatusCodes.Status400BadRequest);


endpointGroup.MapPut(string.Empty, (UpdateDoctorCalendarInput input, ICalendarService calendarService) =>
{
    calendarService.UpdateDoctorCalendar(input);
    return Results.Ok();
})
.WithTags("Calendar")
.WithName("UpdateDoctorCalendar")
.Produces(StatusCodes.Status200OK)
.Produces<string>(StatusCodes.Status400BadRequest);


endpointGroup.MapGet("by-calendar-id/{calendarId:Guid}", async (Guid calendarId, ICalendarService calendarService) =>
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


endpointGroup.MapGet("by-doctor-id/{doctorId:Guid}", async (Guid doctorId, ICalendarService calendarService) =>
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


app.UseHttpsRedirection();

app.Run();

