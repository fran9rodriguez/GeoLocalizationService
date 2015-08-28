using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GeoLocalizationCL;
using GeoLocalizationBL;

namespace GeoLocalizationService.Controllers
{
    public class GetLocationsController : ApiController
    {      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="maxDistance"></param>
        /// <param name="maxResults"></param>
        /// <returns>
        ///     /api/GetLocations?Latitude=52.950175&Longitude=4.766285&maxDistance=10000&maxResults=100
        /// </returns>
        public List<Location> Get(double Latitude, double Longitude, int maxDistance, int maxResults)
        {
            List<Location> lLocations = new List<Location>();

            using (GeoQuerys gq = new GeoQuerys())
            { 
                Location location = new Location("P1",Latitude, Longitude);
                lLocations = gq.GetLocations(location, maxDistance, maxResults);
            }
            return lLocations;
        }
    }
}
