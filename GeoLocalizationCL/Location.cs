using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoLocalizationCL
{

    /// <summary>
    /// Class to define a Location
    /// </summary>
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Location()
        {
        }

        public Location(string name, double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Name = name.ToLower();
            Distance = 0;
        }

        /// <summary>
        /// Creates a new location that is <paramref name="offsetLat"/>, <paramref name="offsetLon"/> meters from this location.
        /// </summary>
        public Location Add(double offsetLat, double offsetLon)
        {
            double latitude = Latitude + (offsetLat / 111111d);
            double longitude = Longitude + (offsetLon / (111111d * Math.Cos(latitude)));

            return new Location("",latitude, longitude);
        }

        /// <summary>
        /// Calculates the distance between this location and another one, in meters.
        /// </summary>
        public double CalculateDistance(Location location)
        {
            try
            {
                var rlat1 = Math.PI * Latitude / 180;
                var rlat2 = Math.PI * location.Latitude / 180;
                var rlon1 = Math.PI * Longitude / 180;
                var rlon2 = Math.PI * location.Longitude / 180;
                var theta = Longitude - location.Longitude;
                var rtheta = Math.PI * theta / 180;
                var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
                dist = Math.Acos(dist);
                dist = dist * 180 / Math.PI;
                dist = dist * 60 * 1.1515;

                Distance = dist * 1609.344;
            }
            catch
            {
                Distance = 0;
            }

            return Distance;
        }

        public override string ToString()
        {
            return Latitude + ", " + Longitude;
        }
    }
}
