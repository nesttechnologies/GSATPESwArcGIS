using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETSERVER.Models.Calculator
{
    //Converting Longtitude and Latitude into UTM
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


/////////////////////////////////////////////////////////////////////////////////////////


        // Generating UTM Grid 
        public static int[,] Calcgrid(double lat, double longt)
        {
            string c = Converter.ConvertToUtmString(lat, longt);
            string[] sub = c.Split(' ');
            string ZoneBand = (sub[0]);
            int easting = (int)Convert.ToInt32(sub[1]) - (int)Convert.ToInt32(sub[1]) % 100;
            int northing = (int)Convert.ToInt32((sub[2])) - (int)Convert.ToInt32((sub[2])) % 100;


            if ((int)Convert.ToInt32(sub[1]) % 100 > 50 && (int)Convert.ToInt32((sub[2])) % 100 > 50)
            {
                easting += 100;
                northing += 100;
            }
            else if ((int)Convert.ToInt32(sub[1]) % 100 > 50 && (int)Convert.ToInt32((sub[2])) % 100 < 50)
            {
                easting += 100;

            }
            else if ((int)Convert.ToInt32(sub[1]) % 100 < 50 && (int)Convert.ToInt32((sub[2])) % 100 > 50)
            {

                northing += 100;
            }



            int iRows = 10000;
            int iCols = 4;

            int[,] dataG = new int[iRows, iCols];
            int i, j, temp;

            Random rnd = new Random();

            for (i = 0; i < iRows; i++)
            {
                //test-arr.insert(index, item)

                if (i % 100 > 0)
                {
                    for (j = 0; j < iCols; j++)
                    {

                        switch (j)
                        {
                            case 0:
                                dataG[i, j] = (int)(dataG[i - 1, 0]);
                                break;
                            case 1:
                                dataG[i, j] = (int)(dataG[i - 1, 1] + 100);
                                break;
                            case 2:
                                dataG[i, j] = (int)rnd.Next(1, 1000); //(int)rnd.Next
                                break;
                            case 3:
                                dataG[i, j] = (int)rnd.Next(1, 100);
                                break;
                        }
                    }
                }
                else
                {
                    for (j = 0; j < iCols; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            dataG[i, j] = (int)(northing - (100 * 50));
                        }
                        else if (i == 0 && j == 1)
                        {
                            dataG[i, j] = (int)(easting - (100 * 50));
                            dataG[i, j + 1] = (int)rnd.Next(1, 1000);
                            dataG[i, j + 2] = (int)rnd.Next(1, 100);
                        }
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                    dataG[i, j] = (int)(dataG[i - 100, 0]);
                                    break;
                                case 1:
                                    dataG[i, j] = (int)(dataG[i - 100, 1] + 100);
                                    break;
                                case 2:
                                    dataG[i, j] = (int)rnd.Next(1, 1000);
                                    break;
                                case 3:
                                    dataG[i, j] = (int)rnd.Next(1, 100);
                                    break;
                            }
                        }
                    }
                }
            }
            for (int m = 0; m < iRows; m++)
            {
                for (int n = 0; n < iCols; n++)
                {
                    Console.Write(string.Format("{0} ", dataG[m, n]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            return dataG;

        }

    }
}