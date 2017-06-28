﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsoleApplication11;
using System.Text.RegularExpressions;
using ASPNETSERVER.Models.Calculator;
using ASPNETSERVER.Controllers;

namespace ConsoleApplication11
{
    public class UTMGrid
    {   // 

        public static void Calcgrid(decimal lat, decimal longt)
        {
            string c = Converter.ConvertToUtmString(38, -77);
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


            Console.WriteLine(ZoneBand + " " + easting + " " + northing);
            Console.ReadKey();


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
            /*    for (int m = 0; m < iRows; m++)
                {
                    for (int n = 0; n < iCols; n++)
                    {
                        Console.Write(string.Format("{0} ", dataG[m, n]));
                    }
                    Console.Write(Environment.NewLine + Environment.NewLine);
                }

                Console.ReadKey();
                */

        }


    }
}