﻿using System.Text.Json.Serialization;

namespace ApiAcademiaUnifor.ApiService.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? BirthDate { get; set; }
        public string? AvatarUrl { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsAdmin { get; set; } = null;
    }

}
