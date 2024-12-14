using System;

namespace XSteamNET
{
    public static class RegionSelection
    {
        /// <summary>
        /// Determines the region based on pressure and temperature.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="T">Temperature in K.</param>
        /// <returns>Region number as an integer.</returns>
        public static int Region_pT(double p, double T)
        {
            int region_pT_number = 0;

            if ((T > 1073.15) && (p < 50.0) && (T < 2273.15) && (p > 0.000611))
            {
                region_pT_number = 5;
            }
            else if ((T <= 1073.15) && (T > 273.15) && (p <= 100) && (p > 0.000611))
            {
                if (T > 623.15)
                {
                    if (p > RegionBorders.B23p_T(T))
                    {
                        region_pT_number = 3;
                        if (T < 647.096)
                        {
                            double ps = Region4.p4_T(T);
                            if (Math.Abs(p - ps) < 0.00001)
                            {
                                region_pT_number = 4;
                            }
                        }
                    }
                    else
                    {
                        region_pT_number = 2;
                    }
                }
                else
                {
                    double ps = Region4.p4_T(T);
                    if (Math.Abs(p - ps) < 0.00001)
                    {
                        region_pT_number = 4;
                    }
                    else if (p > ps)
                    {
                        region_pT_number = 1;
                    }
                    else
                    {
                        region_pT_number = 2;
                    }
                }
            }
            else
            {
                Console.WriteLine("Warning: Temperature outside valid area.");
                region_pT_number = 0; // Error, outside valid area
            }

            return region_pT_number;
        }

        /// <summary>
        /// Determines the region based on pressure and enthalpy.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="h">Enthalpy in kJ/kg.</param>
        /// <returns>Region number as an integer.</returns>
        public static int Region_ph(double p, double h)
        {
            // Check if outside pressure limits
            if (p < 0.000611657 || p > 100)
            {
                Console.WriteLine("Warning: Pressure outside valid area");
                return 0;
            }

            // Check if outside low enthalpy limits
            if (h < (0.963 * p + 2.2)) // Linear adaptation to speed up calculations
            {
                if (h < Region1.h1_pT(p, 273.15))
                {
                    Console.WriteLine("Warning: Enthalpy outside valid area");
                    return 0;
                }
            }

            if (p < 16.5292) // Below Region 3, check Regions 1, 4, 2, 5
            {
                double Ts = Region4.T4_p(p);

                // Check Region 1
                double hL = 109.6635 * Math.Log(p) + 40.3481 * p + 734.58; // Approximation
                if (Math.Abs(h - hL) < 100)
                {
                    hL = Region1.h1_pT(p, Ts); // More accurate function
                }
                if (h <= hL)
                {
                    return 1;
                }

                // Check Region 4
                double hV = 45.1768 * Math.Log(p) - 20.158 * p + 2804.4; // Approximation
                if (Math.Abs(h - hV) < 50)
                {
                    hV = Region2.h2_pT(p, Ts); // More accurate function
                }
                if (h < hV)
                {
                    return 4;
                }

                // Quick test for Region 2 upper limit
                if (h < 4000)
                {
                    return 2;
                }

                // Check Region 2 (Real value)
                double h_45 = Region2.h2_pT(p, 1073.15);
                if (h <= h_45)
                {
                    return 2;
                }

                // Check Region 5
                if (p > 10)
                {
                    Console.WriteLine("Warning: Pressure outside valid area");
                    return 0;
                }
                double h_5u = Region5.h5_pT(p, 2273.15);
                if (h < h_5u)
                {
                    return 5;
                }

                Console.WriteLine("Warning: Enthalpy outside valid area");
                return 0;
            }
            else // For p >= 16.5292
            {
                // Check if in Region 1
                if (h < Region1.h1_pT(p, 623.15))
                {
                    return 1;
                }

                // Check if in Region 3 or 4 (Below Region 2)
                if (h < Region2.h2_pT(p, RegionBorders.B23T_p(p)))
                {
                    if (p > RegionBorders.p3sat_h(h))
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }

                // Check if in Region 2
                if (h < Region2.h2_pT(p, 1073.15))
                {
                    return 2;
                }
            }

            Console.WriteLine("Warning: Pressure outside valid area");
            return 0;
        }

