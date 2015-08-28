
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
        public List<Location> GetLocations(Location pLocation, int maxDistance, int maxResults)
        {

            List<Location> filteredList = new List<Location>();
            try
            {
                //Get all Locations
                List<Location> lLocations = new List<Location>();
                using (GeoData gData = new GeoData())
                {                    
                    lLocations = gData.getAllLocations();
                }

                //Sort by Distance
                List<Location> SortedList = lLocations.OrderBy(o => o.CalculateDistance(pLocation)).ToList();

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
