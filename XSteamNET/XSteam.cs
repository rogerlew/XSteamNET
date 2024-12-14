using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Logging;

namespace XSteamNET
{
    public class XSteam
    {
        private readonly ILogger<XSteam> _logger;

        // Copy constant Values to expose them to the User
        public const int UNIT_SYSTEM_BARE = UnitConverter.__UNIT_SYSTEM_BARE__;
        public const int UNIT_SYSTEM_MKS = UnitConverter.__UNIT_SYSTEM_MKS__;
        public const int UNIT_SYSTEM_FLS = UnitConverter.__UNIT_SYSTEM_FLS__;

        public const int TYPE_ICE_Ih = IAPWS_R14.__TYPE_ICE_Ih__;
        public const int TYPE_ICE_III = IAPWS_R14.__TYPE_ICE_III__;
        public const int TYPE_ICE_V = IAPWS_R14.__TYPE_ICE_V__;
        public const int TYPE_ICE_VI = IAPWS_R14.__TYPE_ICE_VI__;
        public const int TYPE_ICE_VII = IAPWS_R14.__TYPE_ICE_VII__;

        private readonly UnitConverter _unit_converter;

        /// <summary>
        /// Initializes the XSteam class with the given unit system.
        /// </summary>
        /// <param name="unitSystem">Unit system for input and output values.</param>
        public XSteam(int unitSystem = UnitConverter.__UNIT_SYSTEM_BARE__)
        {
            _logger = new LoggerFactory().CreateLogger<XSteam>();
            _unit_converter = new UnitConverter(unitSystem);
            _logger.LogInformation($"Initialized XSteam with Unit System {_unit_converter}");
        }

        /// <summary>
        /// Returns the specific gas constant R in kJ kg^-1 K^-1.
        /// </summary>
        public double SpecificGasConstant()
        {
            return Constants.SPECIFIC_GAS_CONSTANT;
        }

        /// <summary>
        /// Returns the critical temperature with conversion to the selected unit system.
        /// </summary>
        public double CriticalTemperatur()
        {
            return _unit_converter.FromSIunit_T(Constants.CRITICAL_TEMPERATURE);
        }

        /// <summary>
        /// Returns the critical pressure with conversion to the selected unit system.
        /// </summary>
        public double CriticalPressure()
        {
            return _unit_converter.FromSIunit_p(Constants.CRITICAL_PRESSURE);
        }

        /// <summary>
        /// Returns the critical density with conversion to the selected unit system.
        /// </summary>
        public double CriticalDensity()
        {
            return _unit_converter.FromSIunit_p(Constants.CRITICAL_DENSITY);
        }

        /// <summary>
        /// Returns the temperature of the triple point with conversion to the selected unit system.
        /// </summary>
        public double TriplePointTemperatur()
        {
            return _unit_converter.FromSIunit_T(Constants.TRIPLE_POINT_TEMPERATURE);
        }

        /// <summary>
        /// Returns the pressure of the triple point with conversion to the selected unit system.
        /// </summary>
        public double TriplePointPressure()
        {
            return _unit_converter.FromSIunit_p(Constants.TRIPLE_POINT_PRESSURE);
        }

        /// <summary>
        /// Returns the absolute zero temperature with conversion to the selected unit system.
        /// </summary>
        public double ZeroPointTemperature()
        {
            return _unit_converter.FromSIunit_T(0.0);
        }


