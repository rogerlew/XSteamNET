using NUnit.Framework;
using XSteamNET;

namespace XSteamNET.Tests
{
    [TestFixture]
    public class R14_FunctionTests
    {
        private const double maxError = 1e-6;
        private const double maxError_ice_III = 0.003;
        private const double maxError_ice_V = 0.003;
        private const double maxError_ice_VI = 0.003;
        private const double maxError_ice_VII = 0.007;
        private const double maxError_ice_Ih = 0.002;

        [Test]
        public void Test_R14_pmelt_T_function_Ih_1()
        {
            double error = IAPWS_R14.pmelt_T_iceIh(251.165) - 208.566;
            Assert.That(error, Is.InRange(-maxError_ice_Ih, maxError_ice_Ih),
                $"pmelt_T_iceIh not passed Error {error:e} allowed: {maxError_ice_Ih:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_Ih_2()
        {
            double error = IAPWS_R14.pmelt_T_iceIh(254.0) - 187.19355603141994;
            Assert.That(error, Is.InRange(-maxError_ice_Ih, maxError_ice_Ih),
                $"pmelt_T_iceIh not passed Error {error:e} allowed: {maxError_ice_Ih:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_III_1()
        {
            double error = IAPWS_R14.pmelt_T_iceIII(251.165) - 208.566;
            Assert.That(error, Is.InRange(-maxError_ice_III, maxError_ice_III),
                $"pmelt_T_iceIII not passed Error {error:e} allowed: {maxError_ice_III:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_III_2()
        {
            double error = IAPWS_R14.pmelt_T_iceIII(254.0) - 268.685;
            Assert.That(error, Is.InRange(-maxError_ice_III, maxError_ice_III),
                $"pmelt_T_iceIII not passed Error {error:e} allowed: {maxError_ice_III:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_V_1()
        {
            double error = IAPWS_R14.pmelt_T_iceV(256.164) - 350.1;
            Assert.That(error, Is.InRange(-maxError_ice_V, maxError_ice_V),
                $"pmelt_T_iceV not passed Error {error:e} allowed: {maxError_ice_V:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_V_2()
        {
            double error = IAPWS_R14.pmelt_T_iceV(265.0) - 479.640;
            Assert.That(error, Is.InRange(-maxError_ice_V, maxError_ice_V),
                $"pmelt_T_iceV not passed Error {error:e} allowed: {maxError_ice_V:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_VI_1()
        {
            double error = IAPWS_R14.pmelt_T_iceVI(273.31) - 632.4;
            Assert.That(error, Is.InRange(-maxError_ice_VI, maxError_ice_VI),
                $"pmelt_T_iceVI not passed Error {error:e} allowed: {maxError_ice_VI:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_VI_2()
        {
            double error = IAPWS_R14.pmelt_T_iceVI(320.0) - 1356.7565178693883;
            Assert.That(error, Is.InRange(-maxError_ice_VI, maxError_ice_VI),
                $"pmelt_T_iceVI not passed Error {error:e} allowed: {maxError_ice_VI:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_VII_1()
        {
            double error = IAPWS_R14.pmelt_T_iceVII(355.0) - 2216.0;
            Assert.That(error, Is.InRange(-maxError_ice_VII, maxError_ice_VII),
                $"pmelt_T_iceVII not passed Error {error:e} allowed: {maxError_ice_VII:e}");
        }

        [Test]
        public void Test_R14_pmelt_T_function_VII_2()
        {
            double error = IAPWS_R14.pmelt_T_iceVII(550.0) - 6308.71;
            Assert.That(error, Is.InRange(-maxError_ice_VII, maxError_ice_VII),
                $"pmelt_T_iceVII not passed Error {error:e} allowed: {maxError_ice_VII:e}");
        }

        [Test]
        public void Test_R14_psubl_T_function()
        {
            double error = IAPWS_R14.psubl_T(230.0) - 8.94735e-6;
            Assert.That(error, Is.InRange(-maxError, maxError),
                $"psubl_T not passed Error {error:e} allowed: {maxError:e}");
        }
    }
}
