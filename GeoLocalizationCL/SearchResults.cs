
#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace GeoLocalizationCL
{
    /// <summary>
    /// Class to define the results of a search
    /// </summary>
    public class SearchResults: ISearchResults
    {
        public DateTime ts { get; set; } //start time
        public DateTime te { get; set; } //end time
        public double TotalDuration { get; set; } //duration of the search
        public double ReadDataDuration { get; set; } //duration of the search
        public string Latitude { get; set; } 
        public string Longitude { get; set; }
        public string Distance { get; set; }
        public string maxResults { get; set; }
        public List<Location> Locations { get; set; }
        public string Error { get; set; }
        public int FileRecords { get; set; }


        public SearchResults() { }

        /// <summary>
        /// Method to store the start time and the search parameters
        /// </summary>
        /// <param name="pLatitude">Latitude</param>
        /// <param name="pLongitude">Longitude</param>
        /// <param name="pDistance">Distance</param>
        /// <param name="pmaxResults">Max number of Results</param>
        public void StartSearch(string pLatitude, string pLongitude, string pDistance, string pmaxResults)
        {
            ts = DateTime.Now;
            Latitude = pLatitude;
            Longitude = pLongitude;
            Distance = pDistance;
            maxResults = pmaxResults;
        }


        /// <summary>
        /// Method to calculate the duration of the search and store the results
        /// </summary>
        /// <param name="plLocations">Locations</param>
        public void EndSearch(string sError)
        {            
            te = DateTime.Now;
            TimeSpan t = te - ts;
            TotalDuration = t.TotalSeconds;
            Error = sError;            
        }

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
