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
    .MapGroup("api/appointments");

endpointGroup.MapPatch(string.Empty, (ReplyAppointmentInput replyAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.ReplyAppointment(replyAppointment);
    return Results.Ok();
})
.WithTags("Appointments")
.WithName("Create Appoint")
.Produces<Ok>()
.Produces<BadRequest>();

endpointGroup.MapPost(string.Empty, (CreateAppointmentInput createAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.CreateAppointment(createAppointment);
    return Results.Created();
})
.WithTags("Appointments")
.WithName("Reply Appoint")
.Produces<Created<Guid>>()
.Produces<BadRequest>();

endpointGroup.MapPatch(string.Empty, (CancelAppointmentInput cancelAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.CancelAppointment(cancelAppointment);
    return Results.Ok();
})
.WithTags("Appointments")
.WithName("Cancel Appoint")
.Produces<Ok>()
.Produces<BadRequest>();

app.UseHttpsRedirection();

app.Run();
