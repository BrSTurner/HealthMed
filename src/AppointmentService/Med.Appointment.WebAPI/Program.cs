using Med.Application.Extensions;
using Med.Application.Models;
using Med.Application.Services;
using Med.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Med.MessageBus.Extensions;

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
    .MapGroup("api/appointments");

endpointGroup.MapPatch("reply/", (ReplyAppointmentInput replyAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.ReplyAppointment(replyAppointment);
    return Results.Ok();
})
.WithTags("Appointments")
.WithName("Reply Appointment")
.Produces<Ok>()
.Produces<BadRequest>();

endpointGroup.MapPost(string.Empty, async (CreateAppointmentInput createAppointment, IAppointmentService appointmentService) =>
{
    var dto = await appointmentService.CreateAppointment(createAppointment);
    return Results.Created($"/appointment/{dto.Data}", dto);
})
.WithTags("Appointments")
.WithName("Create Appointment")
.Produces<Created<Guid>>()
.Produces<BadRequest>();

endpointGroup.MapPatch("cancel/", (CancelAppointmentInput cancelAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.CancelAppointment(cancelAppointment);
    return Results.Ok();
})
.WithTags("Appointments")
.WithName("Cancel Appoint")
.Produces<Ok>()
.Produces<BadRequest>();

endpointGroup.MapGet("by-doctor-id/{doctorId:Guid}", async (Guid doctorId, IAppointmentService appointmentService) =>
{
    var result = await appointmentService.GetAppointmentsByDoctor(doctorId);

    if (result == null)
        return Results.NoContent();

    return Results.Ok(result);
})
.WithTags("Appointments")
.WithName("GetCalendarByDoctorId")
.Produces<AppointmentDTO>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status204NoContent);

app.UseHttpsRedirection();

app.Run();
