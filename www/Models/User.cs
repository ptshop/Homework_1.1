﻿namespace www.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } // TODO: Change to PasswordHash
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Genders Gender { get; set; }
        public string Interest { get; set; }
        public string City { get; set; }
    }
}
