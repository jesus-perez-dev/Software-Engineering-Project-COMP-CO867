using System;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public static class Extensions
    {
        public static string ToStringAll(this SDataValue value, string separator)
        {
            return string.Join(separator,
                new string[]
                {
                    $"Bool = {value.Bool}",
                    $"Char = {value.Char}",
                    $"Double = {value.Double}",
                    $"Float = {value.Float}",
                    $"Int8 = {value.Int8}",
                    $"Int16 = {value.Int16}",
                    $"Int32 = {value.Int32}",
                    $"Int64 = {value.Int64}",
                    $"UInt8 = {value.UInt8}",
                    $"UInt16 = {value.UInt16}",
                    $"UInt32 = {value.UInt32}",
                    $"UInt64 = {value.UInt64}",
                    $"WChar = {value.WChar}",
                    $"Type = {value.Type}",
                }
            );
        }

        public static string ToHumanString(this EPrimitiveDataType t)
        {
            return Enum.GetName(typeof(EPrimitiveDataType), t) ?? "(Unknown Type)";
        }
    }
}