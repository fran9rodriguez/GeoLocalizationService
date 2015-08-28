using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GeoLocalizationCL;

namespace GeoLocalizationWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISearchService" in both code and config file together.
    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract]
        SearchResults GetLocations(string Latitude, string Longitude, string maxDistance, string maxResults);
    }
}