        /// <summary>
        /// Determines the region based on pressure and entropy.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="s">Entropy in kJ/(kg·K).</param>
        /// <returns>Region number as an integer.</returns>
        public static int Region_ps(double p, double s)
        {
            // Check if pressure or entropy is outside valid range
            if (p < 0.000611657 || p > 100 || s < 0 || s > Region5.s5_pT(p, 2273.15))
            {
                Console.WriteLine("Warning: Pressure or Entropy outside valid area");
                return 0;
            }

            // Check Region 5
            if (s > Region2.s2_pT(p, 1073.15))
            {
                if (p <= 10)
                {
                    return 5;
                }
                else
                {
                    Console.WriteLine("Warning: Pressure outside valid area");
                    return 0;
                }
            }

            // Check Region 2
            double ss;
            if (p > 16.529)
            {
                ss = Region2.s2_pT(p, RegionBorders.B23T_p(p)); // Between 5.047 & 5.261
            }
            else
            {
                ss = Region2.s2_pT(p, Region4.T4_p(p));
            }

            if (s > ss)
            {
                return 2;
            }

            // Check Region 3
            ss = Region1.s1_pT(p, 623.15);
            if (p > 16.529 && s > ss)
            {
                if (p > RegionBorders.p3sat_s(s))
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }

            // Check Region 4 (Not inside Region 3)
            if (p < 16.529 && s > Region1.s1_pT(p, Region4.T4_p(p)))
            {
                return 4;
            }

            // Check Region 1
            if (p > 0.000611657 && s > Region1.s1_pT(p, 273.15))
            {
                return 1;
            }

            return 1; // Default to Region 1 if no other region matches
        }


