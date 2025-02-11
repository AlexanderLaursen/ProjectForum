﻿namespace WebApi.Dto
{
    public class UserDto
    {

        public string Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string Website { get; set; }
        public string ProfileImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}