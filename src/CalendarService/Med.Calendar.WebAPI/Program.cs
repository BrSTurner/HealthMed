using Med.Application.Models;
using Med.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

