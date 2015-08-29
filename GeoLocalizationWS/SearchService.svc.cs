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

    /// <summary>
    /// 
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SearchService : ISearchService
    {
        //Object to control the Multiple Concurrence
        readonly object ThisLock = new object();

        /// <summary>
        /// Interface Rest to provide the Results of the search based on any Location, distance of search and number of results
        /// </summary>
        /// <param name="Latitude">Latitude of the Location</param>
        /// <param name="Longitude">Longitude of the Location</param>
        /// <param name="maxDistance">Max Search Area</param>  
        /// <param name="maxResults">Max number of results to give back</param>
        /// <returns>
        ///     a json object with the SearchResults object structure
        /// </returns>
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "getlocations/{latitude}/{longitude}/{maxdistance}/{maxresults}")]
        public SearchResults GetLocations(string Latitude, string Longitude, string maxDistance, string maxResults)
        {
            ///I've implmented a class to register the Exceptions and traces of the application. 
            ///This Trace class is connected with a 3rd Party System called franrodriguez.loggly.com            

            using (Trace t = new Trace("SearchService", "GetLocations"))
            {
                using (SearchResults sr = new SearchResults())
                {
                    SearchResults srDetailed = new SearchResults();
                    try
                    {
                        //Start the Search
                        sr.StartSearch(Latitude, Longitude, maxDistance, maxResults);

                        //Lock the object to manage the interlocking of the request
                        lock (this.ThisLock)
                        {
                            //Call to the internal Controller to make the search
                            using (IGeoQuerys gq = new GeoQuerys())
                            {
                                Location location = new Location("P1", double.Parse(Latitude), double.Parse(Longitude));
                                srDetailed = gq.GetLocations(location, Int32.Parse(maxDistance), Int32.Parse(maxResults),sr);

                                //End the Search
                                srDetailed.EndSearch("");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        t.Error(e.Message.ToString(), e);
                        srDetailed.EndSearch(e.Message.ToString());
                    }

                    //Give back the json object
                    return srDetailed;
                }
            }
        }



       
    }
}
