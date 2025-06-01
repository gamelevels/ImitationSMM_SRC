using System.Collections.Generic;

namespace DataObjects.models
{
    public class Levels
    {
        public int Level { get; set; }
        public int HandleCooldown { get; set; }
        public List<Platform> Platforms { get; set; }
    }
}