        /// <summary>
        /// Saturation-temperature as a function of pressure.
        /// </summary>
        public double tsat_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                return _unit_converter.FromSIunit_T(Region4.T4_p(p));
            }
            else
            {
                _logger.LogWarning($"pressure {p} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturation-temperature as a function of entropy.
        /// </summary>
        public double tsat_s(double s)
        {
            s = _unit_converter.ToSIunit_s(s);
            if (s > -0.0001545495919 && s < 9.155759395)
            {
                double ps = Region4.p4_s(s);
                return _unit_converter.FromSIunit_T(Region4.T4_p(ps));
            }
            else
            {
                _logger.LogWarning($"entropy value {s} is out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Temperature as a function of pressure and enthalpy.
        /// </summary>
        public double t_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(p, h);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_T(Region1.T1_ph(p, h));
                case 2:
                    return _unit_converter.FromSIunit_T(Region2.T2_ph(p, h));
                case 3:
                    return _unit_converter.FromSIunit_T(Region3.T3_ph(p, h));
                case 4:
                    return _unit_converter.FromSIunit_T(Region4.T4_p(p));
                case 5:
                    return _unit_converter.FromSIunit_T(Region5.T5_ph(p, h));
                default:
                    _logger.LogWarning(
                        $"Region switch t_ph returned unknown value {region} for input p {p} and h {h}");
                    return double.NaN;
            }
        }

        /// <summary>
        /// Temperature as a function of pressure and entropy.
        /// </summary>
        public double t_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(p, s);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_T(Region1.T1_ps(p, s));
                case 2:
                    return _unit_converter.FromSIunit_T(Region2.T2_ps(p, s));
                case 3:
                    return _unit_converter.FromSIunit_T(Region3.T3_ps(p, s));
                case 4:
                    return _unit_converter.FromSIunit_T(Region4.T4_p(p));
                case 5:
                    return _unit_converter.FromSIunit_T(Region5.T5_ps(p, s));
                default:
                    _logger.LogWarning(
                        $"Region switch t_ps returned unknown value {region} for input p {p} and s {s}");
                    return double.NaN;
            }
        }

        /// <summary>
        /// Temperature as a function of enthalpy and entropy.
        /// </summary>
        public double t_hs(double h, double s)
        {
            h = _unit_converter.ToSIunit_h(h);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_hs(h, s);
            switch (region)
            {
                case 1:
                    double p1 = Region1.p1_hs(h, s);
                    return _unit_converter.FromSIunit_T(Region1.T1_ph(p1, h));
                case 2:
                    double p2 = Region2.p2_hs(h, s);
                    return _unit_converter.FromSIunit_T(Region2.T2_ph(p2, h));
                case 3:
                    double p3 = Region3.p3_hs(h, s);
                    return _unit_converter.FromSIunit_T(Region3.T3_ph(p3, h));
                case 4:
                    return _unit_converter.FromSIunit_T(Region4.T4_hs(h, s));
                case 5:
                    _logger.LogError(
                        $"functions t_hs is not available in region 5 for input h {h} and s {s}");
                    return double.NaN;
                default:
                    _logger.LogWarning(
                        $"Region switch t_hs returned unknown value {region} for input h {h} and s {s}");
                    return double.NaN;
            }
        }


        /// <summary>
        /// Saturation pressure as a function of entropy.
        /// </summary>
        public double psat_s(double s)
        {
            s = _unit_converter.ToSIunit_s(s);
            if (s > -0.0001545495919 && s < 9.155759395)
            {
                return _unit_converter.FromSIunit_p(Region4.p4_s(s));
            }
            else
            {
                _logger.LogWarning($"Entropy value {s} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturation pressure as a function of temperature.
        /// </summary>
        public double psat_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T < 647.096 && T > 273.1)
            {
                return _unit_converter.FromSIunit_p(Region4.p4_T(T));
            }
            else
            {
                _logger.LogWarning($"Temperature {T} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Pressure as a function of enthalpy and entropy.
        /// </summary>
        public double p_hs(double h, double s)
        {
            h = _unit_converter.ToSIunit_h(h);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_hs(h, s);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_p(Region1.p1_hs(h, s));
                case 2:
                    return _unit_converter.FromSIunit_p(Region2.p2_hs(h, s));
                case 3:
                    return _unit_converter.FromSIunit_p(Region3.p3_hs(h, s));
                case 4:
                    double tSat = Region4.T4_hs(h, s);
                    return _unit_converter.FromSIunit_p(Region4.p4_T(tSat));
                case 5:
                    _logger.LogWarning(
                        $"Functions p_hs is not available in region 5 for input h {h} and s {s}");
                    return double.NaN;
                default:
                    _logger.LogWarning(
                        $"Region switch p_hs returned unknown value {region} for input h {h} and s {s}");
                    return double.NaN;
            }
        }

        /// <summary>
        /// Pressure as a function of enthalpy and density.
        /// </summary>
        public double p_hrho(double h, double rho)
        {
            if (rho <= 0.0)
            {
                _logger.LogError($"Negative values for density rho not allowed: {rho}");
                throw new ArgumentOutOfRangeException(nameof(rho), "rho out of range");
            }

            h = _unit_converter.ToSIunit_h(h);
            double High_Bound = _unit_converter.FromSIunit_p(100);
            double Low_Bound = _unit_converter.FromSIunit_p(0.000611657);
            double ps = _unit_converter.FromSIunit_p(10);
            double rhos = 1 / v_ph(ps, h);
            int step_counter = 0;

            while (Math.Abs(rho - rhos) > 0.0000001)
            {
                step_counter++;
                double last_rhos = rhos;

                rhos = 1 / v_ph(ps, h);

                if (last_rhos == rhos)
                {
                    _logger.LogWarning(
                        $"p_hrho stopped iterating after {step_counter} steps because values did not converge for input values h {h} and rho {rho}");
                    break;
                }

                if (rhos >= rho)
                {
                    High_Bound = ps;
                }
                else
                {
                    Low_Bound = ps;
                }

                ps = (Low_Bound + High_Bound) / 2;
            }

            return ps;
        }

        /// <summary>
        /// Saturated vapor enthalpy as a function of pressure.
        /// </summary>
        public double hV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                return _unit_converter.FromSIunit_h(Region4.h4V_p(p));
            }
            else
            {
                _logger.LogWarning($"Pressure {p} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid enthalpy as a function of pressure.
        /// </summary>
        public double hL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                return _unit_converter.FromSIunit_h(Region4.h4L_p(p));
            }
            else
            {
                _logger.LogWarning($"Pressure {p} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor enthalpy as a function of temperature.
        /// </summary>
        public double hV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                double p = Region4.p4_T(T);
                return _unit_converter.FromSIunit_h(Region4.h4V_p(p));
            }
            else
            {
                _logger.LogWarning($"Temperature {T} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid enthalpy as a function of temperature.
        /// </summary>
        public double hL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                double p = Region4.p4_T(T);
                return _unit_converter.FromSIunit_h(Region4.h4L_p(p));
            }
            else
            {
                _logger.LogWarning($"Temperature {T} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Enthalpy as a function of pressure and temperature.
        /// </summary>
        public double h_pt(double p, double t)
        {
            p = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(p, T);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_h(Region1.h1_pT(p, T));
                case 2:
                    return _unit_converter.FromSIunit_h(Region2.h2_pT(p, T));
                case 3:
                    return _unit_converter.FromSIunit_h(Region3.h3_pT(p, T));
                case 4:
                    _logger.LogWarning(
                        $"Function h_pt is not available in region 4 for input p {p} and T {T}");
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_h(Region5.h5_pT(p, T));
                default:
                    _logger.LogWarning(
                        $"Region switch h_pt returned unknown value {region} for input p {p} and T {T}");
                    return double.NaN;
            }
        }


        /// <summary>
        /// Enthalpy as a function of pressure and entropy.
        /// </summary>
        public double h_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(p, s);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_h(
                        Region1.h1_pT(p, Region1.T1_ps(p, s))
                    );
                case 2:
                    return _unit_converter.FromSIunit_h(
                        Region2.h2_pT(p, Region2.T2_ps(p, s))
                    );
                case 3:
                    return _unit_converter.FromSIunit_h(
                        Region3.h3_rhoT(1 / Region3.v3_ps(p, s), Region3.T3_ps(p, s))
                    );
                case 4:
                    double xs = Region4.x4_ps(p, s);
                    return _unit_converter.FromSIunit_h(
                        xs * Region4.h4V_p(p) + (1 - xs) * Region4.h4L_p(p)
                    );
                case 5:
                    return _unit_converter.FromSIunit_h(
                        Region5.h5_pT(p, Region5.T5_ps(p, s))
                    );
                default:
                    _logger.LogWarning(
                        $"Region switch h_ps returned unknown value {region} for input p {p} and s {s}");
                    return double.NaN;
            }
        }

        /// <summary>
        /// Enthalpy as a function of pressure and vapor fraction.
        /// </summary>
        public double h_px(double p, double x)
        {
            p = _unit_converter.ToSIunit_p(p);
            x = _unit_converter.ToSIunit_x(x);
            if (x > 1 || x < 0 || p >= 22.064)
            {
                _logger.LogWarning($"Vapor fraction {x} and/or pressure {p} out of range");
                return double.NaN;
            }
            double hL = Region4.h4L_p(p);
            double hV = Region4.h4V_p(p);
            return _unit_converter.FromSIunit_h(hL + x * (hV - hL));
        }

        /// <summary>
        /// Enthalpy as a function of pressure and density.
        /// </summary>
        public double h_prho(double p, double rho)
        {
            if (rho <= 0.0)
            {
                _logger.LogError($"Negative values for density rho not allowed {rho}");
                throw new ArgumentOutOfRangeException(nameof(rho), "rho out of range");
            }
            p = _unit_converter.ToSIunit_p(p);
            rho = 1 / _unit_converter.ToSIunit_v(1 / rho);
            int region = RegionSelection.Region_prho(p, rho);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_h(
                        Region1.h1_pT(p, Region1.T1_prho(p, rho))
                    );
                case 2:
                    return _unit_converter.FromSIunit_h(
                        Region2.h2_pT(p, Region2.T2_prho(p, rho))
                    );
                case 3:
                    return _unit_converter.FromSIunit_h(
                        Region3.h3_rhoT(rho, Region3.T3_prho(p, rho))
                    );
                case 4:
                    double vV, vL, hV, hL, x;
                    if (p < 16.529)
                    {
                        vV = Region2.v2_pT(p, Region4.T4_p(p));
                        vL = Region1.v1_pT(p, Region4.T4_p(p));
                    }
                    else
                    {
                        vV = Region3.v3_ph(p, Region4.h4V_p(p));
                        vL = Region3.v3_ph(p, Region4.h4L_p(p));
                    }
                    hV = Region4.h4V_p(p);
                    hL = Region4.h4L_p(p);
                    x = (1 / rho - vL) / (vV - vL);
                    return _unit_converter.FromSIunit_h((1 - x) * hL + x * hV);
                case 5:
                    return _unit_converter.FromSIunit_h(
                        Region5.h5_pT(p, Region5.T5_prho(p, rho))
                    );
                default:
                    _logger.LogWarning(
                        $"Region switch h_prho returned unknown value {region} for input p {p} and rho {rho}");
                    return double.NaN;
            }
        }


        /// <summary>
        /// Enthalpy as a function of temperature and vapor fraction.
        /// </summary>
        public double h_tx(double t, double x)
        {
            double T = _unit_converter.ToSIunit_T(t);
            x = _unit_converter.ToSIunit_x(x);
            if (x > 1 || x < 0 || T >= 647.096)
            {
                _logger.LogWarning($"Vapor fraction {x} and/or temperature {T} out of range");
                return double.NaN;
            }
            double p = Region4.p4_T(T);
            double hL = Region4.h4L_p(p);
            double hV = Region4.h4V_p(p);
            return _unit_converter.FromSIunit_h(hL + x * (hV - hL));
        }

        /// <summary>
        /// Saturated vapor volume as a function of pressure.
        /// </summary>
        public double vV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_v(
                        Region2.v2_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ph(p, Region4.h4V_p(p))
                    );
                }
            }
            else
            {
                _logger.LogWarning($"Pressure {p} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid volume as a function of pressure.
        /// </summary>
        public double vL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_v(
                        Region1.v1_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ph(p, Region4.h4L_p(p))
                    );
                }
            }
            else
            {
                _logger.LogWarning($"Pressure {p} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor volume as a function of temperature.
        /// </summary>
        public double vV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_v(
                        Region2.v2_pT(Region4.p4_T(T), T)
                    );
                }
                else
                {
                    double p = Region4.p4_T(T);
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ph(p, Region4.h4V_p(p))
                    );
                }
            }
            else
            {
                _logger.LogWarning($"Temperature {T} out of range");
                return double.NaN;
            }
        }


