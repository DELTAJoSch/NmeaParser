using NmeaParser;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ParseGGA_WhenCalled_ParsesCorrectly()
        {
            Parser parser = new Parser();

            string nmea = "$GPGGA,091124.840,4813.990,N,01411.199,E,1,12,1.0,0.0,M,0.0,M,10,*67";
            NmeaStorage storage = new NmeaStorage();
            DateTime dateTime = DateTime.Today;
            dateTime.AddHours(9);
            dateTime.AddMinutes(11);
            dateTime.AddSeconds(24);

            parser.ParseGGA(nmea, storage);

            Assert.AreEqual((float)4813.990, storage.Latitude);
            Assert.AreEqual('N', storage.NorthSouth);
            Assert.AreEqual((float)01411.199, storage.Longitude);
            Assert.AreEqual('E', storage.EastWest);
            Assert.AreEqual(1, storage.Quality);
            Assert.AreEqual(12, storage.SattelitesInUse);
            Assert.AreEqual((float)1.0, storage.HDOP);
            Assert.AreEqual((float)0.0, storage.Altitude);
            Assert.AreEqual((float)0.0, storage.GeoidSeparation);
            Assert.AreEqual(10, storage.Age);
            Assert.AreEqual(dateTime, storage.Time);
        }

        [Test]
        public void ParseGGA_WhenCalledMissingInfo_ParsesCorrectly()
        {
            Parser parser = new Parser();

            string nmea = "$GPGGA,091124.840,4813.990,N,01411.199,E,1,,1.0,0.0,M,0.0,M,10,*67";
            NmeaStorage storage = new NmeaStorage();
            DateTime dateTime = DateTime.Today;
            dateTime.AddHours(9);
            dateTime.AddMinutes(11);
            dateTime.AddSeconds(24);

            parser.ParseGGA(nmea, storage);

            Assert.AreEqual((float)4813.990, storage.Latitude);
            Assert.AreEqual('N', storage.NorthSouth);
            Assert.AreEqual((float)01411.199, storage.Longitude);
            Assert.AreEqual('E', storage.EastWest);
            Assert.AreEqual(1, storage.Quality);
            Assert.AreEqual(0, storage.SattelitesInUse);
            Assert.AreEqual((float)1.0, storage.HDOP);
            Assert.AreEqual((float)0.0, storage.Altitude);
            Assert.AreEqual((float)0.0, storage.GeoidSeparation);
            Assert.AreEqual(10, storage.Age);
            Assert.AreEqual(dateTime, storage.Time);
        }

        [Test]
        public void ParseGGA_WhenCalledIncompleteString_ParserThrowsMissingInfoException()
        {
            Parser parser = new Parser();

            string nmea = "$GPGGA,091124.840,4813.990,N,01411.199,E,1,,1.0,0.0,M,0.0";
            NmeaStorage storage = new NmeaStorage();

            try
            {
                parser.ParseGGA(nmea, storage);
            }
            catch(Exception ex)
            {
                if(ex.GetType() == typeof(ParserIncompleteStringException))
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }

        [Test]
        public void ParseGGA_WhenCalledWrongFormat_ParserThrowsWrongFormatException()
        {
            Parser parser = new Parser();

            string nmea = "$GPGGA,091124.840,WRONG,N,01411.199,E,1,,1.0,0.0,M,0.0,M,10,*67";
            NmeaStorage storage = new NmeaStorage();

            try
            {
                parser.ParseGGA(nmea, storage);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ParserUnknownStringFormatException))
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }

        [Test]
        public void ParseGSA_WhenCalled_ParsesCorrectly()
        {
            Parser parser = new Parser();

            string nmea = "$GPGSA,A,3,01,02,03,04,05,06,07,08,09,10,11,12,1.0,1.0,1.0*30";
            NmeaStorage storage = new NmeaStorage();
            DateTime dateTime = DateTime.Today;
            dateTime.AddHours(9);
            dateTime.AddMinutes(11);
            dateTime.AddSeconds(24);

            parser.ParseGSA(nmea, storage);

            Assert.AreEqual('A', storage.Mode);
            Assert.AreEqual(3, storage.FixType);
            Assert.AreEqual((float)1.0, storage.PDOP);
            Assert.AreEqual((float)1.0, storage.VDOP);
            Assert.AreEqual((float)1.0, storage.HDOP);
        }

        [Test]
        public void ParseGSA_WhenCalledMissingInfo_ParsesCorrectly()
        {
            Parser parser = new Parser();

            string nmea = "$GPGSA,,,01,02,03,04,05,06,07,08,09,10,,12,1.0,1.0,*30";
            NmeaStorage storage = new NmeaStorage();
            DateTime dateTime = DateTime.Today;
            dateTime.AddHours(9);
            dateTime.AddMinutes(11);
            dateTime.AddSeconds(24);

            parser.ParseGSA(nmea, storage);

            Assert.AreEqual('\u0000', storage.Mode);
            Assert.AreEqual(0, storage.FixType);
            Assert.AreEqual((float)1.0, storage.PDOP);
            Assert.AreEqual((float)0.0, storage.VDOP);
            Assert.AreEqual((float)1.0, storage.HDOP);
        }

        [Test]
        public void ParseGSA_WhenCalledIncompleteString_ParserThrowsMissingInfoException()
        {
            Parser parser = new Parser();

            string nmea = "$GPGSA,A";
            NmeaStorage storage = new NmeaStorage();

            try
            {
                parser.ParseGSA(nmea, storage);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ParserIncompleteStringException))
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }

        [Test]
        public void ParseGSA_WhenCalledWrongFormat_ParserThrowsWrongFormatException()
        {
            Parser parser = new Parser();

            string nmea = "$GPGSA,A,Wrooong,01,02,03,04,05,06,07,08,09,10,11,12,1.0,1.0,1.0*30";
            NmeaStorage storage = new NmeaStorage();

            try
            {
                parser.ParseGSA(nmea, storage);
           }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ParserUnknownStringFormatException))
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }

        [Test]
        public void ParseGLL_WhenCalled_ParsesCorrectly()
        {
            Parser parser = new Parser();

            string nmea = "$GPGLL,4916.45,N,12311.12,W,225444,A,A*1D";
            NmeaStorage storage = new NmeaStorage();
            DateTime dateTime = DateTime.Today;
            dateTime.AddHours(22);
            dateTime.AddMinutes(54);
            dateTime.AddSeconds(44);

            parser.ParseGLL(nmea, storage);

            Assert.AreEqual((float)4916.45, storage.Latitude);
            Assert.AreEqual((float)12311.12, storage.Longitude);
            Assert.AreEqual('N', storage.NorthSouth);
            Assert.AreEqual('W', storage.EastWest);
            Assert.AreEqual(dateTime, storage.Time);
            Assert.AreEqual('A', storage.Status);
            Assert.AreEqual('A', storage.ModeIndicator);
        }

        [Test]
        public void ParseGLL_WhenCalledMissingInfo_ParsesCorrectly()
        {
            Parser parser = new Parser();

            string nmea = "$GPGLL,4916.45,,12311.12,,225444,A,*1D";
            NmeaStorage storage = new NmeaStorage();
            DateTime dateTime = DateTime.Today;
            dateTime.AddHours(9);
            dateTime.AddMinutes(11);
            dateTime.AddSeconds(24);

            parser.ParseGLL(nmea, storage);

            Assert.AreEqual((float)4916.45, storage.Latitude);
            Assert.AreEqual((float)12311.12, storage.Longitude);
            Assert.AreEqual('\u0000', storage.NorthSouth);
            Assert.AreEqual('\u0000', storage.EastWest);
            Assert.AreEqual(dateTime, storage.Time);
            Assert.AreEqual('A', storage.Status);
            Assert.AreEqual('\u0000', storage.ModeIndicator); ;
        }

        [Test]
        public void ParseGLL_WhenCalledIncompleteString_ParserThrowsMissingInfoException()
        {
            Parser parser = new Parser();

            string nmea = "$GPGLL,4916.45";
            NmeaStorage storage = new NmeaStorage();

            try
            {
                parser.ParseGLL(nmea, storage);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ParserIncompleteStringException))
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }

        [Test]
        public void ParseGLL_WhenCalledWrongFormat_ParserThrowsWrongFormatException()
        {
            Parser parser = new Parser();

            string nmea = "$GPGLL,WRONG:FORMAT,N,12311.12,W,225444,A,*1D";
            NmeaStorage storage = new NmeaStorage();

            try
            {
                parser.ParseGLL(nmea, storage);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ParserUnknownStringFormatException))
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
        }
    }
}