        /// <summary>
        /// Determines the region based on enthalpy and entropy.
        /// </summary>
        /// <param name="h">Enthalpy in kJ/kg.</param>
        /// <param name="s">Entropy in kJ/(kg·K).</param>
        /// <returns>Region number as an integer.</returns>
        public static int Region_hs(double h, double s)
        {
            double hMin, hMax, Tmin, TMax;

            if (s < -0.0001545495919)
            {
                Console.WriteLine("Warning: Entropy outside valid area");
                return 0;
            }

            // Check linear adaptation to p=0.000611. If below region 4
            hMin = ((-0.0415878 - 2500.89262) / (-0.00015455 - 9.155759)) * s;
            if (s < 9.155759395 && h < hMin)
            {
                Console.WriteLine("Warning: Enthalpy or Entropy outside valid area");
                return 0;
            }

            // Region 1, 4, or a small part of B13
            if (s >= -0.0001545495919 && s <= 3.77828134)
            {
                if (h < Region4.h4_s(s))
                {
                    return 4;
                }
                else if (s < 3.397782955)
                {
                    TMax = Region1.T1_ps(100, s);
                    hMax = Region1.h1_pT(100, TMax);
                    if (h < hMax)
                    {
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("Warning: Enthalpy outside valid area");
                        return 0;
                    }
                }
                else
                {
                    double hB = RegionBorders.hB13_s(s);
                    if (h < hB)
                    {
                        return 1;
                    }
                    TMax = Region3.T3_ps(100, s);
                    double vmax = Region3.v3_ps(100, s);
                    hMax = Region3.h3_rhoT(1 / vmax, TMax);
                    if (h < hMax)
                    {
                        return 3;
                    }
                    else
                    {
                        Console.WriteLine("Warning: Enthalpy outside valid area");
                        return 0;
                    }
                }
            }

            // Region 2 or 4 (upper part of B23 to max)
            if (s >= 5.260578707 && s <= 11.9212156897728)
            {
                if (s > 9.155759395)
                {
                    Tmin = Region2.T2_ps(0.000611, s);
                    hMin = Region2.h2_pT(0.000611, Tmin);
                    hMax = -0.07554022 * Math.Pow(s, 4) + 3.341571 * Math.Pow(s, 3) - 55.42151 * Math.Pow(s, 2) + 408.515 * s + 3031.338;

                    if (h > hMin && h < hMax)
                    {
                        return 2;
                    }
                    else
                    {
                        Console.WriteLine("Warning: Enthalpy outside valid area");
                        return 0;
                    }
                }

                double hV = Region4.h4_s(s);
                if (h < hV)
                {
                    return 4;
                }

                if (s < 6.04048367171238)
                {
                    TMax = Region2.T2_ps(100, s);
                    hMax = Region2.h2_pT(100, TMax);
                }
                else
                {
                    hMax = -2.988734 * Math.Pow(s, 4) + 121.4015 * Math.Pow(s, 3) - 1805.15 * Math.Pow(s, 2) + 11720.16 * s - 23998.33;
                }

                if (h < hMax)
                {
                    return 2;
                }
                else
                {
                    Console.WriteLine("Warning: Enthalpy outside valid area");
                    return 0;
                }
            }

            // Region 3 or 4 under critical point
            if (s >= 3.77828134 && s <= 4.41202148223476)
            {
                double hL = Region4.h4_s(s);
                if (h < hL)
                {
                    return 4;
                }
                TMax = Region3.T3_ps(100, s);
                double vmax = Region3.v3_ps(100, s);
                hMax = Region3.h3_rhoT(1 / vmax, TMax);
                if (h < hMax)
                {
                    return 3;
                }
                else
                {
                    Console.WriteLine("Warning: Enthalpy outside valid area");
                    return 0;
                }
            }

            // Region 3 or 4 from critical point to upper part of B23
            if (s >= 4.41202148223476 && s <= 5.260578707)
            {
                double hV = Region4.h4_s(s);
                if (h < hV)
                {
                    return 4;
                }
                if (s <= 5.048096828)
                {
                    TMax = Region3.T3_ps(100, s);
                    double vmax = Region3.v3_ps(100, s);
                    hMax = Region3.h3_rhoT(1 / vmax, TMax);
                    if (h < hMax)
                    {
                        return 3;
                    }
                    else
                    {
                        Console.WriteLine("Warning: Enthalpy outside valid area");
                        return 0;
                    }
                }
                else
                {
                    if (h > 2812.942061)
                    {
                        if (s > 5.09796573397125)
                        {
                            TMax = Region2.T2_ps(100, s);
                            hMax = Region2.h2_pT(100, TMax);
                            if (h < hMax)
                            {
                                return 2;
                            }
                            else
                            {
                                Console.WriteLine("Warning: Enthalpy outside valid area");
                                return 0;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Warning: Entropy outside valid area");
                            return 0;
                        }
                    }
                    if (h < 2563.592004)
                    {
                        return 3;
                    }
                    double Tact = RegionBorders.TB23_hs(h, s);
                    double pact = Region2.p2_hs(h, s);
                    double pBound = RegionBorders.B23p_T(Tact);
                    if (pact > pBound)
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }

            Console.WriteLine("Warning: Entropy and Enthalpy outside valid area");
            return 0;
        }

        /// <summary>
        /// Determines the region based on pressure and density.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="rho">Density in kg/m³.</param>
        /// <returns>Region number as an integer.</returns>
        public static int Region_prho(double p, double rho)
        {
            double v = 1 / rho;

            if (p < 0.000611657 || p > 100)
            {
                Console.WriteLine("Warning: Pressure outside valid area");
                return 0;
            }

            if (p < 16.5292) // Below region 3, check regions 1, 4, 2
            {
                if (v < Region1.v1_pT(p, 273.15)) // Note: This is not actually the minimum of v. Not valid as water at 4°C is lighter.
                {
                    Console.WriteLine("Warning: Specific volume outside valid area");
                    return 0;
                }

                if (v <= Region1.v1_pT(p, Region4.T4_p(p)))
                {
                    return 1;
                }

                if (v < Region2.v2_pT(p, Region4.T4_p(p)))
                {
                    return 4;
                }

                if (v <= Region2.v2_pT(p, 1073.15))
                {
                    return 2;
                }

                if (p > 10) // Above region 5
                {
                    Console.WriteLine("Warning: Pressure outside valid area");
                    return 0;
                }

                if (v <= Region5.v5_pT(p, 2073.15))
                {
                    return 5;
                }
            }
            else // Check regions 1, 3, 4, 3, 2 (Above the lowest point of region 3)
            {
                if (v < Region1.v1_pT(p, 273.15)) // Note: This is not actually the minimum of v. Not valid as water at 4°C is lighter.
                {
                    Console.WriteLine("Warning: Specific volume outside valid area");
                    return 0;
                }

                if (v < Region1.v1_pT(p, 623.15))
                {
                    return 1;
                }

                // Check if in region 3 or 4 (below region 2)
                if (v < Region2.v2_pT(p, RegionBorders.B23T_p(p)))
                {
                    // Region 3 or 4
                    if (p > 22.064) // Above region 4
                    {
                        return 3;
                    }

                    if (v < Region3.v3_ph(p, Region4.h4L_p(p)) || v > Region3.v3_ph(p, Region4.h4V_p(p)))
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }

                // Check if region 2
                if (v < Region2.v2_pT(p, 1073.15))
                {
                    return 2;
                }
            }

            Console.WriteLine("Warning: Pressure and Density outside valid area");
            return 0;
        }
    }
}