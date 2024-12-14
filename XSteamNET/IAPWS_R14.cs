using System;

namespace XSteamNET
{
    public static class IAPWS_R14
    {
        public const int __TYPE_ICE_Ih__ = 1;
        public const int __TYPE_ICE_III__ = 3;
        public const int __TYPE_ICE_V__ = 5;
        public const int __TYPE_ICE_VI__ = 6;
        public const int __TYPE_ICE_VII__ = 7;

        public static double pmelt_T_iceIh(double T)
        {
            // EQ 1 / Melting pressure of ice Ih
            double T_star = 273.16;
            double p_star = 611.657e-6;
            double theta = T / T_star;

            double[] a = { 0.119539337e7, 0.808183159e5, 0.333826860e4 };
            double[] b = { 0.300000e1, 0.257500e2, 0.103750e3 };

            double temp_sum = 0;
            for (int i = 0; i < 3; i++)
            {
                temp_sum += a[i] * (1 - Math.Pow(theta, b[i]));
            }

            double pi_melt = 1 + temp_sum;
            return pi_melt * p_star;
        }

        public static double pmelt_T_iceIII(double T)
        {
            double T_star = 251.165;
            double p_star = 208.566;
            double theta = T / T_star;
            double pi_melt = 1 - 0.299948 * (1.0 - Math.Pow(theta, 60));
            return pi_melt * p_star;
        }

        public static double pmelt_T_iceV(double T)
        {
            double T_star = 256.164;
            double p_star = 350.1;
            double theta = T / T_star;
            double pi_melt = 1 - 1.18721 * (1.0 - Math.Pow(theta, 8));
            return pi_melt * p_star;
        }

        public static double pmelt_T_iceVI(double T)
        {
            double T_star = 273.31;
            double p_star = 632.4;
            double theta = T / T_star;
            double pi_melt = 1 - 1.07476 * (1.0 - Math.Pow(theta, 4.6));
            return pi_melt * p_star;
        }

        public static double pmelt_T_iceVII(double T)
        {
            double T_star = 355.0;
            double p_star = 2216.0;
            double theta = T / T_star;

            double p1 = 0.173683e1 * (1 - (1 / theta));
            double p2 = 0.544606e-1 * (1 - Math.Pow(theta, 5));
            double p3 = 0.806106e-7 * (1 - Math.Pow(theta, 22));

            double pi_melt = Math.Exp(p1 - p2 + p3);
            return pi_melt * p_star;
        }

        public static double psubl_T(double T)
        {
            double T_star = 273.16;
            double p_star = 611.657e-6;
            double[] a = { -0.212144006e2, 0.273203819e2, -0.610598130e1 };
            double[] b = { 0.333333333e-2, 0.120666667e1, 0.170333333e1 };
            double theta = T / T_star;
            double temp_sum = 0;

            for (int i = 0; i < 3; i++)
            {
                temp_sum += a[i] * Math.Pow(theta, b[i]);
            }

            double pi_subl = Math.Exp((1 / theta) * temp_sum);
            return pi_subl * p_star;
        }
    }
}
