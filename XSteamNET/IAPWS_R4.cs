using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace XSteamNET
{
    public static class IAPWS_R4
    {
        public static double MyHW_RhoT_R4(double rho, double T)
        {
            Console.WriteLine("myHW_rhoT_R4 input: ρ {0} kg/m^3, T {1} K", rho, T);

            double T_star = 643.847; // K
            double rho_star = 358; // kg / m^3
            double my_star = 55.2651; // µ Pa s

            double T_dash = T / T_star;
            double rho_dash = rho / rho_star;

            double[] A = { 1.0, 0.940695, 0.578377, -0.202044 };
            double sum_A_T = 0;
            for (int i = 0; i < 4; i++)
            {
                sum_A_T += A[i] / Math.Pow(T_dash, i);
            }
            double my_0_dash = Math.Sqrt(T_dash) / sum_A_T;

            List<double[]> B = new List<double[]>
            {
                new double[] { 0.4864192, -0.2448372, -0.8702035, 0.8716056, -1.051126, 0.3458395 },
                new double[] { 0.3509007, 1.315436, 1.297752, 1.353448, 0.0, 0.0 },
                new double[] { -0.2847572, -1.037026, -1.287846, 0.0, 0.0, -0.02148229 },
                new double[] { 0.07013759, 0.4660127, 0.2292075, -0.4857462, 0.0, 0.0 },
                new double[] { 0.01641220, -0.02884911, 0.0, 0.1607171, 0.0, -0.009603846 },
                new double[] { -0.01163815, -0.008239587, 0.0, 0.0, 0.0, 0.004559914 },
                new double[] { 0.0, 0.0, 0.0, -0.003886659, 0.0, 0.0 }
            };

            double temp_sum = 0;
            for (int i = 0; i < 6; i++)
            {
                double part_T = Math.Pow((1.0 / T_dash) - 1.0, i);

                double sum_B = 0;
                for (int j = 0; j < 7; j++)
                {
                    sum_B += B[j][i] * Math.Pow(rho_dash - 1.0, j);
                }
                temp_sum += part_T * sum_B;
            }

            double my_1_dash = Math.Exp(rho_dash * temp_sum);

            double my_dash = my_0_dash * my_1_dash;

            double my = my_dash * my_star;
            Console.WriteLine("myHW_rhoT_R4 result: my {0}", my);

            return my;
        }

        public static double TcHW_RhoT_R4(double rho, double T)
        {
            Console.WriteLine("tcHW_rhoT_R4 input: ρ {0} kg/m^3, T {1} K", rho, T);

            double T_star = 643.847; // K
            double rho_star = 358; // kg / m^3
            double tc_star = 0.742128; // mW/(m K)

            double T_dash = T / T_star;
            double rho_dash = rho / rho_star;

            double[] A = { 1.0, 37.3223, 22.5485, 13.0465, 0.0, -2.60735 };
            double B_e = -2.506;
            double[] B = { -167.31, 483.656, -191.039, 73.0358, -7.57467 };
            double C_1 = 35429.6;
            double C_2 = 5000.0e6;
            double C_T1 = 0.144847;
            double C_T2 = -5.64493;
            double C_R1 = -2.80000;
            double C_R2 = -0.080738543;
            double C_R3 = -17.9430;
            double rho_r1 = 0.125698;
            double D_1 = -741.112;

            double tau = T_dash / (Math.Abs(T_dash - 1.1) + 1.1);
            double f_1 = Math.Exp(C_T1 * T_dash + C_T2 * Math.Pow(T_dash, 2));
            double f_2 = Math.Exp(C_R1 * Math.Pow(rho_dash - 1.0, 2)) + C_R2 * Math.Exp(C_R3 * Math.Pow(rho_dash - rho_r1, 2));
            double f_3 = 1 + Math.Exp(60.0 * (tau - 1.0) + 20.0);
            double f_4 = 1 + Math.Exp(100.0 * (tau - 1.0) + 15.0);
            double part_C2 = (C_2 * Math.Pow(f_1, 4)) / f_3;
            double part_f2 = (3.5 * f_2) / f_4;

            double temp_sum = 0;
            for (int i = 0; i < 6; i++)
            {
                temp_sum += A[i] * Math.Pow(T_dash, i);
            }
            double tc_o = temp_sum;

            temp_sum = 0;
            for (int i = 1; i < 5; i++)
            {
                temp_sum += B[i] * Math.Pow(rho_dash, i);
            }
            double delta_tc = B[0] * (1.0 - Math.Exp(B_e * rho_dash)) + temp_sum;

            double delta_tc_c = C_1 * f_1 * f_2 * (1.0 + Math.Pow(f_2, 2) * (part_C2 + part_f2));

            double delta_tc_L = D_1 * Math.Pow(f_1, 1.2) * (1.0 - Math.Exp(-Math.Pow(rho_dash / 2.5, 10)));

            double tc_dash = tc_o + delta_tc + delta_tc_c + delta_tc_L;

            double tc = tc_dash * tc_star;
            Console.WriteLine("tcHW_rhoT_R4 result: λ {0}", tc);

            return tc;
        }
    }
}
