using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace primebird.portal.airtime
{
    public class Position
    {
        public virtual string Id { get; set; }
        public virtual Point Geom { get; set; }
        public virtual long Unixtime { get; set; }
        public virtual double Altitude_pressure_icao { get; set; }
        public virtual double Altitude_gnss { get; set; }

        public static Position fromIGC(string igcLine, string flightDate)
        {
            Position p = new Position();
            Coordinate c = new Coordinate();
            p.Unixtime = 0;
            string lat = igcLine.Substring(7,8);
            double latDeg = double.Parse(lat.Substring(0,2)) + (double.Parse(lat.Substring(2,2) + "." + lat.Substring(4,3)) / 60.0);
            string lon = igcLine.Substring(16,8);
            double lonDeg = double.Parse(lon.Substring(0,2)) + (double.Parse(lon.Substring(2,2) + "." + lon.Substring(4,3)) / 60.0);
            if(lon[7].ToString().ToLower() == "w")
                lonDeg *= -1;
            if(lat[7].ToString().ToLower() == "s")
                latDeg *= -1;
            string fix = igcLine[23].ToString();
            p.Altitude_pressure_icao = double.Parse(igcLine.Substring(25,5));
            p.Altitude_gnss = double.Parse(igcLine.Substring(30,5));
            p.Geom = new Point(lonDeg, latDeg, p.Altitude_gnss);
            p.Geom.SRID = 4326;
            return p;
        }
    }
}