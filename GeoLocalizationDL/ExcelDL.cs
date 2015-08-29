
#region References

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;
using GeoLocalizationCL;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Web;
using System.Runtime.Caching;
using System.Threading;

#endregion

namespace GeoLocalizationDL
{

    /// <summary>
    /// 
    /// </summary>
    public class ExcelDL:IDataSource
    {

        #region getAllLocationsFileReaderLinq

        /// <summary>
        /// Method to get all locations from a .csv usinf FileReader and Linq
        /// </summary>
        /// <returns>
        ///     A list of Locations
        /// </returns>
        public List<Location> getAllLocationsFileReaderLinq(string filename)
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
            catch(Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region getAllLocationsStream

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<Location> getAllLocationsStream(string filename)
        {
            try
            {
                List<Location> lLocations = new List<Location>();
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string Name = getName(data);
                        double Latitude = getLatitude(data);
                        double Longitude = getLongitude(data);
                        Location loc = new Location(Name, Latitude, Longitude);
                        lLocations.Add(loc);

                    }
                    sr.Close();
                }

                return lLocations;
            }
            catch
            {
                throw;
            }

        }

        #endregion

        #region getAllLocationsStreamParallel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<Location> getAllLocationsStreamParallel(string filename)
        {
            try
            {
                List<Location> lLocations = new List<Location>();
                string[] AllLines = File.ReadAllLines(filename);

                var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 10 };

                Parallel.For(0, AllLines.Length, options, i =>
                {
                    try
                    {

                        string[] data = AllLines[i].Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string Name = getName(data);
                        double Latitude = getLatitude(data);
                        double Longitude = getLongitude(data);
                        Location loc = new Location(Name, Latitude, Longitude);

                        lock (lLocations)
                        {
                            lLocations.Add(loc);
                        }

                    }
                    catch { }
                });

                return lLocations;
            }
            catch
            {
                throw;
            }

        }

        #endregion

        #region GetLocationsLinqToExcel

        public List<Location> GetLocationsLinqToExcel()
        {
            string pathToExcelFile = ConfigurationManager.AppSettings["fileLocations"];

            string sheetName = "locations";

            var excelFile = new ExcelQueryFactory(pathToExcelFile);
            //excelFile.AddMapping("Name", "Address");
            //excelFile.AddMapping("Latitude", "Latitude");
            //excelFile.AddMapping("Longitude", "Longitude");

            var qLocations = from location in excelFile.Worksheet(sheetName)                             
                             select new Location
                             {
                                 Name = location["Address"],
                                 Latitude = double.Parse(location["Latitude"]),
                                 Longitude = double.Parse(location["Longitude"])
                             };

            return qLocations.ToList();

            
        }

        #endregion

        #region GetLocationsOldbConnection

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataView GetLocationsOldbConnection()
        {

            string pathToExcelFile = ConfigurationManager.AppSettings["fileLocations"];
            string sheetName = "locations";

            OleDbConnection con = new OleDbConnection(
                    "provider=Microsoft.Jet.OLEDB.4.0;data source="
                    + pathToExcelFile
                    + ";Extended Properties=Excel 8.0;");

            StringBuilder stbQuery = new StringBuilder();
            stbQuery.Append("SELECT * FROM [" + sheetName + "$A1:C1]");
            OleDbDataAdapter adp = new OleDbDataAdapter(stbQuery.ToString(), con);

            DataSet dsXLS = new DataSet();
            adp.Fill(dsXLS);

            DataView dvEmp = new DataView(dsXLS.Tables[0]);

            return dvEmp;
        }

        #endregion

        #region geLocationsByRange

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<List<Location>> geLocationsByRange(string filename, int index, int offset)
        {
            try
            {
                List<Location> lLocations = new List<Location>();
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    int lineNbr = 0;
                    int cont = 0;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        lineNbr++;
                        if (lineNbr >= index)
                        {
                            string[] data = line.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                            string Name = getName(data);
                            double Latitude = getLatitude(data);
                            double Longitude = getLongitude(data);
                            Location loc = new Location(Name, Latitude, Longitude);
                            lLocations.Add(loc);
                            cont++;
                        }

                        if (cont == offset) break;
                    }
                    sr.Close();
                }

                return lLocations;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Private Methods

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
