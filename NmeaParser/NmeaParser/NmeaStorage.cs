using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser
{
    public class NmeaStorage
    {
        public DateTime Time { get; set; }
        public char NorthSouth { get; set; }
        public char EastWest { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Quality { get; set; }
        public int SattelitesInUse { get; set; }
        public float HDOP { get; set; }
        public float Altitude { get; set; }
        public float GeoidSeparation { get; set; }
        public int Age { get; set; }
        public int StationId { get; set; }
        public char Status { get; set; }
        public char ModeIndicator { get; set; }
        public char Mode { get; set; }
        public int FixType { get; set; }
        public float PDOP { get; set; }
        public float VDOP { get; set; }
        public string Type { get; set; }
    }
}
