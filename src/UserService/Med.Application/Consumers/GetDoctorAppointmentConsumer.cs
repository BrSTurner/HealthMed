﻿using MassTransit;
using Med.Application.Interfaces.Services;
using Med.MessageBus.Integration.Requests.Appointments;

namespace Med.application.Consumers
{
    public class GetDoctorAppointmentConsumer(IUserService userService) : IConsumer<GetDoctorByAppointmentRequest>
    {
        private readonly IUserService _userService = userService;

        public async Task Consume(ConsumeContext<GetDoctorByAppointmentRequest> context)
        {
            var response = new GetDoctorByAppointmentResponse();

            try
            {
                var doctor = await _userService.GetDoctorById(context.Message.DoctorId);
                if (doctor != null)
                {
                    response.Success = true;
                    response.DoctorId = doctor.Id;
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "Nao foi possivel achar o doutor";
                }

            }
            catch (Exception e)
            {
                response = new() { ErrorMessage = $"Algo deu errado ao criar o usuário, {e.Message}" };
            }
            finally
            {
                await context.RespondAsync(response);
            }
        }

    }
}
