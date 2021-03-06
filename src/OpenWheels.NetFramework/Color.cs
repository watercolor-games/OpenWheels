﻿using System;
using System.Runtime.InteropServices;
#if NETSTANDARD2_0
using System.ComponentModel;
using System.Runtime.Serialization;
#endif

namespace OpenWheels
{
    /// <summary>
    /// Value type representing an ABGR color with 1 byte per channel (values in [0-255]).
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
#if NETSTANDARD2_0
    [TypeConverter(typeof(ColorConverter))]
    [DataContract]
#endif
    public partial struct Color
    {
        /// <summary>
        /// Red channel value of the color.
        /// </summary>
        [FieldOffset(0)]
        public readonly byte R;

        /// <summary>
        /// Green channel value of the color.
        /// </summary>
        [FieldOffset(1)]
        public readonly byte G;

        /// <summary>
        /// Blue channel value of the color.
        /// </summary>
        [FieldOffset(2)]
        public readonly byte B;

        /// <summary>
        /// Alpha channel value of the color.
        /// </summary>
        [FieldOffset(3)]
        public readonly byte A;
        
        /// <summary>
        /// Value of the 4 channels packed together. Values are ordered ABGR with A as the most significant byte.
        /// </summary>
        [FieldOffset(0)]
#if NETSTANDARD2_0
        [DataMember]
#endif
        public readonly uint Packed;

        /// <summary>
        /// Create a new color with the given r, g, b and a values.
        /// Values should be in the range [0, 255].
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value. Defaults to opaque (255).</param>
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            Packed = 0;
            R = r;
            G = g;
            B = b;
            A = a;
        }
        
        /// <summary>
        /// Create a new color with the given r, g, b and a values.
        /// Values should be in the range [0, 255].
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value. Defaults to opaque (255).</param>
        public Color(int r, int g, int b, int a = 255)
        {
            Packed = 0;
            R = (byte) MathHelper.Clamp(r, 0, 255);
            G = (byte) MathHelper.Clamp(g, 0, 255);
            B = (byte) MathHelper.Clamp(b, 0, 255);
            A = (byte) MathHelper.Clamp(a, 0, 255);
        }

        /// <summary>
        /// Create a new color with the given r, g, b and a values.
        /// Values should be in the range [0, 1].
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value. Defaults to opaque (1f).</param>
        public Color(float r, float g, float b, float a = 1f)
            : this((int) (r * 255), (int) (g * 255), (int) (b * 255), (int) (a * 255))
        {
        }

        /// <summary>
        /// Create a new color with the given packed value.
        /// The packed value is ordered ABGR with A at the most significant byte.
        /// </summary>
        /// <param name="packed">The packed value of this color.</param>
        public Color(uint packed)
        {
            R = G = B = A = 0;
            Packed = packed;
        }

        /// <summary>
        /// Parse a <see cref="Color"/> from a <see cref="string"/>.
        /// Expected format is 3 or 4 byte values separated by any number of ',' or ' '.
        /// Values are parsed as R, G, B, A in that order. Alpha is optional.
        /// All of the following are valid:
        /// - "212, 120, 27"
        /// - "100, 232, 242, 250"
        /// - "0 255 255 255"
        /// - "   0, , ,, 8, ,  , 210,    255"
        /// </summary>
        /// <param name="str">String to parse.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="FormatException">If the given string does not match the expected format.</exception>
        public static Color Parse(string str)
        {
            var s = str.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length < 3)
                throw new FormatException("Expected at least 3 parts separated by either ',' or ' ' (or both).");
            if (s.Length > 4)
                throw new FormatException("Expected at most 4 parts separated by either ',' or ' ' (or both).");
            var r = byte.Parse(s[0]);
            var g = byte.Parse(s[1]);
            var b = byte.Parse(s[2]);
            var a = s.Length == 3 ? (byte) 255 : byte.Parse(s[3]);
            return new Color(r, g, b, a);
        }

        public override string ToString()
        {
            var aStr = A == 255 ? string.Empty : ", " + A;
            return $"{R}, {G}, {B}{aStr}";
        }
    }
}
