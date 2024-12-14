using System;

namespace XSteamNET
{
    public static class TransportProperties
    {
        /// <summary>
        /// Calculates viscosity for all regions based on pressure and temperature.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="T">Temperature in K.</param>
        /// <returns>Viscosity in Pa·s.</returns>
        public static double my_AllRegions_pT(double p, double T)
        {
            double[] h0 = { 0.5132047, 0.3205656, 0, 0, -0.7782567, 0.1885447 };
            double[] h1 = { 0.2151778, 0.7317883, 1.241044, 1.476783, 0, 0 };
            double[] h2 = { -0.2818107, -1.070786, -1.263184, 0, 0, 0 };
            double[] h3 = { 0.1778064, 0.460504, 0.2340379, -0.4924179, 0, 0 };
            double[] h4 = { -0.0417661, 0, 0, 0.1600435, 0, 0 };
            double[] h5 = { 0, -0.01578386, 0, 0, 0, 0 };
            double[] h6 = { 0, 0, 0, -0.003629481, 0, 0 };

            // Determine the region and calculate density.
            double rho;
            int region = RegionSelection.Region_pT(p, T);
            switch (region)
            {
                case 1:
                    rho = 1 / Region1.v1_pT(p, T);
                    break;
                case 2:
                    rho = 1 / Region2.v2_pT(p, T);
                    break;
                case 3:
                    double hs = Region3.h3_pT(p, T);
                    rho = 1 / Region3.v3_ph(p, hs);
                    break;
                case 4:
                    Console.WriteLine("Warning: Function my_AllRegions_pT is not available in region 4");
                    return double.NaN;
                case 5:
                    rho = 1 / Region5.v5_pT(p, T);
                    break;
                default:
                    Console.WriteLine("Warning: Region switch returned unknown value");
                    return double.NaN;
            }

            double rhos = rho / 317.763;
            double Ts = T / 647.226;

            // Check valid area
            if ((T > (900 + 273.15)) ||
                ((T > (600 + 273.15)) && (p > 300)) ||
                ((T > (150 + 273.15)) && (p > 350)) ||
                (p > 500))
            {
                Console.WriteLine("Warning: Temperature and/or Pressure out of range of validity");
                return double.NaN;
            }

            double my0 = Math.Sqrt(Ts) / (1 + 0.978197 / Ts + 0.579829 / (Ts * Ts) - 0.202354 / (Ts * Ts * Ts));
            double tempSum = 0;

            for (int i = 0; i < 6; i++)
            {
                tempSum += h0[i] * Math.Pow((1 / Ts) - 1, i) +
                           h1[i] * Math.Pow((1 / Ts) - 1, i) * (rhos - 1) +
                           h2[i] * Math.Pow((1 / Ts) - 1, i) * Math.Pow(rhos - 1, 2) +
                           h3[i] * Math.Pow((1 / Ts) - 1, i) * Math.Pow(rhos - 1, 3) +
                           h4[i] * Math.Pow((1 / Ts) - 1, i) * Math.Pow(rhos - 1, 4) +
                           h5[i] * Math.Pow((1 / Ts) - 1, i) * Math.Pow(rhos - 1, 5) +
                           h6[i] * Math.Pow((1 / Ts) - 1, i) * Math.Pow(rhos - 1, 6);
            }

            double my1 = Math.Exp(rhos * tempSum);
            double mys = my0 * my1;

            return mys * 0.000055071;
        }


        /// <summary>
        /// Calculates viscosity for all regions based on pressure and enthalpy.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="h">Enthalpy in kJ/kg.</param>
        /// <returns>Viscosity in Pa·s.</returns>
        public static double my_AllRegions_ph(double p, double h)
        {
            double[] h0 = { 0.5132047, 0.3205656, 0, 0, -0.7782567, 0.1885447 };
            double[] h1 = { 0.2151778, 0.7317883, 1.241044, 1.476783, 0, 0 };
            double[] h2 = { -0.2818107, -1.070786, -1.263184, 0, 0, 0 };
            double[] h3 = { 0.1778064, 0.460504, 0.2340379, -0.4924179, 0, 0 };
            double[] h4 = { -0.0417661, 0, 0, 0.1600435, 0, 0 };
            double[] h5 = { 0, -0.01578386, 0, 0, 0, 0 };
            double[] h6 = { 0, 0, 0, -0.003629481, 0, 0 };

            double rho, T;

            // Determine region and calculate density and temperature.
            switch (RegionSelection.Region_ph(p, h))
            {
                case 1:
                    double Ts1 = Region1.T1_ph(p, h);
                    T = Ts1;
                    rho = 1 / Region1.v1_pT(p, Ts1);
                    break;
                case 2:
                    double Ts2 = Region2.T2_ph(p, h);
                    T = Ts2;
                    rho = 1 / Region2.v2_pT(p, Ts2);
                    break;
                case 3:
                    rho = 1 / Region3.v3_ph(p, h);
                    T = Region3.T3_ph(p, h);
                    break;
                case 4:
                    double xs = Region4.x4_ph(p, h);
                    if (p < 16.529)
                    {
                        double v4v = Region2.v2_pT(p, Region4.T4_p(p));
                        double v4L = Region1.v1_pT(p, Region4.T4_p(p));
                        rho = 1 / (xs * v4v + (1 - xs) * v4L);
                    }
                    else
                    {
                        double v4v = Region3.v3_ph(p, Region4.h4V_p(p));
                        double v4L = Region3.v3_ph(p, Region4.h4L_p(p));
                        rho = 1 / (xs * v4v + (1 - xs) * v4L);
                    }
                    T = Region4.T4_p(p);
                    break;
                case 5:
                    double Ts5 = Region5.T5_ph(p, h);
                    T = Ts5;
                    rho = 1 / Region5.v5_pT(p, Ts5);
                    break;
                default:
                    Console.WriteLine("Warning: Region switch returned unknown value");
                    return double.NaN;
            }

            double rhos = rho / 317.763;
            double Ts = T / 647.226;

            // Check valid area
            if ((T > (900 + 273.15)) ||
                (T > (600 + 273.15) && (p > 300)) ||
                (T > (150 + 273.15) && (p > 350)) ||
                (p > 500))
            {
                Console.WriteLine("Warning: Temperature and/or Pressure out of range of validity");
                return double.NaN;
            }

            double my0 = Math.Sqrt(Ts) / (1 + 0.978197 / Ts + 0.579829 / (Ts * Ts) - 0.202354 / (Ts * Ts * Ts));
            double temp_sum = 0;

            for (int i = 0; i < 6; i++)
            {
                temp_sum += h0[i] * Math.Pow(1 / Ts - 1, i)
                         + h1[i] * Math.Pow(1 / Ts - 1, i) * Math.Pow(rhos - 1, 1)
                         + h2[i] * Math.Pow(1 / Ts - 1, i) * Math.Pow(rhos - 1, 2)
                         + h3[i] * Math.Pow(1 / Ts - 1, i) * Math.Pow(rhos - 1, 3)
                         + h4[i] * Math.Pow(1 / Ts - 1, i) * Math.Pow(rhos - 1, 4)
                         + h5[i] * Math.Pow(1 / Ts - 1, i) * Math.Pow(rhos - 1, 5)
                         + h6[i] * Math.Pow(1 / Ts - 1, i) * Math.Pow(rhos - 1, 6);
            }

            double my1 = Math.Exp(rhos * temp_sum);
            double mys = my0 * my1;

            return mys * 0.000055071;
        }


