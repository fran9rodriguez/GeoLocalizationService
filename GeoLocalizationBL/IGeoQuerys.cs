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
        List<Location> GetLocations(Location pLocation, int maxDistance, int maxResults);
    }
}
