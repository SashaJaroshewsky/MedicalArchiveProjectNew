﻿namespace MedicalArchive.API.Application.DTOs
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public bool IsDoctor { get; set; }
    }
}
