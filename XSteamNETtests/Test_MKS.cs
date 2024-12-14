using System;
using NUnit.Framework;
using XSteamNET;

namespace XSteamNET.Tests
{
    [TestFixture]
    public class MKS_FunctionTests
    {
        private XSteam steamTable;
        private const double maxError = 1e-6;

        [SetUp]
        public void SetUp()
        {
            steamTable = new XSteam(XSteam.UNIT_SYSTEM_MKS);
        }

        [TearDown]
        public void TearDown()
        {
            // No teardown actions required here.
        }

        [Test]
        public void Test_tsat_p()
        {
            double error = steamTable.tsat_p(1.0) - 99.60591861;
            Assert.That(error, Is.InRange(-maxError, maxError));
        }

        [Test]
        public void Test_t_ph()
        {
            double error = steamTable.t_ph(1.0, 100.0) - 23.84481908;
            Assert.That(error, Is.InRange(-maxError, maxError));
        }

        [Test]
        public void Test_t_ps()
        {
            double error = steamTable.t_ps(1.0, 1.0) - 73.70859421;
            Assert.That(error, Is.InRange(-maxError, maxError));
        }

        [Test]
        public void Test_t_hs()
        {
            double error = steamTable.t_hs(100.0, 0.2) - 13.84933511;
            Assert.That(error, Is.InRange(-maxError, maxError), $"t_hs not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_psat_t()
        {
            double error = steamTable.psat_t(100.0) - 1.014179779;
            Assert.That(error, Is.InRange(-maxError, maxError), $"psat_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_p_hs()
        {
            double error = steamTable.p_hs(84.0, 0.296) - 2.295498269;
            Assert.That(error, Is.InRange(-maxError, maxError), $"p_hs not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_hV_p()
        {
            double error = steamTable.hV_p(1.0) - 2674.949641;
            Assert.That(error, Is.InRange(-maxError, maxError), $"hV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_hL_p()
        {
            double error = steamTable.hL_p(1.0) - 417.4364858;
            Assert.That(error, Is.InRange(-maxError, maxError), $"hL_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_hV_t()
        {
            double error = steamTable.hV_t(100.0) - 2675.572029;
            Assert.That(error, Is.InRange(-maxError, maxError), $"hV_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_hL_t()
        {
            double error = steamTable.hL_t(100.0) - 419.099155;
            Assert.That(error, Is.InRange(-maxError, maxError), $"hL_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_h_pt()
        {
            double error = steamTable.h_pt(1.0, 20.0) - 84.01181117;
            Assert.That(error, Is.InRange(-maxError, maxError), $"h_pt not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_h_ps()
        {
            double error = steamTable.h_ps(1.0, 1.0) - 308.6107171;
            Assert.That(error, Is.InRange(-maxError, maxError), $"h_ps not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_h_px()
        {
            double error = steamTable.h_px(1.0, 0.5) - 1546.193063;
            Assert.That(error, Is.InRange(-maxError, maxError), $"h_px not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_h_prho()
        {
            double error = steamTable.h_prho(1.0, 2.0) - 1082.773391;
            Assert.That(error, Is.InRange(-maxError, maxError), $"h_prho not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_h_tx()
        {
            double error = steamTable.h_tx(100.0, 0.5) - 1547.33559211;
            Assert.That(error, Is.InRange(-maxError, maxError), $"h_tx not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_vV_p()
        {
            double error = steamTable.vV_p(1.0) - 1.694022523;
            Assert.That(error, Is.InRange(-maxError, maxError), $"vV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_vL_p()
        {
            double error = steamTable.vL_p(1.0) - 0.001043148;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"vL_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_vV_t()
        {
            double error = steamTable.vV_t(100.0) - 1.671860601;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"vV_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_vL_t()
        {
            double error = steamTable.vL_t(100.0) - 0.001043455;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"vL_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_v_pt()
        {
            double error = steamTable.v_pt(1.0, 100.0) - 1.695959407;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"v_pt not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_v_ph()
        {
            double error = steamTable.v_ph(1.0, 1000.0) - 0.437925658;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"v_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_v_ps()
        {
            double error = steamTable.v_ps(1.0, 5.0) - 1.03463539;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"v_ps not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_rhoV_p()
        {
            double error = steamTable.rhoV_p(1.0) - 0.590310924;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rhoV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_rhoL_p()
        {
            double error = steamTable.rhoL_p(1.0) - 958.6368897;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rhoL_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_rhoV_t()
        {
            double error = steamTable.rhoV_t(100.0) - 0.598135993;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rhoV_t not passed Error {error:e} allowed: {maxError:e}");
        }
        [Test]
        public void Test_rhoL_t()
        {
            double error = steamTable.rhoL_t(100.0) - 958.3542773;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rhoL_t not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_rho_pt()
        {
            double error = steamTable.rho_pt(1.0, 100.0) - 0.589636754;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rho_pt not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_rho_ph()
        {
            double error = steamTable.rho_ph(1.0, 1000.0) - 2.283492601;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rho_ph not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_rho_ps()
        {
            double error = steamTable.rho_ps(1.0, 1.0) - 975.6236788;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"rho_ps not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_sV_p()
        {
            double error = steamTable.sV_p(0.006117) - 9.155465556;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"sV_p not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_sL_p()
        {
            double error = steamTable.sL_p(0.0061171) - 1.8359e-05;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"sL_p not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_sV_t()
        {
            double error = steamTable.sV_t(0.0001) - 9.155756716;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"sV_t not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_sL_t()
        {
            double error = steamTable.sL_t(100.0) - 1.307014328;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"sL_t not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_s_pt()
        {
            double error = steamTable.s_pt(1.0, 20.0) - 0.296482921;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"s_pt not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_s_ph()
        {
            double error = steamTable.s_ph(1.0, 84.01181117) - 0.296813845;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"s_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_uV_p()
        {
            double error = steamTable.uV_p(1.0) - 2505.547389;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"uV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_uL_p()
        {
            double error = steamTable.uL_p(1.0) - 417.332171;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"uL_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_uV_t()
        {
            double error = steamTable.uV_t(100.0) - 2506.015308;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"uV_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_uL_t()
        {
            double error = steamTable.uL_t(100.0) - 418.9933299;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"uL_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_u_pt()
        {
            double error = steamTable.u_pt(1.0, 100.0) - 2506.171426;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"u_pt not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_u_ph()
        {
            double error = steamTable.u_ph(1.0, 1000.0) - 956.2074342;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"u_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_u_ps()
        {
            double error = steamTable.u_ps(1.0, 1.0) - 308.5082185;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"u_ps not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CpV_p()
        {
            double error = steamTable.CpV_p(1.0) - 2.075938025;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CpV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CpL_p()
        {
            double error = steamTable.CpL_p(1.0) - 4.216149431;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CpL_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CpV_t()
        {
            double error = steamTable.CpV_t(100.0) - 2.077491868;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CpV_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CpL_t()
        {
            double error = steamTable.CpL_t(100.0) - 4.216645119;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CpL_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_Cp_pt()
        {
            double error = steamTable.Cp_pt(1.0, 100.0) - 2.074108555;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"Cp_pt not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_Cp_ph()
        {
            double error = steamTable.Cp_ph(1.0, 200.0) - 4.17913573169;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"Cp_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_Cp_ps()
        {
            double error = steamTable.Cp_ps(1.0, 1.0) - 4.190607038;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"Cp_ps not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CvV_p()
        {
            double error = steamTable.CvV_p(1.0) - 1.552696979;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CvV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CvL_p()
        {
            double error = steamTable.CvL_p(1.0) - 3.769699683;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CvL_p not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CvV_t()
        {
            double error = steamTable.CvV_t(100.0) - 1.553698696;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CvV_t not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_CvL_t()
        {
            double error = steamTable.CvL_t(100.0) - 3.76770022;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"CvL_t not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_Cv_pt()
        {
            double error = steamTable.Cv_pt(1.0, 100.0) - 1.551397249;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"Cv_pt not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_Cv_ph()
        {
            double error = steamTable.Cv_ph(1.0, 200.0) - 4.035176364;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"Cv_ph not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_Cv_ps()
        {
            double error = steamTable.Cv_ps(1.0, 1.0) - 3.902919468;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"Cv_ps not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_wV_p()
        {
            double error = steamTable.wV_p(1.0) - 472.0541571;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"wV_p not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_wL_p()
        {
            double error = steamTable.wL_p(1.0) - 1545.451948;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"wL_p not passed. Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_wV_t()
        {
            double error = steamTable.wV_t(100.0) - 472.2559492;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"wV_t not passed. Error {error:e} allowed: {maxError:e}");
        }


        [Test]
        public void Test_wL_t()
        {
            double error = steamTable.wL_t(100.0) - 1545.092249;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"wL_t not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_w_pt()
        {
            double error = steamTable.w_pt(1.0, 100.0) - 472.3375235;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"w_pt not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_w_ph()
        {
            double error = steamTable.w_ph(1.0, 200.0) - 1542.682475;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"w_ph not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_w_ps()
        {
            double error = steamTable.w_ps(1.0, 1.0) - 1557.858535;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"w_ps not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_my_pt()
        {
            double error = steamTable.my_pt(1.0, 100.0) - 1.22704e-05;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"my_pt not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_my_ph()
        {
            double error = steamTable.my_ph(1.0, 100.0) - 0.000914003770302;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"my_ph not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_my_ps()
        {
            double error = steamTable.my_ps(1.0, 1.0) - 0.000384222;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"my_ps not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tcL_p()
        {
            double error = steamTable.tcL_p(1.0) - 0.677593822;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"tcL_p not passed. Error: {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tcV_p()
        {
            double error = steamTable.tcV_p(1.0) - 0.024753668;
            Assert.That(error, Is.InRange(-maxError, maxError), $"tcV_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tcL_t()
        {
            double error = steamTable.tcL_t(25.0) - 0.607458162;
            Assert.That(error, Is.InRange(-maxError, maxError), $"tcL_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tcV_t()
        {
            double error = steamTable.tcV_t(25.0) - 0.018326723;
            Assert.That(error, Is.InRange(-maxError, maxError), $"tcV_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tc_pt()
        {
            double error = steamTable.tc_pt(1.0, 25.0) - 0.607509806;
            Assert.That(error, Is.InRange(-maxError, maxError), $"tc_pt not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tc_ph()
        {
            double error = steamTable.tc_ph(1.0, 100.0) - 0.605710062;
            Assert.That(error, Is.InRange(-maxError, maxError), $"tc_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_tc_hs()
        {
            double error = steamTable.tc_hs(100.0, 0.34) - 0.606283124;
            Assert.That(error, Is.InRange(-maxError, maxError), $"tc_hs not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_st_t()
        {
            double error = steamTable.st_t(100.0) - 0.0589118685877;
            Assert.That(error, Is.InRange(-maxError, maxError), $"st_t not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_st_p()
        {
            double error = steamTable.st_p(1.0) - 0.058987784;
            Assert.That(error, Is.InRange(-maxError, maxError), $"st_p not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_x_ph()
        {
            double error = steamTable.x_ph(1.0, 1000.0) - 0.258055424;
            Assert.That(error, Is.InRange(-maxError, maxError), $"x_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_x_ps()
        {
            double error = steamTable.x_ps(1.0, 4.0) - 0.445397961;
            Assert.That(error, Is.InRange(-maxError, maxError), $"x_ps not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_vx_ph()
        {
            double error = steamTable.vx_ph(1.0, 418.0) - 0.288493093;
            Assert.That(error, Is.InRange(-maxError, maxError), $"vx_ph not passed Error {error:e} allowed: {maxError:e}");
        }

        [Test]
        public void Test_vx_ps()
        {
            double error = steamTable.vx_ps(1.0, 4.0) - 0.999233827;
            Assert.That(error, Is.InRange(-maxError, maxError), $"vx_ps not passed Error {error:e} allowed: {maxError:e}");
        }


    }
}
