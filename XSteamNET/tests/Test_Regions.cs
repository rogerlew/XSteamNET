using System;
using System.Linq;
using NUnit.Framework;
using XSteamNET;

namespace XSteamNET.Tests
{
    [TestFixture]
    public class Region1Tests
    {
        private const double maxError = 1e-8;
        private const double maxMatrixError = 2e-8;

        [Test]
        public void Test_pT_Functions()
        {
            // IF-97 Table 5, Page 9
            double[] p = { 3.0, 80.0, 3.0 };
            double[] T = { 300.0, 300.0, 500.0 };
            double[,] IF97 = {
                { 0.00100215168, 0.000971180894, 0.001202418 },
                { 115.331273, 184.142828, 975.542239 },
                { 112.324818, 106.448356, 971.934985 },
                { 0.392294792, 0.368563852, 2.58041912 },
                { 4.17301218, 4.01008987, 4.65580682 },
                { 1507.73921, 1634.69054, 1240.71337 }
            };

            double[,] R1 = new double[6, 3];

            for (int i = 0; i < 3; i++)
            {
                R1[0, i] = Region1.v1_pT(p[i], T[i]);
                R1[1, i] = Region1.h1_pT(p[i], T[i]);
                R1[2, i] = Region1.u1_pT(p[i], T[i]);
                R1[3, i] = Region1.s1_pT(p[i], T[i]);
                R1[4, i] = Region1.Cp1_pT(p[i], T[i]);
                R1[5, i] = Region1.w1_pT(p[i], T[i]);
            }

            double Region1Error = 0.0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Region1Error += Math.Abs((R1[i, j] - IF97[i, j]) / IF97[i, j]);
                }
            }

