using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GeoLocalizationBL;
using GeoLocalizationCL;

namespace GeoLocalizationWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SearchService : ISearchService
    {
        readonly object ThisLock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="maxDistance"></param>  
        /// <param name="maxResults"></param>
        /// <returns></returns>
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "getlocations/{latitude}/{longitude}/{maxdistance}/{maxresults}")]
        public SearchResults GetLocations(string Latitude, string Longitude, string maxDistance, string maxResults)
        {
            List<Location> lLocations = new List<Location>();

            using (Trace t = new Trace("SearchService", "GetLocations"))
            {
                using (SearchResults sr = new SearchResults())
                {
                    try
                    {
                        sr.StartSearch(Latitude, Longitude, maxDistance, maxResults);
                        lock (this.ThisLock)
                        {
                            using (IGeoQuerys gq = new GeoQuerys())
                            {
                                Location location = new Location("P1", double.Parse(Latitude), double.Parse(Longitude));
                                lLocations = gq.GetLocations(location, Int32.Parse(maxDistance), Int32.Parse(maxResults));
                                sr.EndSearch(lLocations, "");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        t.Error(e.Message.ToString(), e);
                        sr.EndSearch(null, e.Message.ToString());
                    }

                    return sr;
                }
            }
        }



       
    }
}
