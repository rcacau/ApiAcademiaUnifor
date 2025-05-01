using Supabase.Postgrest.Attributes;

namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
