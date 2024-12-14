using System;
using Microsoft.Extensions.Logging;


namespace XSteamNET
{
    /// <summary>
    /// Main class for the heavy water parts of pyXSteam
    /// Abstract of all other functions to allow auto selection of
    /// the correct region for each set of parameters.
    /// </summary>
    public class XSteam_HW
    {
        public static readonly int UNIT_SYSTEM_BARE = UnitConverter.__UNIT_SYSTEM_BARE__;
        public static readonly int UNIT_SYSTEM_MKS = UnitConverter.__UNIT_SYSTEM_MKS__;
        public static readonly int UNIT_SYSTEM_FLS = UnitConverter.__UNIT_SYSTEM_FLS__;

        private readonly ILogger<XSteam_HW> _logger;
        private readonly UnitConverter _unitConverter;

        public XSteam_HW(int unitSystem = UnitConverter.__UNIT_SYSTEM_BARE__)
        {
            _logger = new LoggerFactory().CreateLogger<XSteam_HW>();
            _unitConverter = new UnitConverter(unitSystem);
            _logger.LogInformation("Initialized pyXSteam for Heavy Water with Unit System {UnitSystem}", unitSystem);
        }

        public double CriticalTemperature()
        {
            return _unitConverter.FromSIunit_T(Constants.CRITICAL_TEMPERATURE_D20_1992);
        }

        public double CriticalPressure()
        {
            return _unitConverter.FromSIunit_p(Constants.CRITICAL_PRESSURE_D20_1992);
        }

        public double CriticalDensity()
        {
            return _unitConverter.FromSIunit_p(Constants.CRITICAL_DENSITY_D20_1992);
        }

        public double MyRhoT(double rho, double T)
        {
            rho = _unitConverter.ToSIunit_p(rho);
            T = _unitConverter.ToSIunit_T(T);

            if (T < 277.0 || T > 775.0)
            {
                _logger.LogError("Temperature out of range");
                return double.NaN;
            }

            _logger.LogWarning("Input for density wasn't checked!");

            return _unitConverter.FromSIunit_T(IAPWS_R4.MyHW_RhoT_R4(rho, T));
        }

        public double TcRhoT(double rho, double T)
        {
            rho = _unitConverter.ToSIunit_p(rho);
            T = _unitConverter.ToSIunit_T(T);

            if (T < 277.0 || T > 825.0)
            {
                _logger.LogError("Temperature out of range");
                return double.NaN;
            }

            _logger.LogWarning("Input for density wasn't checked!");

            return _unitConverter.FromSIunit_tc(IAPWS_R4.TcHW_RhoT_R4(rho, T));
        }
    }
}
