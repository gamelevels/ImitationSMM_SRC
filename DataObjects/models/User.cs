using System;
using System.Collections.Generic;

namespace DataObjects.models
{
    public class User
    {
        public string Username { get; set; }
        public Levels LevelInfo { get; set; }
        public DateTime Expiration { get; set; }
        public RegisterToken Register { get; set; }
        public bool Enabled { get; set; }
        public string PasswordHash { get; set; }
        public UserSettings Settings { get; set; }
    }
    public class UserVM : User
    {
        public List<Platform> Platforms { get; set; }
    }
}
