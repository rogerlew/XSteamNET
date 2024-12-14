using System;
using NUnit.Framework;
using XSteamNET;

namespace XSteamNET.Tests
{
	[TestFixture]
	public class UnitConverter_MKS_FunctionTests
	{
		private UnitConverter uc;
		private const double maxError = 1e-8;

		[SetUp]
		public void SetUp()
		{
			uc = new UnitConverter(XSteam.UNIT_SYSTEM_MKS);
		}

		[Test]
		public void Test_toSIunit_p_1_MKS()
		{
			double error = uc.ToSIunit_p(1.0) - 0.1;
			Assert.That(error, Is.InRange(-maxError, maxError),
				$"Test of toSIunit_p for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_p_2_MKS()
		{
			double error = uc.ToSIunit_p(100.0) - 10.0;
			Assert.That(error, Is.InRange(-maxError, maxError),
				$"Test of toSIunit_p for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_p_1_MKS()
		{
			double error = uc.FromSIunit_p(1.0) - 10.0;
			Assert.That(error, Is.InRange(-maxError, maxError),
				$"Test of fromSIunit_p for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_p_2_MKS()
		{
			double error = uc.FromSIunit_p(100.0) - 1000.0;
			Assert.That(error, Is.InRange(-maxError, maxError),
				$"Test of fromSIunit_p for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_T_1_MKS()
		{
			double error = uc.ToSIunit_T(1.0) - 274.15;
			Assert.That(error, Is.InRange(-maxError, maxError),
				$"Test of toSIunit_T for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_T_2_MKS()
		{
			double error = uc.ToSIunit_T(1.0) - 274.15;
			Assert.That(error, Is.InRange(-maxError, maxError),
				$"Test of toSIunit_T for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_T_1_MKS()
		{
			double error = uc.FromSIunit_T(1.0) - 1.0;
			Assert.That(error, Is.LessThan(maxError),
				$"Test of fromSIunit_T for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_T_2_MKS()
		{
			double error = uc.FromSIunit_T(42.0) - 1.0;
			Assert.That(error, Is.LessThan(maxError),
				$"Test of fromSIunit_T for MKS failed. Error was {error:e} allowed: {maxError:e}");
		}
	}


	[TestFixture]
	public class UnitConverter_FLS_Tests
	{
		private UnitConverter uc;
		private const double maxError = 1e-8;

		[SetUp]
		public void SetUp()
		{
			uc = new UnitConverter(XSteam.UNIT_SYSTEM_FLS);
		}

		[Test]
		public void Test_toSIunit_p_FLS()
		{
			double error = uc.ToSIunit_p(1.0) - 1.0;
			Assert.That(error, Is.LessThan(maxError), $"Test of toSIunit_p for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_p_FLS()
		{
			double error = uc.FromSIunit_p(1.0) - 145.0377377968587;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_p for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_T_FLS()
		{
			double kelvin = uc.ToSIunit_T(1.0);
			double error = kelvin - 255.92777777777775;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_T for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_T_FLS()
		{
			double degf = uc.FromSIunit_T(1.0);
			double error = degf + 457.86999999999995;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_T for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_h_FLS()
		{
			double error = uc.ToSIunit_h(1.0) - 2.326;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_h for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_h_FLS()
		{
			double error = uc.FromSIunit_h(1.0) - 0.42992261392949266;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_h for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_v_FLS()
		{
			double error = uc.ToSIunit_v(1.0) - 0.0624279606;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_v for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_v_FLS()
		{
			double error = uc.FromSIunit_v(1.0) - 16.018463367839058;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_v for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_s_FLS()
		{
			double error = uc.ToSIunit_s(1.0) - 4.1868000000086933;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_s for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_s_FLS()
		{
			double error = uc.FromSIunit_s(1.0) - 0.238845896627;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_s for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_u_FLS()
		{
			double error = uc.ToSIunit_u(1.0) - 2.326;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_u for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_u_FLS()
		{
			double error = uc.FromSIunit_u(1.0) - 0.42992261392949266;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_u for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_Cp_FLS()
		{
			double error = uc.ToSIunit_Cp(1.0) - 4.1867981879537446;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_Cp for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_Cp_FLS()
		{
			double error = uc.FromSIunit_Cp(1.0) - 0.238846;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_Cp for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_Cv_FLS()
		{
			double error = uc.ToSIunit_Cv(1.0) - 4.1867981879537446;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_Cv for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_Cv_FLS()
		{
			double error = uc.FromSIunit_Cv(1.0) - 0.238846;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_Cv for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_w_FLS()
		{
			double error = uc.ToSIunit_w(1.0) - 0.3048;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_w for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_w_FLS()
		{
			double error = uc.FromSIunit_w(1.0) - 3.280839895013123;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_w for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_tc_FLS()
		{
			double error = uc.ToSIunit_tc(1.0) - 1.7307356145582558;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_tc for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_tc_FLS()
		{
			double error = uc.FromSIunit_tc(1.0) - 0.577789;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_tc for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_st_FLS()
		{
			double error = uc.ToSIunit_st(1.0) - 14.593902906705587;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_st for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_st_FLS()
		{
			double error = uc.FromSIunit_st(1.0) - 0.068521766;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_st for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_x_FLS()
		{
			double error = uc.ToSIunit_x(1.0) - 1.0;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_x for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_x_FLS()
		{
			double error = uc.FromSIunit_x(1.0) - 1.0;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_x for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_toSIunit_my_FLS()
		{
			double error = uc.ToSIunit_my(1.0) - 0.00041337887312870405;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of toSIunit_my for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}

		[Test]
		public void Test_fromSIunit_my_FLS()
		{
			double error = uc.FromSIunit_my(1.0) - 2419.088311;
			Assert.That(error, Is.InRange(-maxError, maxError), $"Test of fromSIunit_my for FLS failed. Error: {error:e}, Allowed: {maxError:e}");
		}
	}

}