            Assert.That(Region1Error, Is.LessThan(maxMatrixError), $"Test of p,T Functions for Region 1 failed. Error: {Region1Error:e}, allowed: {maxMatrixError:e}");
        }

        [Test]
        public void Test_ph_Function()
        {
            // IF-97 Table 7, Page 11
            double[] p = { 3.0, 80.0, 80.0 };
            double[] h = { 500.0, 500.0, 1500.0 };
            double[] IF97 = { 391.798509, 378.108626, 611.041229 };

            double[] R1 = new double[3];
            for (int i = 0; i < 3; i++)
            {
                R1[i] = Region1.T1_ph(p[i], h[i]);
            }

            double T1_ph_Error = 0.0;
            for (int i = 0; i < 3; i++)
            {
                T1_ph_Error += Math.Abs((R1[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(T1_ph_Error, Is.LessThan(maxError), $"Test of T(p,h) Function for Region 1 failed. Error: {T1_ph_Error:e}, allowed: {maxError:e}");
        }

        [Test]
        public void Test_ps_Function()
        {
            // IF-97 Table 9, Page 12
            double[] p = { 3.0, 80.0, 80.0 };
            double[] s = { 0.5, 0.5, 3.0 };
            double[] IF97 = { 307.842258, 309.979785, 565.899909 };

            double[] R1 = new double[3];
            for (int i = 0; i < 3; i++)
            {
                R1[i] = Region1.T1_ps(p[i], s[i]);
            }

            double T1_ps_Error = 0.0;
            for (int i = 0; i < 3; i++)
            {
                T1_ps_Error += Math.Abs((R1[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(T1_ps_Error, Is.LessThan(maxError), $"Test of T(p,s) Function for Region 1 failed. Error: {T1_ps_Error:e}, allowed: {maxError:e}");
        }

        [Test]
        public void Test_hs_Function()
        {
            // IF-97 Table 3, Page 6
            double[] h = { 0.001, 90.0, 1500.0 };
            double[] s = { 0.0, 0.0, 3.4 };
            double[] IF97 = { 0.0009800980612, 91.929547272, 58.68294423 };

            double[] R1 = new double[3];
            for (int i = 0; i < 3; i++)
            {
                R1[i] = Region1.p1_hs(h[i], s[i]);
            }

            double p1_hs_Error = 0.0;
            for (int i = 0; i < 3; i++)
            {
                p1_hs_Error += Math.Abs((R1[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(p1_hs_Error, Is.LessThan(maxError), $"Test of p(h,s) Function for Region 1 failed. Error: {p1_hs_Error:e}, allowed: {maxError:e}");
        }
    }


    [TestFixture]
    public class Region2Tests
    {
        private const double maxError = 1e-8;
        private const double maxMatrixError = 2e-8;

        [Test]
        public void Test_pT_function()
        {
            double[] p = { 0.0035, 0.0035, 30.0 };
            double[] T = { 300.0, 700.0, 700.0 };
            double[,] IF97 =
            {
                { 39.4913866, 92.3015898, 0.00542946619 },
                { 2549.91145, 3335.68375, 2631.49474 },
                { 2411.6916, 3012.62819, 2468.61076 },
                { 8.52238967, 10.1749996, 5.17540298 },
                { 1.91300162, 2.08141274, 10.3505092 },
                { 427.920172, 644.289068, 480.386523 }
            };

            double[] v2 = new double[3];
            double[] h2 = new double[3];
            double[] u2 = new double[3];
            double[] s2 = new double[3];
            double[] Cp2 = new double[3];
            double[] w2 = new double[3];

            // Call each method and assert individually
            for (int i = 0; i < 3; i++)
            {
                v2[i] = Region2.v2_pT(p[i], T[i]);
                Assert.That(v2[i], Is.LessThan(IF97[0, i] * (1 + maxMatrixError)),
                    $"v2_pT failed at index {i}. Expected: {IF97[0, i]:e}, Actual: {v2[i]:e}");

                h2[i] = Region2.h2_pT(p[i], T[i]);
                Assert.That(h2[i], Is.LessThan(IF97[1, i] * (1 + maxMatrixError)),
                    $"h2_pT failed at index {i}. Expected: {IF97[1, i]:e}, Actual: {h2[i]:e}");

                u2[i] = Region2.u2_pT(p[i], T[i]);
                Assert.That(u2[i], Is.LessThan(IF97[2, i] * (1 + maxMatrixError)),
                    $"u2_pT failed at index {i}. Expected: {IF97[2, i]:e}, Actual: {u2[i]:e}");

                s2[i] = Region2.s2_pT(p[i], T[i]);
                Assert.That(s2[i], Is.LessThan(IF97[3, i] * (1 + maxMatrixError)),
                    $"s2_pT failed at index {i}. Expected: {IF97[3, i]:e}, Actual: {s2[i]:e}");

                Cp2[i] = Region2.Cp2_pT(p[i], T[i]);
                Assert.That(Cp2[i], Is.LessThan(IF97[4, i] * (1 + maxMatrixError)),
                    $"Cp2_pT failed at index {i}. Expected: {IF97[4, i]:e}, Actual: {Cp2[i]:e}");

                w2[i] = Region2.w2_pT(p[i], T[i]);
                Assert.That(w2[i], Is.LessThan(IF97[5, i] * (1 + maxMatrixError)),
                    $"w2_pT failed at index {i}. Expected: {IF97[5, i]:e}, Actual: {w2[i]:e}");
            }
        }

        [Test]
        public void Test_pT_meta_function()
        {
            // IF-97 Table 18, Page 20
            double[] T = { 450.0, 440.0, 450.0 };
            double[] p = { 1.0, 1.0, 1.5 };

            // Expected values from IF-97
            double[,] IF97 = new double[,]
            {
                { 0.192516540, 0.186212297, 0.121685206 },  // v
                { 0.276881115e4, 0.274015123e4, 0.272134539e4 },  // h
                { 0.257629461e4, 0.255393894e4, 0.253881758e4 },  // u
                { 0.656660377e1, 0.650218759e1, 0.629170440e1 },  // s
                { 0.276349265e1, 0.298166443e1, 0.362795578e1 },  // cp
                { 0.498408101e3, 0.489363295e3, 0.481941819e3 }   // w
            };

            double tolerance = 1e-4; // Define the maximum allowed error

            // Test each parameter for each row in the table
            for (int i = 0; i < 3; i++)
            {
                // v
                double v = Region2.v2_pT_meta(p[i], T[i]);
                Assert.That(Math.Abs((v - IF97[0, i]) / IF97[0, i]), Is.LessThan(tolerance),
                    $"v2_pT failed for p={p[i]}, T={T[i]}");

                // h
                double h = Region2.h2_pT_meta(p[i], T[i]);
                Assert.That(Math.Abs((h - IF97[1, i]) / IF97[1, i]), Is.LessThan(tolerance),
                    $"h2_pT failed for p={p[i]}, T={T[i]}");

                // u
                double u = Region2.u2_pT_meta(p[i], T[i]);
                Assert.That(Math.Abs((u - IF97[2, i]) / IF97[2, i]), Is.LessThan(tolerance),
                    $"u2_pT failed for p={p[i]}, T={T[i]}");

                // s
                double s = Region2.s2_pT_meta(p[i], T[i]);
                Assert.That(Math.Abs((s - IF97[3, i]) / IF97[3, i]), Is.LessThan(tolerance),
                    $"s2_pT failed for p={p[i]}, T={T[i]}");

                // cp
                double cp = Region2.Cp2_pT_meta(p[i], T[i]);
                Assert.That(Math.Abs((cp - IF97[4, i]) / IF97[4, i]), Is.LessThan(tolerance),
                    $"Cp2_pT failed for p={p[i]}, T={T[i]}");

                // w
                double w = Region2.w2_pT_meta(p[i], T[i]);
                Assert.That(Math.Abs((w - IF97[5, i]) / IF97[5, i]), Is.LessThan(tolerance),
                    $"w2_pT failed for p={p[i]}, T={T[i]}");
            }
        }


        [Test]
        public void Test_ph_function()
        {
            double[] p = { 0.001, 3.0, 3.0, 5.0, 5.0, 25.0, 40.0, 60.0, 60.0 };
            double[] h = { 3000.0, 3000.0, 4000.0, 3500.0, 4000.0, 3500.0, 2700.0, 2700.0, 3200.0 };
            double[] IF97 = { 534.433241, 575.37337, 1010.77577, 801.299102, 1015.31583, 875.279054, 743.056411, 791.137067, 882.75686 };

            double[] R2 = new double[9];
            for (int i = 0; i < 9; i++)
            {
                R2[i] = Region2.T2_ph(p[i], h[i]);
            }

            double T2_ph_error = 0;
            for (int i = 0; i < 9; i++)
            {
                T2_ph_error += Math.Abs((R2[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(T2_ph_error, Is.LessThan(maxMatrixError),
                $"Test of ph Function for Region 2 failed. Error was {T2_ph_error:e} allowed: {maxMatrixError:e}");
        }

        [Test]
        public void Test_ps_function()
        {
            double[] p = { 0.1, 0.1, 2.5, 8.0, 8.0, 90.0, 20.0, 80.0, 80.0 };
            double[] s = { 7.5, 8.0, 8.0, 6.0, 7.5, 6.0, 5.75, 5.25, 5.75 };
            double[] IF97 = { 399.517097, 514.127081, 1039.84917, 600.48404, 1064.95556, 1038.01126, 697.992849, 854.011484, 949.017998 };

            double[] R2 = new double[9];
            for (int i = 0; i < 9; i++)
            {
                R2[i] = Region2.T2_ps(p[i], s[i]);
            }

            double T2_ps_error = 0;
            for (int i = 0; i < 9; i++)
            {
                T2_ps_error += Math.Abs((R2[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(T2_ps_error, Is.LessThan(maxMatrixError),
                $"Test of ps Function for Region 2 failed. Error was {T2_ps_error:e} allowed: {maxMatrixError:e}");
        }

        [Test]
        public void Test_hs_function()
        {
            double[] h = { 2800.0, 2800.0, 4100.0, 2800.0, 3600.0, 3600.0, 2800.0, 2800.0, 3400.0 };
            double[] s = { 6.5, 9.5, 9.5, 6.0, 6.0, 7.0, 5.1, 5.8, 5.8 };
            double[] IF97 = { 1.371012767, 0.001879743844, 0.1024788997, 4.793911442, 83.95519209, 7.527161441, 94.3920206, 8.414574124, 83.76903879 };

            double[] R2 = new double[9];
            for (int i = 0; i < 9; i++)
            {
                R2[i] = Region2.p2_hs(h[i], s[i]);
            }

            double p2_hs_error = 0;
            for (int i = 0; i < 9; i++)
            {
                p2_hs_error += Math.Abs((R2[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(p2_hs_error, Is.LessThan(maxError),
                $"Test of hs Function for Region 2 failed. Error was {p2_hs_error:e} allowed: {maxError:e}");
        }
    }


    [TestFixture]
    public class Region3Tests
    {
        private const double maxError = 1e-8;
        private const double maxMatrixError = 2e-8; // Accumulated error for matrix-based tests.

        [Test]
        public void Test_rhoT_function()
        {
            double[] T = { 650.0, 650.0, 750.0 };
            double[] rho = { 500.0, 200.0, 500.0 };
            double[,] IF97 =
            {
                { 25.5837018, 22.2930643, 78.3095639 },
                { 1863.43019, 2375.12401, 2258.68845 },
                { 1812.26279, 2263.65868, 2102.06932 },
                { 4.05427273, 4.85438792, 4.46971906 },
                { 13.8935717, 44.6579342, 6.34165359 },
                { 502.005554, 383.444594, 760.696041 }
            };

            double[,] R3 = new double[6, 3];
            for (int i = 0; i < 3; i++)
            {
                R3[0, i] = Region3.p3_rhoT(rho[i], T[i]);
                R3[1, i] = Region3.h3_rhoT(rho[i], T[i]);
                R3[2, i] = Region3.u3_rhoT(rho[i], T[i]);
                R3[3, i] = Region3.s3_rhoT(rho[i], T[i]);
                R3[4, i] = Region3.Cp3_rhoT(rho[i], T[i]);
                R3[5, i] = Region3.w3_rhoT(rho[i], T[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    errorSum += Math.Abs((R3[i, j] - IF97[i, j]) / IF97[i, j]);
                }
            }

            Assert.That(errorSum, Is.LessThan(maxMatrixError), $"Test of rhoT Function for Region 3 failed. Error: {errorSum:e}, Allowed: {maxMatrixError:e}");
        }

        [Test]
        public void Test_T_ph_function()
        {
            double[] p = { 20.0, 50.0, 100.0, 20.0, 50.0, 100.0 };
            double[] h = { 1700.0, 2000.0, 2100.0, 2500.0, 2400.0, 2700.0 };
            double[] IF97 = { 629.3083892, 690.5718338, 733.6163014, 641.8418053, 735.1848618, 842.0460876 };

            double[] R3 = new double[6];
            for (int i = 0; i < 6; i++)
            {
                R3[i] = Region3.T3_ph(p[i], h[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 6; i++)
            {
                errorSum += Math.Abs((R3[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(errorSum, Is.LessThan(maxError), $"Test of T(p,h) Function for Region 3 failed. Error: {errorSum:e}, Allowed: {maxError:e}");
        }

        [Test]
        public void Test_v_ph_function()
        {
            double[] p = { 20.0, 50.0, 100.0, 20.0, 50.0, 100.0 };
            double[] h = { 1700.0, 2000.0, 2100.0, 2500.0, 2400.0, 2700.0 };
            double[] IF97 = { 0.001749903962, 0.001908139035, 0.001676229776, 0.006670547043, 0.0028012445, 0.002404234998 };

            double[] R3 = new double[6];
            for (int i = 0; i < 6; i++)
            {
                R3[i] = Region3.v3_ph(p[i], h[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 6; i++)
            {
                errorSum += Math.Abs((R3[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(errorSum, Is.LessThan(1e-7), $"Test of v(p,h) Function for Region 3 failed. Error: {errorSum:e}, Allowed: {1e-7:e}");
        }

        [Test]
        public void Test_T_ps_function()
        {
            double[] p = { 20.0, 50.0, 100.0, 20.0, 50.0, 100.0 };
            double[] s = { 3.7, 3.5, 4, 5, 4.5, 5.0 };
            double[] IF97 = { 620.8841563, 618.1549029, 705.6880237, 640.1176443, 716.3687517, 847.4332825 };

            double[] R3 = new double[6];
            for (int i = 0; i < 6; i++)
            {
                R3[i] = Region3.T3_ps(p[i], s[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 6; i++)
            {
                errorSum += Math.Abs((R3[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(errorSum, Is.LessThan(maxError), $"Test of T(p,s) Function for Region 3 failed. Error: {errorSum:e}, Allowed: {maxError:e}");
        }

        [Test]
        public void Test_v_ps_function()
        {
            double[] p = { 20.0, 50.0, 100.0, 20.0, 50.0, 100.0 };
            double[] s = { 3.7, 3.5, 4.0, 5.0, 4.5, 5.0 };
            double[] IF97 = { 0.001639890984, 0.001423030205, 0.001555893131, 0.006262101987, 0.002332634294, 0.002449610757 };

            double[] R3 = new double[6];
            for (int i = 0; i < 6; i++)
            {
                R3[i] = Region3.v3_ps(p[i], s[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 6; i++)
            {
                errorSum += Math.Abs((R3[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(errorSum, Is.LessThan(maxError), $"Test of v(p,s) Function for Region 3 failed. Error: {errorSum:e}, Allowed: {maxError:e}");
        }

        [Test]
        public void Test_hs_function()
        {
            double[] h = { 1700.0, 2000.0, 2100.0, 2500.0, 2400.0, 2700.0 };
            double[] s = { 3.8, 4.2, 4.3, 5.1, 4.7, 5.0 };
            double[] IF97 = { 25.55703246, 45.40873468, 60.7812334, 17.20612413, 63.63924887, 88.39043281 };

            double[] R3 = new double[6];
            for (int i = 0; i < 6; i++)
            {
                R3[i] = Region3.p3_hs(h[i], s[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 6; i++)
            {
                errorSum += Math.Abs((R3[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(errorSum, Is.LessThan(maxError), $"Test of p(h,s) Function for Region 3 failed. Error: {errorSum:e}, Allowed: {maxError:e}");
        }

        [Test]
        public void Test_pT_function()
        {
            double[] p = { 25.583702, 22.293064, 78.309564 };
            double[] T = { 650.0, 650.0, 750.0 };
            double[] IF97 = { 1863.271389, 2375.696155, 2258.626582 };

            double[] R3 = new double[3];
            for (int i = 0; i < 3; i++)
            {
                R3[i] = Region3.h3_pT(p[i], T[i]);
            }

            double errorSum = 0;
            for (int i = 0; i < 3; i++)
            {
                errorSum += Math.Abs((R3[i] - IF97[i]) / IF97[i]);
            }

            Assert.That(errorSum, Is.LessThan(1e-6), $"Test of h(p,T) Function for Region 3 failed. Error: {errorSum:e}, Allowed: {1e-6:e}");
        }
    }

    [TestFixture]
    public class Region4Tests
    {
        private const double MaxError = 1e-7;

        [Test]
        public void Test_T_Function()
        {
            // Saturation pressure, IF-97, Table 35, Page 34
            double[] T = { 300.0, 500.0, 600.0 };
            double[] IF97 = { 0.00353658941, 2.63889776, 12.3443146 };
            double[] R4 = new double[T.Length];

            for (int i = 0; i < T.Length; i++)
            {
                R4[i] = Region4.p4_T(T[i]);
            }

            double p4_t_error = CalculateRelativeErrorSum(R4, IF97);
            Assert.That(p4_t_error, Is.LessThan(MaxError),
                $"Test of p(T) Function for Region 4 failed. Error was {p4_t_error:e} allowed: {MaxError:e}");
        }

        [Test]
        public void Test_p_Functions()
        {
            double[] p = { 0.1, 1.0, 10.0 };
            double[] IF97 = { 372.755919, 453.035632, 584.149488 };
            double[] R4 = new double[p.Length];

            for (int i = 0; i < p.Length; i++)
            {
                R4[i] = Region4.T4_p(p[i]);
            }

            double T4_p_error = CalculateRelativeErrorSum(R4, IF97);
            Assert.That(T4_p_error, Is.LessThan(MaxError),
                $"Test of T(p) Function for Region 4 failed. Error was {T4_p_error:e} allowed: {MaxError:e}");
        }

        [Test]
        public void Test_s_Functions()
        {
            double[] s = { 1.0, 2.0, 3.0, 3.8, 4.0, 4.2, 7.0, 8.0, 9.0, 5.5, 5.0, 4.5 };
            double[] IF97 = {
                308.5509647, 700.6304472, 1198.359754, 1685.025565, 1816.891476,
                1949.352563, 2723.729985, 2599.04721, 2511.861477, 2687.69385,
                2451.623609, 2144.360448
            };
            double[] R4 = new double[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                R4[i] = Region4.h4_s(s[i]);
            }

            double h4_s_error = CalculateRelativeErrorSum(R4, IF97);
            Assert.That(h4_s_error, Is.LessThan(MaxError),
                $"Test of h(s) Function for Region 4 failed. Error was {h4_s_error:e} allowed: {MaxError:e}");
        }

        private double CalculateRelativeErrorSum(double[] calculated, double[] reference)
        {
            double errorSum = 0.0;

            for (int i = 0; i < calculated.Length; i++)
            {
                errorSum += Math.Abs((calculated[i] - reference[i]) / reference[i]);
            }

            return errorSum;
        }
    }

    [TestFixture]
    public class Region5Tests
    {
        private const double MaxError = 1e-8;
        private const double MaxMatrixError = 2e-8;

        [Test]
        public void Test_pT_function()
        {
            // IF-97 Table 42, Page 39
            double[] T = { 1500.0, 1500.0, 2000.0 };
            double[] p = { 0.5, 8.0, 8.0 };
            double[,] IF97 = {
                { 1.38455354, 0.0865156616, 0.115743146 },
                { 5219.76332, 5206.09634, 6583.80291 },
                { 4527.48654, 4513.97105, 5657.85774 },
                { 9.65408431, 8.36546724, 9.15671044 },
                { 2.61610228, 2.64453866, 2.8530675 },
                { 917.071933, 919.708859, 1054.35806 }
            };

            double[,] R5 = new double[6, 3];

            for (int i = 0; i < 3; i++)
            {
                R5[0, i] = Region5.v5_pT(p[i], T[i]);
                R5[1, i] = Region5.h5_pT(p[i], T[i]);
                R5[2, i] = Region5.u5_pT(p[i], T[i]);
                R5[3, i] = Region5.s5_pT(p[i], T[i]);
                R5[4, i] = Region5.Cp5_pT(p[i], T[i]);
                R5[5, i] = Region5.w5_pT(p[i], T[i]);
            }

            double region5Error = 0.0;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    region5Error += Math.Abs((R5[i, j] - IF97[i, j]) / IF97[i, j]);
                }
            }

            Assert.That(region5Error, Is.LessThan(MaxMatrixError),
                $"Test of p,T Function for Region 5 failed. Error was {region5Error:e} allowed: {MaxMatrixError:e}");
        }

        [Test]
        public void Test_ph_function()
        {
            // T5_ph (Iteration)
            double[] p = { 0.5, 8.0, 8.0 };
            double[] h = { 5219.76331549428, 5206.09634477373, 6583.80290533381 };
            double[] IF97 = { 1500.0, 1500.0, 2000.0 };
            double[] R5 = new double[3];

            for (int i = 0; i < 3; i++)
            {
                R5[i] = Region5.T5_ph(p[i], h[i]);
            }

            double t5PhError = R5.Select((val, i) => Math.Abs((val - IF97[i]) / IF97[i])).Sum();

            Assert.That(t5PhError, Is.LessThan(MaxError * 3.0),
                $"Test of T(p,h) Function for Region 5 failed. Error was {t5PhError:e} allowed: {MaxError:e}");
        }

        [Test]
        public void Test_ps_function()
        {
            // T5_ps (Iteration)
            double[] p = { 0.5, 8.0, 8.0 };
            double[] s = { 9.65408430982588, 8.36546724495503, 9.15671044273249 };
            double[] IF97 = { 1500.0, 1500.0, 2000.0 };
            double[] R5 = new double[3];

            for (int i = 0; i < 3; i++)
            {
                R5[i] = Region5.T5_ps(p[i], s[i]);
            }

            double t5PsError = R5.Select((val, i) => Math.Abs((val - IF97[i]) / IF97[i])).Sum();

            Assert.That(t5PsError, Is.LessThan(2e-4),
                $"Test of T(p,s) Function for Region 5 failed. Error was {t5PsError:e} allowed: {1e-4:e}");
        }
    }
}



