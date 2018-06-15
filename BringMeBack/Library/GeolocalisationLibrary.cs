using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace BringMeBack.Library
{
    class GeolocalisationLibrary
    {
        public GeolocalisationLibrary() { }

        public async Task<Geopoint> Located()
        {

            return (await new Geolocator().GetGeopositionAsync()).Coordinate.Point;


        }
        
    }
}
