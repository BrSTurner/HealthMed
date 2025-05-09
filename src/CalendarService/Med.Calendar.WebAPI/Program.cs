using Med.Application.Models;
using Med.Application.Services;
using Med.Application.Extensions;
using Med.MessageBus.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Med.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, true);
builder.Services.AddMessageBus();

var app = builder.Build();

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


endpointGroup.MapGet("/{doctorId:Guid}", async (Guid doctorId, ICalendarService calendarService) =>
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

app.Run();