        /// <summary>
        /// Saturated liquid volume as a function of temperature.
        /// </summary>
        public double vL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_v(
                        Region1.v1_pT(Region4.p4_T(T), T)
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ph(
                            Region4.p4_T(T),
                            Region4.h4L_p(Region4.p4_T(T))
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning($"Temperature {T} out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific volume as a function of pressure and temperature.
        /// </summary>
        public double v_pt(double p, double t)
        {
            double pSI = _unit_converter.ToSIunit_p(p);
            double TSI = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(pSI, TSI);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_v(Region1.v1_pT(pSI, TSI));
                case 2:
                    return _unit_converter.FromSIunit_v(Region2.v2_pT(pSI, TSI));
                case 3:
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ph(pSI, Region3.h3_pT(pSI, TSI))
                    );
                case 4:
                    _logger.LogWarning(
                        $"Function v_pt is not available in region 4 for input p {pSI} and T {TSI}"
                    );
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_v(Region5.v5_pT(pSI, TSI));
                default:
                    _logger.LogWarning(
                        $"Region switch v_pt returned unknown value {region} for input p {pSI} and T {TSI}"
                    );
                    return double.NaN;
            }
        }


        /// <summary>
        /// Specific volume as a function of pressure and enthalpy.
        /// </summary>
        public double v_ph(double p, double h)
        {
            double pSI = _unit_converter.ToSIunit_p(p);
            double hSI = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(pSI, hSI);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_v(
                        Region1.v1_pT(pSI, Region1.T1_ph(pSI, hSI))
                    );
                case 2:
                    return _unit_converter.FromSIunit_v(
                        Region2.v2_pT(pSI, Region2.T2_ph(pSI, hSI))
                    );
                case 3:
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ph(pSI, hSI)
                    );
                case 4:
                    double xs = Region4.x4_ph(pSI, hSI);
                    double v4v, v4L;

                    if (pSI < 16.529)
                    {
                        v4v = Region2.v2_pT(pSI, Region4.T4_p(pSI));
                        v4L = Region1.v1_pT(pSI, Region4.T4_p(pSI));
                    }
                    else
                    {
                        v4v = Region3.v3_ph(pSI, Region4.h4V_p(pSI));
                        v4L = Region3.v3_ph(pSI, Region4.h4L_p(pSI));
                    }

                    return _unit_converter.FromSIunit_v(xs * v4v + (1 - xs) * v4L);
                case 5:
                    double Ts = Region5.T5_ph(pSI, hSI);
                    return _unit_converter.FromSIunit_v(Region5.v5_pT(pSI, Ts));
                default:
                    _logger.LogWarning(
                        $"Region switch v_ph returned unknown value {region} for input p {pSI} and h {hSI}"
                    );
                    return double.NaN;
            }
        }

        /// <summary>
        /// Specific volume as a function of pressure and entropy.
        /// </summary>
        public double v_ps(double p, double s)
        {
            double pSI = _unit_converter.ToSIunit_p(p);
            double sSI = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(pSI, sSI);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_v(
                        Region1.v1_pT(pSI, Region1.T1_ps(pSI, sSI))
                    );
                case 2:
                    return _unit_converter.FromSIunit_v(
                        Region2.v2_pT(pSI, Region2.T2_ps(pSI, sSI))
                    );
                case 3:
                    return _unit_converter.FromSIunit_v(
                        Region3.v3_ps(pSI, sSI)
                    );
                case 4:
                    double xs = Region4.x4_ps(pSI, sSI);
                    double v4v, v4L;

                    if (pSI < 16.529)
                    {
                        v4v = Region2.v2_pT(pSI, Region4.T4_p(pSI));
                        v4L = Region1.v1_pT(pSI, Region4.T4_p(pSI));
                    }
                    else
                    {
                        v4v = Region3.v3_ph(pSI, Region4.h4V_p(pSI));
                        v4L = Region3.v3_ph(pSI, Region4.h4L_p(pSI));
                    }

                    return _unit_converter.FromSIunit_v(xs * v4v + (1 - xs) * v4L);
                case 5:
                    double Ts = Region5.T5_ps(pSI, sSI);
                    return _unit_converter.FromSIunit_v(Region5.v5_pT(pSI, Ts));
                default:
                    _logger.LogWarning(
                        $"Region switch v_ps returned unknown value {region} for input p {pSI} and s {sSI}"
                    );
                    return double.NaN;
            }
        }


        /// <summary>
        /// Saturated vapor density as a function of pressure.
        /// </summary>
        public double rhoV_p(double p)
        {
            return 1 / vV_p(p);
        }

        /// <summary>
        /// Saturated liquid density as a function of pressure.
        /// </summary>
        public double rhoL_p(double p)
        {
            return 1 / vL_p(p);
        }

        /// <summary>
        /// Saturated vapor density as a function of temperature.
        /// </summary>
        public double rhoV_t(double t)
        {
            return 1 / vV_t(t);
        }

        /// <summary>
        /// Saturated liquid density as a function of temperature.
        /// </summary>
        public double rhoL_t(double t)
        {
            return 1 / vL_t(t);
        }

        /// <summary>
        /// Density as a function of pressure and temperature.
        /// </summary>
        public double rho_pt(double p, double t)
        {
            return 1 / v_pt(p, t);
        }


        /// <summary>
        /// Density as a function of pressure and enthalpy.
        /// </summary>
        public double rho_ph(double p, double h)
        {
            return 1 / v_ph(p, h);
        }

        /// <summary>
        /// Density as a function of pressure and entropy.
        /// </summary>
        public double rho_ps(double p, double s)
        {
            return 1 / v_ps(p, s);
        }

        /// <summary>
        /// Saturated vapor entropy as a function of pressure.
        /// </summary>
        public double sV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_s(Region2.s2_pT(p, Region4.T4_p(p)));
                }
                else
                {
                    return _unit_converter.FromSIunit_s(
                        Region3.s3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4V_p(p)),
                            Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid entropy as a function of pressure.
        /// </summary>
        public double sL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_s(Region1.s1_pT(p, Region4.T4_p(p)));
                }
                else
                {
                    return _unit_converter.FromSIunit_s(
                        Region3.s3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4L_p(p)),
                            Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor entropy as a function of temperature.
        /// </summary>
        public double sV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_s(Region2.s2_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_s(
                        Region3.s3_rhoT(
                            1 / Region3.v3_ph(Region4.p4_T(T), Region4.h4V_p(Region4.p4_T(T))),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid entropy as a function of temperature.
        /// </summary>
        public double sL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_s(Region1.s1_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_s(
                        Region3.s3_rhoT(
                            1 / Region3.v3_ph(Region4.p4_T(T), Region4.h4L_p(Region4.p4_T(T))),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific entropy as a function of pressure and temperature.
        /// Returns saturated vapor entropy if mixture.
        /// </summary>
        public double s_pt(double p, double t)
        {
            double P = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(P, T);
            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_s(Region1.s1_pT(P, T));
                case 2:
                    return _unit_converter.FromSIunit_s(Region2.s2_pT(P, T));
                case 3:
                    double hs = Region3.h3_pT(P, T);
                    double rhos = 1 / Region3.v3_ph(P, hs);
                    return _unit_converter.FromSIunit_s(Region3.s3_rhoT(rhos, T));
                case 4:
                    _logger.LogWarning("Function s_pt is not available in region 4 (p {0}, T {1})", P, T);
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_s(Region5.s5_pT(P, T));
                default:
                    _logger.LogWarning(
                        "Region switch s_pt returned unknown value {0} for input p {1} and T {2}",
                        region, P, T
                    );
                    return double.NaN;
            }
        }


        /// <summary>
        /// Specific entropy as a function of pressure and enthalpy.
        /// </summary>
        public double s_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(p, h);

            if (region == 1)
            {
                double T = Region1.T1_ph(p, h);
                return _unit_converter.FromSIunit_s(Region1.s1_pT(p, T));
            }
            else if (region == 2)
            {
                double T = Region2.T2_ph(p, h);
                return _unit_converter.FromSIunit_s(Region2.s2_pT(p, T));
            }
            else if (region == 3)
            {
                double rhos = 1 / Region3.v3_ph(p, h);
                double Ts = Region3.T3_ph(p, h);
                return _unit_converter.FromSIunit_s(Region3.s3_rhoT(rhos, Ts));
            }
            else if (region == 4)
            {
                double Ts = Region4.T4_p(p);
                double xs = Region4.x4_ph(p, h);
                double s4v, s4L;

                if (p < 16.529)
                {
                    s4v = Region2.s2_pT(p, Ts);
                    s4L = Region1.s1_pT(p, Ts);
                }
                else
                {
                    double v4v = Region3.v3_ph(p, Region4.h4V_p(p));
                    s4v = Region3.s3_rhoT(1 / v4v, Ts);

                    double v4L = Region3.v3_ph(p, Region4.h4L_p(p));
                    s4L = Region3.s3_rhoT(1 / v4L, Ts);
                }

                return _unit_converter.FromSIunit_s((xs * s4v + (1 - xs) * s4L));
            }
            else if (region == 5)
            {
                double T = Region5.T5_ph(p, h);
                return _unit_converter.FromSIunit_s(Region5.s5_pT(p, T));
            }
            else
            {
                _logger.LogWarning("Region switch s_ph returned unknown value {0} for input p {1} and h {2}", region, p, h);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor internal energy as a function of pressure.
        /// </summary>
        public double uV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);

            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_u(Region2.u2_pT(p, Region4.T4_p(p)));
                }
                else
                {
                    return _unit_converter.FromSIunit_u(
                        Region3.u3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4V_p(p)),
                            Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid internal energy as a function of pressure.
        /// </summary>
        public double uL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);

            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_u(Region1.u1_pT(p, Region4.T4_p(p)));
                }
                else
                {
                    return _unit_converter.FromSIunit_u(
                        Region3.u3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4L_p(p)),
                            Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }


        /// <summary>
        /// Saturated vapour internal energy as a function of temperature.
        /// </summary>
        public double uV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_u(Region2.u2_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_u(
                        Region3.u3_rhoT(
                            1 / Region3.v3_ph(
                                Region4.p4_T(T), Region4.h4V_p(Region4.p4_T(T))),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid internal energy as a function of temperature.
        /// </summary>
        public double uL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_u(Region1.u1_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_u(
                        Region3.u3_rhoT(
                            1 / Region3.v3_ph(
                                Region4.p4_T(T), Region4.h4L_p(Region4.p4_T(T))),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific internal energy as a function of pressure and temperature.
        /// </summary>
        public double u_pt(double p, double t)
        {
            double P = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(P, T);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_u(Region1.u1_pT(P, T));
                case 2:
                    return _unit_converter.FromSIunit_u(Region2.u2_pT(P, T));
                case 3:
                    double hs = Region3.h3_pT(P, T);
                    double rhos = 1 / Region3.v3_ph(P, hs);
                    return _unit_converter.FromSIunit_u(Region3.u3_rhoT(rhos, T));
                case 4:
                    _logger.LogWarning("function u_pt is not available in region 4 for input p {0} and T {1}", P, T);
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_u(Region5.u5_pT(P, T));
                default:
                    _logger.LogWarning("Region switch u_pt returned unknown value {0} for input p {1} and T {2}", region, P, T);
                    return double.NaN;
            }
        }


        /// <summary>
        /// Specific internal energy as a function of pressure and enthalpy.
        /// </summary>
        public double u_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(p, h);

            if (region == 1)
            {
                double Ts = Region1.T1_ph(p, h);
                return _unit_converter.FromSIunit_u(Region1.u1_pT(p, Ts));
            }
            else if (region == 2)
            {
                double Ts = Region2.T2_ph(p, h);
                return _unit_converter.FromSIunit_u(Region2.u2_pT(p, Ts));
            }
            else if (region == 3)
            {
                double rhos = 1 / Region3.v3_ph(p, h);
                double Ts = Region3.T3_ph(p, h);
                return _unit_converter.FromSIunit_u(Region3.u3_rhoT(rhos, Ts));
            }
            else if (region == 4)
            {
                double Ts = Region4.T4_p(p);
                double xs = Region4.x4_ph(p, h);
                double u4v, u4L;

                if (p < 16.529)
                {
                    u4v = Region2.u2_pT(p, Ts);
                    u4L = Region1.u1_pT(p, Ts);
                }
                else
                {
                    double v4v = Region3.v3_ph(p, Region4.h4V_p(p));
                    u4v = Region3.u3_rhoT(1 / v4v, Ts);
                    double v4L = Region3.v3_ph(p, Region4.h4L_p(p));
                    u4L = Region3.u3_rhoT(1 / v4L, Ts);
                }

                return _unit_converter.FromSIunit_u(xs * u4v + (1 - xs) * u4L);
            }
            else if (region == 5)
            {
                double Ts = Region5.T5_ph(p, h);
                return _unit_converter.FromSIunit_u(Region5.u5_pT(p, Ts));
            }
            else
            {
                _logger.LogWarning("Region switch u_ph returned unknown value {0} for input p {1} and h {2}", region, p, h);
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific internal energy as a function of pressure and entropy.
        /// </summary>
        public double u_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(p, s);

            if (region == 1)
            {
                double Ts = Region1.T1_ps(p, s);
                return _unit_converter.FromSIunit_u(Region1.u1_pT(p, Ts));
            }
            else if (region == 2)
            {
                double Ts = Region2.T2_ps(p, s);
                return _unit_converter.FromSIunit_u(Region2.u2_pT(p, Ts));
            }
            else if (region == 3)
            {
                double rhos = 1 / Region3.v3_ps(p, s);
                double Ts = Region3.T3_ps(p, s);
                return _unit_converter.FromSIunit_u(Region3.u3_rhoT(rhos, Ts));
            }
            else if (region == 4)
            {
                double uLp, uVp;
                if (p < 16.529)
                {
                    uLp = Region1.u1_pT(p, Region4.T4_p(p));
                    uVp = Region2.u2_pT(p, Region4.T4_p(p));
                }
                else
                {
                    uLp = Region3.u3_rhoT(1 / Region3.v3_ph(p, Region4.h4L_p(p)), Region4.T4_p(p));
                    uVp = Region3.u3_rhoT(1 / Region3.v3_ph(p, Region4.h4V_p(p)), Region4.T4_p(p));
                }

                double xs = Region4.x4_ps(p, s);
                return _unit_converter.FromSIunit_u(xs * uVp + (1 - xs) * uLp);
            }
            else if (region == 5)
            {
                double Ts = Region5.T5_ps(p, s);
                return _unit_converter.FromSIunit_u(Region5.u5_pT(p, Ts));
            }
            else
            {
                _logger.LogWarning("Region switch u_ps returned unknown value {0} for input p {1} and s {2}", region, p, s);
                return double.NaN;
            }
        }


        /// <summary>
        /// Saturated vapor heat capacity as a function of pressure.
        /// </summary>
        public double CpV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region2.Cp2_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region3.Cp3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4V_p(p)), Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid heat capacity as a function of pressure.
        /// </summary>
        public double CpL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region1.Cp1_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region3.Cp3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4L_p(p)), Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor heat capacity as a function of temperature.
        /// </summary>
        public double CpV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region2.Cp2_pT(Region4.p4_T(T), T)
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region3.Cp3_rhoT(
                            1 /
                            Region3.v3_ph(
                                Region4.p4_T(T), Region4.h4V_p(Region4.p4_T(T))
                            ),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }


        /// <summary>
        /// Saturated liquid heat capacity as a function of temperature.
        /// </summary>
        public double CpL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_Cp(Region1.Cp1_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_Cp(
                        Region3.Cp3_rhoT(
                            1 / Region3.v3_ph(Region4.p4_T(T), Region4.h4L_p(Region4.p4_T(T))),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific isobaric heat capacity as a function of pressure and temperature.
        /// </summary>
        public double Cp_pt(double p, double t)
        {
            double P = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(P, T);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_Cp(Region1.Cp1_pT(P, T));
                case 2:
                    return _unit_converter.FromSIunit_Cp(Region2.Cp2_pT(P, T));
                case 3:
                    double hs = Region3.h3_pT(P, T);
                    double rhos = 1 / Region3.v3_ph(P, hs);
                    return _unit_converter.FromSIunit_Cp(Region3.Cp3_rhoT(rhos, T));
                case 4:
                    _logger.LogWarning("Function Cp_pt is not available in region 4 for input p {0} and T {1}", P, T);
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_Cp(Region5.Cp5_pT(P, T));
                default:
                    _logger.LogWarning("Region switch Cp_pt returned unknown value {0} for input p {1} and T {2}", region, P, T);
                    return double.NaN;
            }
        }

        /// <summary>
        /// Specific isobaric heat capacity as a function of pressure and enthalpy.
        /// </summary>
        public double Cp_ph(double p, double h)
        {
            double P = _unit_converter.ToSIunit_p(p);
            double H = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(P, H);

            switch (region)
            {
                case 1:
                    double Ts1 = Region1.T1_ph(P, H);
                    return _unit_converter.FromSIunit_Cp(Region1.Cp1_pT(P, Ts1));
                case 2:
                    double Ts2 = Region2.T2_ph(P, H);
                    return _unit_converter.FromSIunit_Cp(Region2.Cp2_pT(P, Ts2));
                case 3:
                    double rhos = 1 / Region3.v3_ph(P, H);
                    double Ts3 = Region3.T3_ph(P, H);
                    return _unit_converter.FromSIunit_Cp(Region3.Cp3_rhoT(rhos, Ts3));
                case 4:
                    _logger.LogWarning("Function Cp_ph is not available in region 4 for input p {0} and h {1}", P, H);
                    return double.NaN;
                case 5:
                    double Ts5 = Region5.T5_ph(P, H);
                    return _unit_converter.FromSIunit_Cp(Region5.Cp5_pT(P, Ts5));
                default:
                    _logger.LogWarning("Region switch Cp_ph returned unknown value {0} for input p {1} and h {2}", region, P, H);
                    return double.NaN;
            }
        }


        /// <summary>
        /// Specific isobaric heat capacity as a function of pressure and entropy.
        /// </summary>
        public double Cp_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(p, s);

            switch (region)
            {
                case 1:
                    double Ts1 = Region1.T1_ps(p, s);
                    return _unit_converter.FromSIunit_Cp(Region1.Cp1_pT(p, Ts1));
                case 2:
                    double Ts2 = Region2.T2_ps(p, s);
                    return _unit_converter.FromSIunit_Cp(Region2.Cp2_pT(p, Ts2));
                case 3:
                    double rhos = 1 / Region3.v3_ps(p, s);
                    double Ts3 = Region3.T3_ps(p, s);
                    return _unit_converter.FromSIunit_Cp(Region3.Cp3_rhoT(rhos, Ts3));
                case 4:
                    _logger.LogWarning(
                        "Function Cp_ps is not available in region 4 for input p {0} and s {1}",
                        p, s
                    );
                    return double.NaN;
                case 5:
                    double Ts5 = Region5.T5_ps(p, s);
                    return _unit_converter.FromSIunit_Cp(Region5.Cp5_pT(p, Ts5));
                default:
                    _logger.LogWarning(
                        "Region switch Cp_ps returned unknown value {0} for input p {1} and s {2}",
                        region, p, s
                    );
                    return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor isochoric heat capacity as a function of pressure.
        /// </summary>
        public double CvV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_Cv(
                        Region2.Cv2_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_Cv(
                        Region3.Cv3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4V_p(p)), Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid isochoric heat capacity as a function of pressure.
        /// </summary>
        public double CvL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_Cv(
                        Region1.Cv1_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_Cv(
                        Region3.Cv3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4L_p(p)), Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }


        /// <summary>
        /// Saturated vapor isochoric heat capacity as a function of temperature.
        /// </summary>
        public double CvV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_Cv(Region2.Cv2_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_Cv(
                        Region3.Cv3_rhoT(
                            1 / Region3.v3_ph(
                                Region4.p4_T(T), Region4.h4V_p(Region4.p4_T(T))
                            ),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid isochoric heat capacity as a function of temperature.
        /// </summary>
        public double CvL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_Cv(Region1.Cv1_pT(Region4.p4_T(T), T));
                }
                else
                {
                    return _unit_converter.FromSIunit_Cv(
                        Region3.Cv3_rhoT(
                            1 / Region3.v3_ph(
                                Region4.p4_T(T), Region4.h4L_p(Region4.p4_T(T))
                            ),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific isochoric heat capacity as a function of pressure and temperature.
        /// </summary>
        public double Cv_pt(double p, double t)
        {
            double P = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(P, T);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_Cv(Region1.Cv1_pT(P, T));
                case 2:
                    return _unit_converter.FromSIunit_Cv(Region2.Cv2_pT(P, T));
                case 3:
                    double hs = Region3.h3_pT(P, T);
                    double rhos = 1 / Region3.v3_ph(P, hs);
                    return _unit_converter.FromSIunit_Cv(Region3.Cv3_rhoT(rhos, T));
                case 4:
                    _logger.LogWarning(
                        "Function Cv_pt is not available in region 4 for input p {0} and T {1}",
                        P, T
                    );
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_Cv(Region5.Cv5_pT(P, T));
                default:
                    _logger.LogWarning(
                        "Region switch Cv_pt returned unknown value {0} for input p {1} and T {2}",
                        region, P, T
                    );
                    return double.NaN;
            }
        }


        /// <summary>
        /// Specific isochoric heat capacity as a function of pressure and enthalpy.
        /// </summary>
        public double Cv_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(p, h);
            if (region == 1)
            {
                double Ts = Region1.T1_ph(p, h);
                return _unit_converter.FromSIunit_Cv(Region1.Cv1_pT(p, Ts));
            }
            else if (region == 2)
            {
                double Ts = Region2.T2_ph(p, h);
                return _unit_converter.FromSIunit_Cv(Region2.Cv2_pT(p, Ts));
            }
            else if (region == 3)
            {
                double rhos = 1 / Region3.v3_ph(p, h);
                double Ts = Region3.T3_ph(p, h);
                return _unit_converter.FromSIunit_Cv(Region3.Cv3_rhoT(rhos, Ts));
            }
            else if (region == 4)
            {
                _logger.LogWarning("function Cv_ph is not available in region 4 for input p {0} and h {1}", p, h);
                return double.NaN;
            }
            else if (region == 5)
            {
                double Ts = Region5.T5_ph(p, h);
                return _unit_converter.FromSIunit_Cv(Region5.Cv5_pT(p, Ts));
            }
            else
            {
                _logger.LogWarning("Region switch Cv_ph returned unknown value {0} for input p {1} and h {2}", region, p, h);
                return double.NaN;
            }
        }

        /// <summary>
        /// Specific isochoric heat capacity as a function of pressure and entropy.
        /// </summary>
        public double Cv_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(p, s);
            if (region == 1)
            {
                double Ts = Region1.T1_ps(p, s);
                return _unit_converter.FromSIunit_Cv(Region1.Cv1_pT(p, Ts));
            }
            else if (region == 2)
            {
                double Ts = Region2.T2_ps(p, s);
                return _unit_converter.FromSIunit_Cv(Region2.Cv2_pT(p, Ts));
            }
            else if (region == 3)
            {
                double rhos = 1 / Region3.v3_ps(p, s);
                double Ts = Region3.T3_ps(p, s);
                return _unit_converter.FromSIunit_Cv(Region3.Cv3_rhoT(rhos, Ts));
            }
            else if (region == 4)
            {
                _logger.LogWarning("function Cv_ps is not available in region 4 for input p {0} and s {1}", p, s);
                return double.NaN;
            }
            else if (region == 5)
            {
                double Ts = Region5.T5_ps(p, s);
                return _unit_converter.FromSIunit_Cv(Region5.Cv5_pT(p, Ts));
            }
            else
            {
                _logger.LogWarning("Region switch Cv_ps returned unknown value {0} for input p {1} and s {2}", region, p, s);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor speed of sound as a function of pressure.
        /// </summary>
        public double wV_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_w(Region2.w2_pT(p, Region4.T4_p(p)));
                }
                else
                {
                    return _unit_converter.FromSIunit_w(
                        Region3.w3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4V_p(p)),
                            Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }


        /// <summary>
        /// Saturated liquid speed of sound as a function of pressure.
        /// </summary>
        public double wL_p(double p)
        {
            p = _unit_converter.ToSIunit_p(p);
            if (p > 0.000611657 && p < 22.06395)
            {
                if (p < 16.529)
                {
                    return _unit_converter.FromSIunit_w(
                        Region1.w1_pT(p, Region4.T4_p(p))
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_w(
                        Region3.w3_rhoT(
                            1 / Region3.v3_ph(p, Region4.h4L_p(p)),
                            Region4.T4_p(p)
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated vapor speed of sound as a function of temperature.
        /// </summary>
        public double wV_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_w(
                        Region2.w2_pT(Region4.p4_T(T), T)
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_w(
                        Region3.w3_rhoT(
                            1 / Region3.v3_ph(
                                Region4.p4_T(T),
                                Region4.h4V_p(Region4.p4_T(T))
                            ),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Saturated liquid speed of sound as a function of temperature.
        /// </summary>
        public double wL_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T > 273.15 && T < 647.096)
            {
                if (T <= 623.15)
                {
                    return _unit_converter.FromSIunit_w(
                        Region1.w1_pT(Region4.p4_T(T), T)
                    );
                }
                else
                {
                    return _unit_converter.FromSIunit_w(
                        Region3.w3_rhoT(
                            1 / Region3.v3_ph(
                                Region4.p4_T(T),
                                Region4.h4L_p(Region4.p4_T(T))
                            ),
                            T
                        )
                    );
                }
            }
            else
            {
                _logger.LogWarning("Temperature {0} out of range", T);
                return double.NaN;
            }
        }


        /// <summary>
        /// Speed of sound as a function of pressure and temperature.
        /// </summary>
        public double w_pt(double p, double t)
        {
            p = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(p, T);

            switch (region)
            {
                case 1:
                    return _unit_converter.FromSIunit_w(Region1.w1_pT(p, T));
                case 2:
                    return _unit_converter.FromSIunit_w(Region2.w2_pT(p, T));
                case 3:
                    double hs = Region3.h3_pT(p, T);
                    double rhos = 1 / Region3.v3_ph(p, hs);
                    return _unit_converter.FromSIunit_w(Region3.w3_rhoT(rhos, T));
                case 4:
                    _logger.LogWarning("function w_pt is not available in region 4 for input p {0} and T {1}", p, T);
                    return double.NaN;
                case 5:
                    return _unit_converter.FromSIunit_w(Region5.w5_pT(p, T));
                default:
                    _logger.LogWarning("Region switch w_pt returned unknown value {0} for input p {1} and T {2}", region, p, T);
                    return double.NaN;
            }
        }

        /// <summary>
        /// Speed of sound as a function of pressure and enthalpy.
        /// </summary>
        public double w_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(p, h);

            switch (region)
            {
                case 1:
                    double Ts1 = Region1.T1_ph(p, h);
                    return _unit_converter.FromSIunit_w(Region1.w1_pT(p, Ts1));
                case 2:
                    double Ts2 = Region2.T2_ph(p, h);
                    return _unit_converter.FromSIunit_w(Region2.w2_pT(p, Ts2));
                case 3:
                    double rhos3 = 1 / Region3.v3_ph(p, h);
                    double Ts3 = Region3.T3_ph(p, h);
                    return _unit_converter.FromSIunit_w(Region3.w3_rhoT(rhos3, Ts3));
                case 4:
                    _logger.LogWarning("function w_ph is not available in region 4 for input p {0} and h {1}", p, h);
                    return double.NaN;
                case 5:
                    double Ts5 = Region5.T5_ph(p, h);
                    return _unit_converter.FromSIunit_w(Region5.w5_pT(p, Ts5));
                default:
                    _logger.LogWarning("Region switch w_ph returned unknown value {0} for input p {1} and h {2}", region, p, h);
                    return double.NaN;
            }
        }

        /// <summary>
        /// Speed of sound as a function of pressure and entropy.
        /// </summary>
        public double w_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);
            int region = RegionSelection.Region_ps(p, s);

            switch (region)
            {
                case 1:
                    double Ts1 = Region1.T1_ps(p, s);
                    return _unit_converter.FromSIunit_w(Region1.w1_pT(p, Ts1));
                case 2:
                    double Ts2 = Region2.T2_ps(p, s);
                    return _unit_converter.FromSIunit_w(Region2.w2_pT(p, Ts2));
                case 3:
                    double rhos3 = 1 / Region3.v3_ps(p, s);
                    double Ts3 = Region3.T3_ps(p, s);
                    return _unit_converter.FromSIunit_w(Region3.w3_rhoT(rhos3, Ts3));
                case 4:
                    _logger.LogWarning("function w_ps is not available in region 4 for input p {0} and s {1}", p, s);
                    return double.NaN;
                case 5:
                    double Ts5 = Region5.T5_ps(p, s);
                    return _unit_converter.FromSIunit_w(Region5.w5_pT(p, Ts5));
                default:
                    _logger.LogWarning("Region switch w_ps returned unknown value {0} for input p {1} and s {2}", region, p, s);
                    return double.NaN;
            }
        }

        /// <summary>
        /// Viscosity as a function of pressure and temperature.
        /// </summary>
        public double my_pt(double p, double t)
        {
            p = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            int region = RegionSelection.Region_pT(p, T);

            if (region == 4)
            {
                _logger.LogWarning(
                    "Function my_pt is not available in region 4 for input p {0} and T {1}",
                    p, T);
                return double.NaN;
            }
            else if (region == 1 || region == 2 || region == 3 || region == 5)
            {
                return _unit_converter.FromSIunit_my(
                    TransportProperties.my_AllRegions_pT(p, T));
            }
            else
            {
                _logger.LogWarning(
                    "Region switch my_pt returned unknown value {0} for input p {1} and T {2}",
                    region, p, T);
                return double.NaN;
            }
        }

        /// <summary>
        /// Viscosity as a function of pressure and enthalpy.
        /// </summary>
        public double my_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);
            int region = RegionSelection.Region_ph(p, h);

            if (region == 1 || region == 2 || region == 3 || region == 5)
            {
                return _unit_converter.FromSIunit_my(
                    TransportProperties.my_AllRegions_ph(p, h));
            }
            else if (region == 4)
            {
                _logger.LogWarning(
                    "Function my_ph is not available in region 4 for input p {0} and h {1}",
                    p, h);
                return double.NaN;
            }
            else
            {
                _logger.LogWarning(
                    "Region switch my_ph returned unknown value {0} for input p {1} and h {2}",
                    region, p, h);
                return double.NaN;
            }
        }

        /// <summary>
        /// Viscosity as a function of pressure and entropy.
        /// </summary>
        public double my_ps(double p, double s)
        {
            double h = h_ps(p, s);
            return my_ph(p, h);
        }

        /// <summary>
        /// Prandtl number as a function of pressure and temperature.
        /// </summary>
        public double pr_pt(double p, double t)
        {
            double Cp = _unit_converter.ToSIunit_Cp(Cp_pt(p, t));
            double my = _unit_converter.ToSIunit_my(my_pt(p, t));
            double tc = _unit_converter.ToSIunit_tc(tc_pt(p, t));

            return Cp * 1000 * my / tc;
        }

        /// <summary>
        /// Prandtl number as a function of pressure and enthalpy.
        /// </summary>
        public double pr_ph(double p, double h)
        {
            double Cp = _unit_converter.ToSIunit_Cp(Cp_ph(p, h));
            double my = _unit_converter.ToSIunit_my(my_ph(p, h));
            double tc = _unit_converter.ToSIunit_tc(tc_ph(p, h));

            return Cp * 1000 * my / tc;
        }

        /// <summary>
        /// Surface tension for two-phase water/steam as a function of temperature.
        /// </summary>
        public double st_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            return _unit_converter.FromSIunit_st(
                TransportProperties.Surface_Tension_T(T)
            );
        }

        /// <summary>
        /// Surface tension for two-phase water/steam as a function of pressure.
        /// </summary>
        public double st_p(double p)
        {
            double T = tsat_p(p);
            T = _unit_converter.ToSIunit_T(T);
            return _unit_converter.FromSIunit_st(
                TransportProperties.Surface_Tension_T(T)
            );
        }

        /// <summary>
        /// Saturated liquid thermal conductivity as a function of pressure.
        /// </summary>
        public double tcL_p(double p)
        {
            double t = tsat_p(p);
            double v = vL_p(p);
            p = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(t);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1.0 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(p, T, rho)
            );
        }

        /// <summary>
        /// Saturated vapour thermal conductivity as a function of pressure.
        /// </summary>
        public double tcV_p(double p)
        {
            double ps = p;
            double T = tsat_p(p);
            double v = vV_p(ps);
            p = _unit_converter.ToSIunit_p(p);
            T = _unit_converter.ToSIunit_T(T);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1.0 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(p, T, rho)
            );
        }

        /// <summary>
        /// Saturated liquid thermal conductivity as a function of temperature.
        /// </summary>
        public double tcL_t(double t)
        {
            double Ts = t;
            double p = psat_t(Ts);
            double v = vL_t(Ts);
            p = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(Ts);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1.0 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(p, T, rho)
            );
        }

        /// <summary>
        /// Saturated vapour thermal conductivity as a function of temperature.
        /// </summary>
        public double tcV_t(double t)
        {
            double Ts = t;
            double p = psat_t(Ts);
            double v = vV_t(Ts);
            p = _unit_converter.ToSIunit_p(p);
            double T = _unit_converter.ToSIunit_T(Ts);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1.0 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(p, T, rho)
            );
        }

        /// <summary>
        /// Thermal conductivity as a function of pressure and temperature.
        /// </summary>
        public double tc_pt(double p, double t)
        {
            double Ts = t;
            double ps = p;
            double v = v_pt(ps, Ts);
            p = _unit_converter.ToSIunit_p(ps);
            double T = _unit_converter.ToSIunit_T(Ts);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(p, T, rho)
            );
        }

        /// <summary>
        /// Thermal conductivity as a function of pressure and enthalpy.
        /// </summary>
        public double tc_ph(double p, double h)
        {
            double hs = h;
            double ps = p;
            double v = v_ph(ps, hs);
            double T = t_ph(ps, hs);
            p = _unit_converter.ToSIunit_p(ps);
            T = _unit_converter.ToSIunit_T(T);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(p, T, rho)
            );
        }

        /// <summary>
        /// Thermal conductivity as a function of enthalpy and entropy.
        /// </summary>
        public double tc_hs(double h, double s)
        {
            double hs = h;
            double ps = p_hs(hs, s);
            double v = v_ph(ps, hs);
            double T = t_ph(ps, hs);
            ps = _unit_converter.ToSIunit_p(ps);
            T = _unit_converter.ToSIunit_T(T);
            v = _unit_converter.ToSIunit_v(v);
            double rho = 1 / v;

            return _unit_converter.FromSIunit_tc(
                TransportProperties.tc_ptrho(ps, T, rho)
            );
        }

        /// <summary>
        /// Vapour fraction as a function of pressure and enthalpy.
        /// </summary>
        public double x_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);

            if (p > 0.000611657 && p < 22.06395)
            {
                return _unit_converter.FromSIunit_x(Region4.x4_ph(p, h));
            }
            else
            {
                _logger.LogWarning("Pressure out of range");
                return double.NaN;
            }
        }

        /// <summary>
        /// Vapour fraction as a function of pressure and entropy.
        /// </summary>
        public double x_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);

            if (p > 0.000611657 && p < 22.06395)
            {
                return _unit_converter.FromSIunit_x(Region4.x4_ps(p, s));
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Vapour volume fraction as a function of pressure and enthalpy.
        /// </summary>
        public double vx_ph(double p, double h)
        {
            p = _unit_converter.ToSIunit_p(p);
            h = _unit_converter.ToSIunit_h(h);

            if (p > 0.000611657 && p < 22.06395)
            {
                double vL, vV;

                if (p < 16.529)
                {
                    vL = Region1.v1_pT(p, Region4.T4_p(p));
                    vV = Region2.v2_pT(p, Region4.T4_p(p));
                }
                else
                {
                    vL = Region3.v3_ph(p, Region4.h4L_p(p));
                    vV = Region3.v3_ph(p, Region4.h4V_p(p));
                }

                double xs = Region4.x4_ph(p, h);
                return _unit_converter.FromSIunit_vx(xs * vV / (xs * vV + (1 - xs) * vL));
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Vapour volume fraction as a function of pressure and entropy.
        /// </summary>
        public double vx_ps(double p, double s)
        {
            p = _unit_converter.ToSIunit_p(p);
            s = _unit_converter.ToSIunit_s(s);

            if (p > 0.000611657 && p < 22.06395)
            {
                double vL, vV;

                if (p < 16.529)
                {
                    vL = Region1.v1_pT(p, Region4.T4_p(p));
                    vV = Region2.v2_pT(p, Region4.T4_p(p));
                }
                else
                {
                    vL = Region3.v3_ph(p, Region4.h4L_p(p));
                    vV = Region3.v3_ph(p, Region4.h4V_p(p));
                }

                double xs = Region4.x4_ps(p, s);
                return _unit_converter.FromSIunit_vx(xs * vV / (xs * vV + (1 - xs) * vL));
            }
            else
            {
                _logger.LogWarning("Pressure {0} out of range", p);
                return double.NaN;
            }
        }

        /// <summary>
        /// Pressure along the melting curve as a function of temperature.
        /// Based on IAPWS R14-08(2011).
        /// </summary>
        public double pmelt_t(double t, int? hint = null)
        {
            double T = _unit_converter.ToSIunit_T(t);

            if (hint == null)
            {
                if (T >= 251.165 && T < 256.164)
                {
                    _logger.LogWarning($"Can't select ice type based on temperature {T}, hint required");
                    return double.NaN;
                }
                else if (T >= 256.164 && T < 273.31)
                {
                    _logger.LogDebug("Chose ice type V based on temperature");
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceV(T));
                }
                else if (T >= 273.31 && T < 355)
                {
                    _logger.LogDebug("Chose ice type VI based on temperature");
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceVI(T));
                }
                else if (T >= 355 && T < 751)
                {
                    _logger.LogDebug("Chose ice type VII based on temperature");
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceVII(T));
                }
                else
                {
                    _logger.LogWarning($"Temperature {T} out of range");
                    return double.NaN;
                }
            }
            else if (hint == TYPE_ICE_Ih)
            {
                if (T >= 251.165 && T < 273.16)
                {
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceIh(T));
                }
                else
                {
                    _logger.LogWarning($"Temperature {T} out of range");
                    return double.NaN;
                }
            }
            else if (hint == TYPE_ICE_III)
            {
                if (T >= 251.165 && T < 256.164)
                {
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceIII(T));
                }
                else
                {
                    _logger.LogWarning($"Temperature {T} out of range");
                    return double.NaN;
                }
            }
            else if (hint == TYPE_ICE_V)
            {
                if (T >= 256.164 && T < 273.31)
                {
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceV(T));
                }
                else
                {
                    _logger.LogWarning($"Temperature {T} out of range");
                    return double.NaN;
                }
            }
            else if (hint == TYPE_ICE_VI)
            {
                if (T >= 273.31 && T < 355)
                {
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceVI(T));
                }
                else
                {
                    _logger.LogWarning($"Temperature {T} out of range");
                    return double.NaN;
                }
            }
            else if (hint == TYPE_ICE_VII)
            {
                if (T >= 355 && T < 751)
                {
                    return _unit_converter.FromSIunit_p(IAPWS_R14.pmelt_T_iceVII(T));
                }
                else
                {
                    _logger.LogWarning($"Temperature {T} out of range");
                    return double.NaN;
                }
            }
            else
            {
                _logger.LogError($"Unknown value for parameter 'hint' {hint}, can't select ice type");
                throw new ArgumentException("Unknown value for parameter 'hint'");
            }
        }

        /// <summary>
        /// Pressure along the sublimation curve as a function of temperature.
        /// Based on IAPWS R14-08(2011).
        /// </summary>
        public double psubl_t(double t)
        {
            double T = _unit_converter.ToSIunit_T(t);
            if (T >= 50 && T < 273.16)
            {
                return _unit_converter.FromSIunit_p(IAPWS_R14.psubl_T(T));
            }
            else
            {
                _logger.LogWarning($"Temperature {T} out of range");
                return double.NaN;
            }
        }
    }
}