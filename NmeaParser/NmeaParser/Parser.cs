using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NmeaParser
{
    public class Parser
    {
        private DateTime _routeDate { get; set; }

        public Parser()
        {
            _routeDate = DateTime.Today;
        }

        public Parser(DateTime NmeaDate)
        {
            _routeDate = NmeaDate;
        }
        public NmeaStorage Parse(string NMEA)
        {
            NmeaStorage storage = new NmeaStorage();

            string[] split = NMEA.Split(',');

            if (split[0] == "$GPGGA")
                ParseGGA(NMEA, storage);
            if (split[0] == "$GPGLL")
                ParseGLL(NMEA, storage);
            if (split[0] == "$GPGSA")
                ParseGSA(NMEA, storage);

            return storage;
        }

        public void ParseGGA(string NMEA, NmeaStorage storage)
        {
            string[] split = NMEA.Split(',');

            try
            {
                if (split[0] != "$GPGGA")
                {
                    throw new ParserWrongFormatException();
                }

                storage.Type = "GGA";

                if (!String.IsNullOrEmpty(split[1]))
                {
                    storage.Time = _routeDate;
                    storage.Time.AddHours(Convert.ToDouble(split[1].Substring(0, 2)));
                    storage.Time.AddMinutes(Convert.ToDouble(split[1].Substring(2, 2)));
                    storage.Time.AddSeconds(Convert.ToDouble(split[1].Substring(4, 2)));
                }

                if (!String.IsNullOrEmpty(split[2]))
                    storage.Latitude = float.Parse(split[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                if (!String.IsNullOrEmpty(split[3]))
                    storage.NorthSouth = split[3].First();

                if (!String.IsNullOrEmpty(split[4]))
                    storage.Longitude = float.Parse(split[4], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                if (!String.IsNullOrEmpty(split[5]))
                    storage.EastWest = split[5].First();

                if (!String.IsNullOrEmpty(split[6]))
                    storage.Quality = Convert.ToInt32(split[6]);

                if (!String.IsNullOrEmpty(split[7]))
                    storage.SattelitesInUse = Convert.ToInt32(split[7]);

                if (!String.IsNullOrEmpty(split[8]))
                    storage.HDOP = float.Parse(split[8], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                if (!String.IsNullOrEmpty(split[9]))
                    storage.Altitude = float.Parse(split[9], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                if (!String.IsNullOrEmpty(split[11]))
                    storage.GeoidSeparation = float.Parse(split[11], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

                if (!String.IsNullOrEmpty(split[13]))
                    storage.Age = Convert.ToInt32(split[13]);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ParserIncompleteStringException(ex.StackTrace);
            }
            catch (FormatException ex)
            {
                throw new ParserUnknownStringFormatException(ex.StackTrace);
            }
        }

        public void ParseGSA(string NMEA, NmeaStorage storage)
        {
            string[] split = NMEA.Split(',');

            int length = split.GetLength(0);

            try
            {
                if (split[0] != "$GPGSA")
                {
                    throw new ParserWrongFormatException();
                }

                storage.Type = "GSA";

                if (!String.IsNullOrEmpty(split[1]))
                    storage.Mode = split[1].First();
                if (!String.IsNullOrEmpty(split[2]))
                    storage.FixType = Convert.ToInt32(split[2]);
                if (!String.IsNullOrEmpty(split[length - 3]))
                    storage.PDOP = float.Parse(split[length-3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                if (!String.IsNullOrEmpty(split[length - 2]))
                    storage.HDOP = float.Parse(split[length-2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                if (!String.IsNullOrEmpty(split[length-1]))
                {
                    string[] vs = split[length-1].Split('*');
                    if(!String.IsNullOrEmpty(vs[0]))
                        storage.VDOP = float.Parse(vs[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ParserIncompleteStringException(ex.StackTrace);
            }
            catch (FormatException ex)
            {
                throw new ParserUnknownStringFormatException(ex.StackTrace);
            }
        }

        public void ParseRMC(string NMEA, NmeaStorage storage)
        {
            throw new NotImplementedException();
        }

        public void ParseGLL(string NMEA, NmeaStorage storage)
        {
            string[] split = NMEA.Split(',');

            try
            {
                if (split[0] != "$GPGLL")
                {
                    throw new ParserWrongFormatException();
                }

                storage.Type = "GLL";

                if (!String.IsNullOrEmpty(split[5]))
                {
                    storage.Time = _routeDate;
                    storage.Time.AddHours(Convert.ToDouble(split[5].Substring(0, 2)));
                    storage.Time.AddMinutes(Convert.ToDouble(split[5].Substring(2, 2)));
                    storage.Time.AddSeconds(Convert.ToDouble(split[5].Substring(4, 2)));
                }

                if (!String.IsNullOrEmpty(split[1]))
                    storage.Latitude = float.Parse(split[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                if (!String.IsNullOrEmpty(split[2]))
                    storage.NorthSouth = split[2].First();
                if (!String.IsNullOrEmpty(split[3]))
                    storage.Longitude = float.Parse(split[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                if (!String.IsNullOrEmpty(split[4]))
                    storage.EastWest = split[4].First();
                if (!String.IsNullOrEmpty(split[6]))
                    storage.Status = split[6].First();
                if (!String.IsNullOrEmpty(split[7]))
                {
                    string[] vs = split[7].Split('*');
                    if (!String.IsNullOrEmpty(vs[0]))
                        storage.ModeIndicator = vs[0].First();
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new ParserIncompleteStringException(ex.StackTrace);
            }
            catch (FormatException ex)
            {
                throw new ParserUnknownStringFormatException(ex.StackTrace);
            }
        }

        public void ParseVTG(string NMEA, NmeaStorage storage)
        {
            throw new NotImplementedException();
        }

        public void ParseZDA(string NMEA, NmeaStorage storage)
        {
            throw new NotImplementedException();
        }
    }

    public class ParserWrongFormatException : Exception
    {
        public ParserWrongFormatException()
        {
        }

        public ParserWrongFormatException(string message)
            : base(message)
        {
        }

        public ParserWrongFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class ParserIncompleteStringException : Exception
    {
        public ParserIncompleteStringException()
        {
        }

        public ParserIncompleteStringException(string message)
            : base(message)
        {
        }

        public ParserIncompleteStringException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class ParserUnknownStringFormatException : Exception
    {
        public ParserUnknownStringFormatException()
        {
        }

        public ParserUnknownStringFormatException(string message)
            : base(message)
        {
        }

        public ParserUnknownStringFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
