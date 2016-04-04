using ICities;

using System;

namespace RoadColorChangerContinued
{
    public class RoadColorMod : IUserMod
    {

        public const UInt64 workshop_id = 651610627;
        public string Name
        {
            get { return "Road Color Changer Continued"; }
        }

        public string Description
        {
            get { return "Changes road color. Fixed for version 1.4.0-f3"; }
        }
    }
}
