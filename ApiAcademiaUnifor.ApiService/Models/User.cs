using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ApiAcademiaUnifor.ApiService.Models
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("birthDate")]
        public string? BirthDate { get; set; }

        [Column("avatarUrl")]
        public string? AvatarUrl { get; set; }

        [Column("is_dmin")]
        public bool IsAdmin { get; set; } = false;
    }
}
