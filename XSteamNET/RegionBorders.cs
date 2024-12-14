using System;

namespace XSteamNET
{
    /// <summary>
    /// Static class to define region border equations and auxiliary equations as per IAPWS Industrial formulation.
    /// </summary>
    public static class RegionBorders
    {
        /// <summary>
        /// Boundary between region 2 and 3. 
        /// Equation 5, Page 5.
        /// </summary>
        public static double B23p_T(double T)
        {
            return 348.05185628969 - 1.1671859879975 * T + 1.0192970039326e-03 * Math.Pow(T, 2);
        }

        /// <summary>
        /// Boundary between region 2 and 3.
        /// Equation 6, Page 6.
        /// </summary>
        public static double B23T_p(double p)
        {
            return 572.54459862746 + Math.Sqrt((p - 13.91883977887) / 1.0192970039326e-03);
        }

        /// <summary>
        /// Saturation pressure as a function of specific enthalpy for region 3.
        /// Equation 10, Page 17.
        /// </summary>
        public static double p3sat_h(double h)
        {
            int[] Ii = { 0, 1, 1, 1, 1, 5, 7, 8, 14, 20, 22, 24, 28, 36 };
            int[] Ji = { 0, 1, 3, 4, 36, 3, 0, 24, 16, 16, 3, 18, 8, 24 };
            double[] ni = {
                0.600073641753024,
                -9.36203654849857,
                24.6590798594147,
                -107.014222858224,
                -91582131580576.8,
                -8623.32011700662,
                -23.5837344740032,
                2.52304969384128e17,
                -3.89718771997719e18,
                -3.33775713645296e22,
                35649946963.6328,
                -1.48547544720641e26,
                3.30611514838798e18,
                8.13641294467829e37
            };

            double hs = h / 2600.0;
            double ps = 0.0;

            for (int i = 0; i < Ii.Length; i++)
            {
                ps += ni[i] * Math.Pow(hs - 1.02, Ii[i]) * Math.Pow(hs - 0.608, Ji[i]);
            }

            return ps * 22.0;
        }

        /// <summary>
        /// Saturation pressure as a function of specific entropy for region 3.
        /// </summary>
        public static double p3sat_s(double s)
        {
            int[] Ii = { 0, 1, 1, 4, 12, 12, 16, 24, 28, 32 };
            int[] Ji = { 0, 1, 32, 7, 4, 14, 36, 10, 0, 18 };
            double[] ni = {
                0.639767553612785,
                -12.9727445396014,
                -2.24595125848403e15,
                1774667.41801846,
                7170793495.71538,
                -3.78829107169011e17,
                -9.55586736431328e34,
                1.87269814676188e23,
                119254746466.473,
                1.10649277244882e36
            };

            double sigma = s / 5.2;
            double pi = 0.0;

            for (int i = 0; i < Ii.Length; i++)
            {
                pi += ni[i] * Math.Pow(sigma - 1.03, Ii[i]) * Math.Pow(sigma - 0.699, Ji[i]);
            }

            return pi * 22.0;
        }

        /// <summary>
        /// Specific enthalpy as a function of specific entropy for the boundary between regions 1 and 3.
        /// </summary>
        public static double hB13_s(double s)
        {
            int[] Ii = { 0, 1, 1, 3, 5, 6 };
            int[] Ji = { 0, -2, 2, -12, -4, -3 };
            double[] ni = {
                0.913965547600543,
                -4.30944856041991e-05,
                60.3235694765419,
                1.17518273082168e-18,
                0.220000904781292,
                -69.0815545851641
            };

            double sigma = s / 3.8;
            double eta = 0.0;

            for (int i = 0; i < Ii.Length; i++)
            {
                eta += ni[i] * Math.Pow(sigma - 0.884, Ii[i]) * Math.Pow(sigma - 0.864, Ji[i]);
            }

            return eta * 1700.0;
        }

        /// <summary>
        /// Temperature as a function of specific enthalpy and specific entropy for the boundary between regions 1 and 3.
        /// </summary>
        public static double TB23_hs(double h, double s)
        {
            int[] Ii = { -12, -10, -8, -4, -3, -2, -2, -2, -2, 0, 1, 1, 1, 3, 3, 5, 6, 6, 8, 8, 8, 12, 12, 14, 14 };
            int[] Ji = { 10, 8, 3, 4, 3, -6, 2, 3, 4, 0, -3, -2, 10, -2, -1, -5, -6, -3, -8, -2, -1, -12, -1, -12, 1 };
            double[] ni = {
                6.2909626082981e-04,
                -8.23453502583165e-04,
                5.15446951519474e-08,
                -1.17565945784945,
                3.48519684726192,
                -5.07837382408313e-12,
                -2.84637670005479,
                -2.36092263939673,
                6.01492324973779,
                1.48039650824546,
                3.60075182221907e-04,
                -1.26700045009952e-02,
                -1221843.32521413,
                0.149276502463272,
                0.698733471798484,
                -2.52207040114321e-02,
                1.47151930985213e-02,
                -1.08618917681849,
                -9.36875039816322e-04,
                81.9877897570217,
                -182.041861521835,
                2.61907376402688e-06,
                -29162.6417025961,
                1.40660774926165e-05,
                7832370.62349385
            };

            double sigma = s / 5.3;
            double eta = h / 3000.0;
            double teta = 0.0;

            for (int i = 0; i < Ii.Length; i++)
            {
                teta += ni[i] * Math.Pow(eta - 0.727, Ii[i]) * Math.Pow(sigma - 0.864, Ji[i]);
            }

            return teta * 900.0;
        }
    }
}
