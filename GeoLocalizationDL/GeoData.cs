
#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GeoLocalizationCL;
using System.Configuration;
using System.Web;
using System.Runtime.Caching;

#endregion

namespace GeoLocalizationDL
{
    public class GeoData : IDisposable
    {
        static MemoryCache MemCache;
        static int RefreshInterval = 120;

        /// <summary>
        /// Method to get all locations 
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        public List<Location> getAllLocations()
        {
            try
            {
                if (MemCache == null) MemCache = new MemoryCache("MemCache");
                List<Location> lLocations = new List<Location>();
                if (!MemCache.Contains("ListOfLocations"))
                {
                    string dataSource = ConfigurationManager.AppSettings["dataSource"];
                    switch (dataSource)
                    {
                        case "CSV":
                            string fileName = ConfigurationManager.AppSettings["fileLocations"];
                            lLocations = getAllLocations(fileName);
                           
                            var policy = new CacheItemPolicy
                            {
                                UpdateCallback = new CacheEntryUpdateCallback(CacheEntryUpdate),
                                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(RefreshInterval)
                            };
                            
                            MemCache.Set("ListOfLocations", lLocations, policy);

                            break;
                        default:
                            break;
                    }


                    //slLocations = lLocations;
                }
                else lLocations = (List<Location>)MemCache["ListOfLocations"];

                return lLocations;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Method to get all locations from a .csv
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        public List<Location> getAllLocations(string filename)
        {
            try
            {
                var qLocations = from location in File.ReadAllLines(filename).Skip(1)
                                 let data = location.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries)
                                 select new Location
                                 {
                                     Name = getName(data),
                                     Latitude = getLatitude(data),
                                     Longitude = getLongitude(data)
                                 };

                return qLocations.ToList();
            }
            catch
            {
                throw;
            }

        }

        #region Private Methods

        private void CacheEntryUpdate(CacheEntryUpdateArguments args)
        {
            var cacheItem = MemCache.GetCacheItem(args.Key);
            var cacheObj = cacheItem.Value;

            cacheItem.Value = cacheObj;
            args.UpdatedCacheItem = cacheItem;
            var policy = new CacheItemPolicy
            {
                UpdateCallback = new CacheEntryUpdateCallback(CacheEntryUpdate),
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(RefreshInterval)
            };
            args.UpdatedCacheItemPolicy = policy;
        }

        private string getName(string[] data)
        {
            return data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
        }

        private double getLatitude(string[] data)
        {
            return data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
        }

        private double getLongitude(string[] data)
        {
            return data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;
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
