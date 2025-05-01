using ApiAcademiaUnifor.ApiService.Dto;
using ApiAcademiaUnifor.ApiService.Models;
using ApiAcademiaUnifor.ApiService.Service.Base;
using System.Security.Cryptography.X509Certificates;

namespace ApiAcademiaUnifor.ApiService.Service
{
    public class NotificationService : ServiceBase
    {
        public NotificationService(Supabase.Client supabase) : base(supabase)
        {
        }
        public async Task<List<NotificationDto>> GetAll()
        {
            try
            {
                var notificationsResponse = await _supabase.From<Models.Notification>().Get();
                var notifications = notificationsResponse.Models.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    CreatedAt = n.CreatedAt
                }).ToList();
                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NotificationDto> GetById(int id)
        {
            try
            {
                var notification = await _supabase
                    .From<Models.Notification>()
                    .Where(n => n.Id == id)
                    .Get();
                var notificationResult = notification.Models.FirstOrDefault();

                if (notificationResult is null)
                    throw new Exception("Equipamento não encontrado");


                return new NotificationDto
                {
                    Id = notificationResult.Id,
                    Title = notificationResult.Title,
                    Description = notificationResult.Description,
                    CreatedAt = notificationResult.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NotificationDto> Post(NotificationDto notification)
        {
            try
            {

                notification.Id = 0;

                var lista = await _supabase.From<Models.Notification>().Get();

                int nextId = lista.Models.Any()
                    ? lista.Models.Max(e => e.Id) + 1
                    : 1;

                var newNotification = new Models.Notification
                {
                    Id = nextId,
                    Title = notification.Title,
                    Description = notification.Description,
                    CreatedAt = DateTime.UtcNow
                }; 
                var result = await _supabase.From<Models.Notification>().Insert(newNotification);
                var createdNotification = result.Models.FirstOrDefault();

                if (createdNotification is null)
                    throw new Exception("Erro na criação da notificação.");


                return new NotificationDto
                {
                    Id = createdNotification.Id,
                    Title = createdNotification.Title,
                    Description = createdNotification.Description,
                    CreatedAt = createdNotification.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NotificationDto> Put(int id, NotificationDto notificationDto)
        {
            try
            {

                var response = await _supabase.From<Models.Notification>().Where(x => x.Id == id).Single();

                if (response == null)
                    throw new Exception("Usuário não encontrado.");

                response.Title = notificationDto.Title;
                response.Description = notificationDto.Description;
                response.CreatedAt = notificationDto.CreatedAt;

                var notification = await response.Update<Notification>();

                var updatedNotification = notification.Models.FirstOrDefault();

                if (updatedNotification == null)
                    throw new Exception("Erro ao atualizar usuário.");

                return new NotificationDto
                {
                    Id = updatedNotification.Id,
                    Title = updatedNotification.Title,
                    Description = updatedNotification.Description,
                    CreatedAt = updatedNotification.CreatedAt,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NotificationDto> Delete(int id)
        {
            try
            {
                var response = await _supabase.From<Models.Notification>().Where(x => x.Id == id).Single();

                if (response == null)
                    throw new Exception("Usuário não encontrado.");

                await _supabase.From<Models.Notification>().Where(x => x.Id == id).Delete();

                return new NotificationDto
                {
                    Id = response.Id,
                    Title = response.Title,
                    Description = response.Description,
                    CreatedAt = response.CreatedAt
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

