using Med.Application.Extensions;
using Med.Application.Models;
using Med.Application.Services;
using Med.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Med.MessageBus.Extensions;
using Med.SharedAuth;
using Microsoft.OpenApi.Models;
using Med.Infrastructure.Data;
using Med.Migrator;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Appointment API", Version = "v1" });

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
    options.ListenAnyIP(8083, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

DatabaseMigrator.MigrateDatabase<AppointmentContext>(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var endpointGroup = app
    .MapGroup("api/appointments");

endpointGroup.MapPatch("reply/", [Authorize(Roles = "Doctor")] (ReplyAppointmentInput replyAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.ReplyAppointment(replyAppointment);
    return Results.Ok();
})
.WithTags("Appointments")
.WithName("Reply Appointment")
.Produces<Ok>()
.Produces<BadRequest>();

endpointGroup.MapPost(string.Empty, [Authorize(Roles = "Doctor,Patient")] async (CreateAppointmentInput createAppointment, IAppointmentService appointmentService) =>
{
    var dto = await appointmentService.CreateAppointment(createAppointment);
    return Results.Created($"/appointment/{dto.Data}", dto);
})
.WithTags("Appointments")
.WithName("Create Appointment")
.Produces<Created<Guid>>()
.Produces<BadRequest>();

endpointGroup.MapPatch("cancel/", [Authorize(Roles = "Patient")] (CancelAppointmentInput cancelAppointment, IAppointmentService appointmentService) =>
{
    appointmentService.CancelAppointment(cancelAppointment);
    return Results.Ok();
})
.WithTags("Appointments")
.WithName("Cancel Appoint")
.Produces<Ok>()
.Produces<BadRequest>();

endpointGroup.MapGet("by-doctor-id/{doctorId:Guid}", [Authorize(Roles = "Doctor,Patient")] async (Guid doctorId, IAppointmentService appointmentService) =>
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
app.UseAuthentication();
app.UseAuthorization();

app.Run();
