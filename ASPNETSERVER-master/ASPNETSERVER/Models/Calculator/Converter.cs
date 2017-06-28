using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETSERVER.Models.Calculator
{
    public class Converter
    {
        private static string GetBand(double latitude)
        {
            if (latitude <= 84 && latitude >= 72)
                return "X";
            else if (latitude < 72 && latitude >= 64)
                return "W";
            else if (latitude < 64 && latitude >= 56)
                return "V";
            else if (latitude < 56 && latitude >= 48)
                return "U";
            else if (latitude < 48 && latitude >= 40)
                return "T";
            else if (latitude < 40 && latitude >= 32)
                return "S";
            else if (latitude < 32 && latitude >= 24)
                return "R";
            else if (latitude < 24 && latitude >= 16)
                return "Q";
            else if (latitude < 16 && latitude >= 8)
                return "P";
            else if (latitude < 8 && latitude >= 0)
                return "N";
            else if (latitude < 0 && latitude >= -8)
                return "M";
            else if (latitude < -8 && latitude >= -16)
                return "L";
            else if (latitude < -16 && latitude >= -24)
                return "K";
            else if (latitude < -24 && latitude >= -32)
                return "J";
            else if (latitude < -32 && latitude >= -40)
                return "H";
            else if (latitude < -40 && latitude >= -48)
                return "G";
            else if (latitude < -48 && latitude >= -56)
                return "F";
            else if (latitude < -56 && latitude >= -64)
                return "E";
            else if (latitude < -64 && latitude >= -72)
                return "D";
            else if (latitude < -72 && latitude >= -80)
                return "C";
            else
                return null;
        }

        private static int GetZone(double latitude, double longitude)
        {
            // Norway 
            if (latitude >= 56 && latitude < 64 && longitude >= 3 && longitude < 13)
                return 32;

            // Spitsbergen 
            if (latitude >= 72 && latitude < 84)
            {
                if (longitude >= 0 && longitude < 9)
                    return 31;
                else if (longitude >= 9 && longitude < 21)
                    return 33;
                if (longitude >= 21 && longitude < 33)
                    return 35;
                if (longitude >= 33 && longitude < 42)
                    return 37;
            }

            return (int)Math.Ceiling((longitude + 180) / 6);
        }

        public static string ConvertToUtmString(double latitude, double longitude)
        {
            if (latitude < -80 || latitude > 84)
                return null;

            int zone = GetZone(latitude, longitude);
            string band = GetBand(latitude);

            //Transform to UTM 
            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateSystem wgs84geo = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
            ICoordinateSystem utm = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WGS84_UTM(zone, latitude > 0);
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84geo, utm);
            double[] pUtm = trans.MathTransform.Transform(new double[] { longitude, latitude });

            double easting = pUtm[0];
            double northing = pUtm[1];

            return String.Format("{0}{1} {2:0} {3:0}", zone, band, easting, northing);
        }
    }
}