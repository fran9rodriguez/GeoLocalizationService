using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLocalizationCL;

namespace GeoLocalizationBL
{
    public interface IGeoQuerys: IDisposable
    {
        SearchResults GetLocations(Location pLocation, int maxDistance, int maxResults, SearchResults sr);
    }
}
