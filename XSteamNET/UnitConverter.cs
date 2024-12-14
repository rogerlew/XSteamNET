using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace XSteamNET
{
    public class UnitConverter
    {
        // Constants for unit systems
        public const int __UNIT_SYSTEM_BARE__ = 0; // m/kg/sec/K/MPa/W
        public const int __UNIT_SYSTEM_MKS__ = 1;  // m/kg/sec/°C/bar/W
        public const int __UNIT_SYSTEM_FLS__ = 2;  // ft/lb/sec/°F/psi/btu

        public int _unitSystem;
        public static readonly double __ABSOLUTE_ZERO_CELSIUS__ = -273.15;

        public UnitConverter(int unitSystem = __UNIT_SYSTEM_BARE__)
        {
            SetUnitSystem(unitSystem);
            Debug.WriteLine($"Set unit converter to {ToString()}");
        }

        public void SetUnitSystem(int unitSystem)
        {
            if (unitSystem == __UNIT_SYSTEM_BARE__ || unitSystem == __UNIT_SYSTEM_MKS__ || unitSystem == __UNIT_SYSTEM_FLS__)
            {
                _unitSystem = unitSystem;
            }
            else
            {
                throw new ArgumentException("Unknown Unit System selected");
            }
        }

        public double ToSIunit_p(double input)
        {
            return _unitSystem switch
            {
                __UNIT_SYSTEM_MKS__ => input / 10.0, // bar to MPa
                __UNIT_SYSTEM_FLS__ => input * 0.00689475729, // psi to MPa
                _ => input
            };
        }

        public double FromSIunit_p(double input)
        {
            return _unitSystem switch
            {
                __UNIT_SYSTEM_MKS__ => input * 10.0, // MPa to bar
                __UNIT_SYSTEM_FLS__ => input / 0.00689475729, // MPa to psi
                _ => input
            };
        }

        public double ToSIunit_T(double input)
        {
            return _unitSystem switch
            {
                __UNIT_SYSTEM_MKS__ => input - __ABSOLUTE_ZERO_CELSIUS__, // °C to Kelvin
                __UNIT_SYSTEM_FLS__ => (5.0 / 9.0) * (input - 32) - __ABSOLUTE_ZERO_CELSIUS__, // °F to Kelvin
                _ => input
            };
        }
        public double FromSIunit_T(double ins)
        {
            // function fromSIunit_T = fromSIunit_T( ins )
            if (_unitSystem == __UNIT_SYSTEM_MKS__)
            {
                return ins + Constants.ABSOLUTE_ZERO_CELSIUS; // Kelvin to degC
            }
            else if (_unitSystem == __UNIT_SYSTEM_FLS__)
            {
                return (ins + Constants.ABSOLUTE_ZERO_CELSIUS) * (9.0 / 5.0) + 32; // Kelvin to degF
            }
            return ins;
        }

        public double ToSIunit_h(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input * 2.32600 : input; // btu/lb to kJ/kg
        }

        public double FromSIunit_h(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input / 2.32600 : input; // kJ/kg to btu/lb
        }

        public double ToSIunit_v(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input * 0.0624279606 : input; // ft^3/lb to m^3/kg
        }

        public double FromSIunit_v(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input / 0.0624279606 : input; // m^3/kg to ft^3/lb
        }

        public double ToSIunit_s(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input / 0.238845896627 : input; // btu/(lb°F) to kJ/(kg°C)
        }

        public double FromSIunit_s(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input * 0.238845896627 : input; // kJ/(kg°C) to btu/(lb°F)
        }

        public double ToSIunit_my(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input / 2419.088311 : input; // lbm/ft/hr to Pa·s
        }

        public double FromSIunit_my(double input)
        {
            return _unitSystem == __UNIT_SYSTEM_FLS__ ? input * 2419.088311 : input; // Pa·s to lbm/ft/hr
        }

        public double ToSIunit_u(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins * 2.32600; // btu/lb to kJ/kg
            return ins;
        }

        public double FromSIunit_u(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins / 2.32600; // kJ/kg to btu/lb
            return ins;
        }

        public double ToSIunit_Cp(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins / 0.238846; // btu/(lb degF) to kJ/(kg degC)
            return ins;
        }

        public double FromSIunit_Cp(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins * 0.238846; // kJ/(kg degC) to btu/(lb degF)
            return ins;
        }

        public double ToSIunit_Cv(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins / 0.238846; // btu/(lb degF) to kJ/(kg degC)
            return ins;
        }

        public double FromSIunit_Cv(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins * 0.238846; // kJ/(kg degC) to btu/(lb degF)
            return ins;
        }

        public double ToSIunit_w(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins * 0.3048; // ft/s to m/s
            return ins;
        }

        public double FromSIunit_w(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins / 0.3048; // m/s to ft/s
            return ins;
        }

        public double ToSIunit_tc(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins / 0.577789; // btu/(h*ft*degF) to W/(m*degC)
            return ins;
        }

        public double FromSIunit_tc(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins * 0.577789; // W/(m*degC) to btu/(h*ft*degF)
            return ins;
        }

        public double ToSIunit_st(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins / 0.068521766; // lb/ft to N/m
            return ins;
        }

        public double FromSIunit_st(double ins)
        {
            if (_unitSystem == __UNIT_SYSTEM_FLS__)
                return ins * 0.068521766; // N/m to lb/ft
            return ins;
        }

        public double ToSIunit_x(double ins)
        {
            if (ins >= 0.0 && ins <= 1.0)
                return ins;
            Console.WriteLine("Value of vapour fraction out of range: 0 <= x <= 1");
            throw new ArgumentOutOfRangeException("Vapour fraction out of range");
        }

        public double FromSIunit_x(double ins)
        {
            if (ins >= 0.0 && ins <= 1.0)
                return ins;
            Console.WriteLine("Value of vapour fraction out of range: 0 <= x <= 1");
            throw new ArgumentOutOfRangeException("Vapour fraction out of range");
        }

        public double ToSIunit_vx(double ins)
        {
            if (ins >= 0.0 && ins <= 1.0)
                return ins;
            Console.WriteLine("Value of vapour volume fraction out of range: 0 <= x <= 1");
            throw new ArgumentOutOfRangeException("Vapour volume fraction out of range");
        }

        public double FromSIunit_vx(double ins)
        {
            if (ins >= 0.0 && ins <= 1.0)
                return ins;
            Console.WriteLine("Value of vapour volume fraction out of range: 0 <= x <= 1");
            throw new ArgumentOutOfRangeException("Vapour volume fraction out of range");
        }
    }
}
