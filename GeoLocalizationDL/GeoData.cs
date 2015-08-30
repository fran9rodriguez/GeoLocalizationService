
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
using System.Threading;

#endregion

namespace GeoLocalizationDL
{

    /// <summary>
    /// This class implement different methods to get the Locations from a Data Source. I've implemented 3 different approaches in order to
    /// find out the best performance. After several test the best one is getAllLocationsCacheStreamParallel which read the file source using
    /// StreamReader and Parallel Loop + Memori cache. Withe the Cache use the 1st query is a bit more late but the following querys after the 1st 
    /// one give me a performance about 1seg for files of 1.000.000 of records.
    /// </summary>
    public class GeoData : IDisposable
    {
        static MemoryCache MemCache;
        static int RefreshInterval = Int32.Parse(ConfigurationManager.AppSettings["refreshIntervalCache"].ToString());

        #region getAllLocations

        /// <summary>
        /// General Method to get all the locations. I've created this method to isolate the BL and the DL.
        /// After several test the best method has been getAllLocationsCacheStreamParallel
        /// </summary>
        /// <returns></returns>
        public List<Location> getAllLocations()
        {
            try
            {                
                List<Location> lLocations = new List<Location>();
                lLocations = getAllLocationsCacheStreamParallel();

                return lLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region getAllLocationsCacheStreamParallel

        /// <summary>
        /// Method to get all locations using the Memory Cache and reading the source of data using Stream Object and ParallelFor.        
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        /// <remarks>
        ///     After testing this is the best option
        /// </remarks>
        private List<Location> getAllLocationsCacheStreamParallel()
        {
            try
            {
                List<Location> lLocations = new List<Location>();

                //Check if the list of Locations is already in Cache
                if (MemCache == null) MemCache = new MemoryCache("MemCache");
                
                //If not makes the search
                if (!MemCache.Contains("ListOfLocations"))
                {
                    
                    string dataSource = ConfigurationManager.AppSettings["dataSource"];                   

                    switch (dataSource)
                    {
                        case "CSV":
                            string fileName = ConfigurationManager.AppSettings["fileLocations"];
                            using (ExcelDL edl = new ExcelDL())
                            {
                                lLocations = edl.getAllLocationsStreamParallel(fileName);
                            }
                            
                            break;
                        default:
                            break;
                    }

                    //Add to the Cache
                    CacheItemPolicy policy = getacheItemPolicy();                   
                    MemCache.Set("ListOfLocations", lLocations, policy);
                }
                else lLocations = (List<Location>)MemCache["ListOfLocations"];

                return lLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region getAllLocationsCacheFileReader

        /// <summary>
        /// Method to get all locations 
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        private List<Location> getAllLocationsCacheFileReader()
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

                            using (ExcelDL edl = new ExcelDL())
                            {
                                lLocations = edl.getAllLocationsFileReaderLinq(fileName);
                            }                           
                            
                            break;
                        default:
                            break;
                    }

                    //Add to the cache
                    CacheItemPolicy policy = getacheItemPolicy();
                    MemCache.Set("ListOfLocations", lLocations, policy);
                }
                else lLocations = (List<Location>)MemCache["ListOfLocations"];

                return lLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region getAllLocationsNonCacheStreamParallel

        /// <summary>
        /// Method to get all locations 
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        private List<Location> getAllLocationsNonCacheStreamParallel()
        {
            try
            {
                List<Location> lLocations = new List<Location>();
                string dataSource = ConfigurationManager.AppSettings["dataSource"];
                switch (dataSource)
                {
                    case "CSV":
                        string fileName = ConfigurationManager.AppSettings["fileLocations"];
                        using (ExcelDL edl = new ExcelDL())
                        {
                            lLocations = edl.getAllLocationsStreamParallel(fileName);
                        }
                        break;
                    default:
                        break;
                }
                return lLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region getAllLocationsByRange Async

        /// <summary>
        /// Method to get all locations 
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        public async Task<List<Location>> getAllLocationsByRange(int index, int offset)
        {
            try
            {

                List<Location> lLocations = new List<Location>();
                string dataSource = ConfigurationManager.AppSettings["dataSource"];
                switch (dataSource)
                {
                    case "CSV":
                        string fileName = ConfigurationManager.AppSettings["fileLocations"];
                        using (ExcelDL edl = new ExcelDL())
                        {
                            lLocations = await edl.geLocationsByRange(fileName, index, offset);
                        }
                        break;
                    default:
                        break;
                }

                return lLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        #endregion        

        #region Cache Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private CacheItemPolicy getacheItemPolicy()
        {
            CacheItemPolicy policy = new CacheItemPolicy
                {
                    UpdateCallback = new CacheEntryUpdateCallback(CacheEntryUpdate),
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(RefreshInterval)
                };

            return policy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
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
