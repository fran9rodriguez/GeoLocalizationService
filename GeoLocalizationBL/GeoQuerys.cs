
#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLocalizationCL;
using GeoLocalizationDL;
using System.Configuration;

#endregion

namespace GeoLocalizationBL
{

    /// <summary>
    /// Class to implement the methos to query for locations
    /// </summary>
    public class GeoQuerys: IGeoQuerys
    {

        #region GetLocations

        /// <summary>
        /// Method to search locations close to other one 
        /// </summary>
        /// <param name="pLocation">Location</param>
        /// <param name="maxDistance">Max Distance</param>
        /// <param name="maxResults">Max number of Results</param>
        /// <returns></returns>
        public SearchResults GetLocations(Location pLocation, int maxDistance, int maxResults, SearchResults sr)
        {

            List<Location> filteredList = new List<Location>();
            SearchResults srDetailed = sr;
            try
            {
                //Get all Locations
                List<Location> lLocations = new List<Location>();
                using (GeoData gData = new GeoData())
                {
                    DateTime t1 = DateTime.UtcNow;

                    lLocations = gData.getAllLocations();
                    
                    DateTime t2 = DateTime.UtcNow;
                    TimeSpan t = t2 - t1;
                    double d = t.TotalSeconds;
                    srDetailed.ReadDataDuration = d;
                    srDetailed.FileRecords = lLocations.Count;
                }

                //Sort by Distance
                List<Location> SortedList = lLocations.OrderBy(o => o.CalculateDistance(pLocation)).ToList();

                //Filter the Locations with the same Distance, Longitude and Latitude
                List<Location> filterRepeated = SortedList.GroupBy(x => new { x.Distance, x.Longitude, x.Latitude })
                                                   .Select(g => g.First())
                                                   .ToList();

                //Filter by the max Number of Results
                filteredList = filterRepeated.Where(x => x.Distance <= maxDistance).Take(maxResults).ToList();
                srDetailed.Locations = filteredList;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return srDetailed;
        }

        #endregion

        #region GetLocations Async

        /// <summary>
        /// Method to search locations close to other one 
        /// </summary>
        /// <param name="pLocation">Location</param>
        /// <param name="maxDistance">Max Distance</param>
        /// <param name="maxResults">Max number of Results</param>
        /// <returns></returns>
        public async Task<List<Location>> GetLocationsAsync(Location pLocation, int maxDistance, int maxResults)
        {

            List<Location> filteredList = new List<Location>();
            try
            {
                //Get all Locations
                List<Location> lLocationsJoined = new List<Location>();

                using (GeoData gData = new GeoData())
                {
                    int totalL = 1000000;
                    int paralellism = 100000;
                    int nThreads = totalL / paralellism;
                    int[] arrayRanges = new int[nThreads];

                    for (int i = 0; i < nThreads; i++) arrayRanges[i] = i * paralellism;

                    IEnumerable<Task<List<Location>>> getLocationsTasksQuery = from range in arrayRanges
                                                                               select gData.getAllLocationsByRange(range,paralellism);

                    // Use ToArray to execute the query and start the getLocations tasks.
                    Task<List<Location>>[] downloadTasks = getLocationsTasksQuery.ToArray();

                    List<Location>[] lLocations = await Task.WhenAll(downloadTasks);
                    
                }

                //Sort by Distance
                List<Location> SortedList = lLocationsJoined.OrderBy(o => o.CalculateDistance(pLocation)).ToList();

                //Filter the Locations with the same Distance, Longitude and Latitude
                List<Location> filterRepeated = SortedList.GroupBy(x => new { x.Distance, x.Longitude, x.Latitude })
                                                   .Select(g => g.First())
                                                   .ToList();

                //Filter by the max Number of Results
                filteredList = filterRepeated.Where(x => x.Distance <= maxDistance).Take(maxResults).ToList();
            }
            catch
            {
                throw;
            }

            return filteredList;
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            Close();
        }
        public void Close()
        {
            // Do what's necessary to close the file
            System.GC.SuppressFinalize(this);
        }

        #endregion

    }
}
