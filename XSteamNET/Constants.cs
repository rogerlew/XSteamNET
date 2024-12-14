
namespace XSteamNET
{
    /// <summary>
    /// Constants for the calculation of water steam properties.
    /// Sources:
    /// * IAPWS Industrial formulation 1997 for the Thermodynamic Properties of Water and Steam, September 1997.
    /// * IAWPS Release on Values of Temperature, Pressure, and Density of Ordinary and Heavy Water Substances
    ///   at their Respective Critical Points, Released September 1992, Revision of the Release of 1992.
    /// </summary>
    public static class Constants
    {
        public static readonly double SPECIFIC_GAS_CONSTANT = 0.461526; // kJ kg^-1 K^-1
        public static readonly double CRITICAL_TEMPERATURE = 647.096; // K
        public static readonly double CRITICAL_PRESSURE = 22.06395; // MPa
        public static readonly double CRITICAL_DENSITY = 322.0; // kg m^-3
        public static readonly double TRIPLE_POINT_TEMPERATURE = 273.16; // K (Eq9 Page 7)
        public static readonly double TRIPLE_POINT_PRESSURE = 0.000611657; // MPa (Eq9 Page 7)
        public static readonly double TRIPLE_POINT_SPECIFIC_ENTHALPY = 0.611783e-3; // kJ kg^-1 (Eq10 Page 7)

        // IAWPS Release 1992
        public static readonly double CRITICAL_TEMPERATURE_H20_1992 = 647.096; // +-0.1 K
        public static readonly double CRITICAL_PRESSURE_H20_1992 = 22.067; // + 0.27*(+-0.1)+-0.005 MPa
        public static readonly double CRITICAL_DENSITY_H20_1992 = 322.0; // +-3 kg m^-3

        public static readonly double CRITICAL_TEMPERATURE_D20_1992 = 643.847; // +-0.2 K
        public static readonly double CRITICAL_PRESSURE_D20_1992 = 21.671; // + 0.27*(+-0.2)+-0.01 MPa
        public static readonly double CRITICAL_DENSITY_D20_1992 = 356.0; // +-5 kg m^-3

        // Other common constants used in calculations
        public static readonly double ABSOLUTE_ZERO_CELSIUS = -273.15; // °C
        public static readonly double ABSOLUTE_ZERO_FAHRENHEIT = -459.67; // °F

        // IAPWS R15-11
        public static readonly double SPECIFIC_GAS_CONSTANT_IAPWS_R15_11 = 0.46151805; // kJ kg^-1 K^-1
        public static readonly double CRITICAL_TEMPERATURE_IAPWS_R15_11 = 647.096; // K
        public static readonly double CRITICAL_DENSITY_IAPWS_R15_11 = 322.0; // kg m^-3
    }
}