        /// <summary>
        /// Calculates thermal conductivity based on pressure, temperature, and density.
        /// </summary>
        /// <param name="p">Pressure in MPa.</param>
        /// <param name="T">Temperature in K.</param>
        /// <param name="rho">Density in kg/m³.</param>
        /// <returns>Thermal conductivity in W/(m·K).</returns>
        public static double tc_ptrho(double p, double T, double rho)
        {
            // Check validity of temperature and pressure ranges
            if (T < 273.15)
            {
                Console.WriteLine("Warning: Temperature out of range of validity");
                return double.NaN;
            }
            else if (T < 500 + 273.15 && p > 100)
            {
                Console.WriteLine("Warning: Pressure out of range of validity");
                return double.NaN;
            }
            else if (T <= 650 + 273.15 && p > 70)
            {
                Console.WriteLine("Warning: Pressure out of range of validity");
                return double.NaN;
            }
            else if (T <= 800 + 273.15 && p > 40)
            {
                Console.WriteLine("Warning: Pressure out of range of validity");
                return double.NaN;
            }

            // Normalize temperature and density
            double T_norm = T / 647.26; // Page 8, Eq 4
            double rho_norm = rho / 317.7; // Page 8, Eq 5

            // Base thermal conductivity (tc0)
            double tc0 = Math.Sqrt(T_norm) *
                (0.0102811 + 0.0299621 * T_norm + 0.0156146 * Math.Pow(T_norm, 2) - 0.00422464 * Math.Pow(T_norm, 3)); // Page 9, Eq 9

            // Density-dependent thermal conductivity correction (tc1)
            double tc1 = -0.397070 +
                         0.400302 * rho_norm +
                         1.06 * Math.Exp(-0.171587 * Math.Pow(rho_norm + 2.392190, 2)); // Page 9, Eq 10

            // Additional corrections (tc2)
            double dT = Math.Abs(T_norm - 1) + 0.00308976; // Page 9, Eq 12
            double Q = 2 + 0.0822994 / Math.Pow(dT, 3.0 / 5.0); // Page 10, Eq 13

            double s;
            if (T_norm >= 1) // Page 10, Eq 14
            {
                s = 1 / dT;
            }
            else
            {
                s = 10.0932 / Math.Pow(dT, 3.0 / 5.0);
            }

            double tc2 = (
                (0.0701309 / Math.Pow(T_norm, 10) + 0.0118520) *
                Math.Pow(rho_norm, 9.0 / 5.0) *
                Math.Exp(0.642857 * (1 - Math.Pow(rho_norm, 14.0 / 5.0))) +
                0.00169937 * s * Math.Pow(rho_norm, Q) * Math.Exp((Q / (1 + Q)) * (1 - Math.Pow(rho_norm, 1 + Q))) -
                1.02 * Math.Exp(-4.11717 * Math.Pow(T_norm, 3.0 / 2.0) - 6.17937 / Math.Pow(rho_norm, 5))
            ); // Page 9, Eq 11

            return tc0 + tc1 + tc2; // Page 9, Eq 8
        }


        /// <summary>
        /// Calculates surface tension based on temperature.
        /// </summary>
        /// <param name="T">Temperature in Kelvin.</param>
        /// <returns>Surface tension in N/m.</returns>
        public static double Surface_Tension_T(double T)
        {
            double tc = Constants.CRITICAL_TEMPERATURE; // Critical temperature
            double B = 0.2358; // Coefficient B in N/m
            double bb = -0.625; // Coefficient bb
            double my = 1.256; // Coefficient my

            // Validate temperature range
            if (T < 0.01 || T > tc)
            {
                Console.WriteLine("Warning: Temperature out of range of validity");
                return double.NaN;
            }

            double tau = 1 - T / tc; // Reduced temperature
            return B * Math.Pow(tau, my) * (1 + bb * tau);
        }
    }
}