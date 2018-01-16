using System;
using System.Drawing;
using System.Globalization;

namespace Yata.CoreNode.DataTypes
{
    internal class BoolSerializer : DataTypeSerializer
    {
        public override object Deserialize(string s) { return s.ToLower() == "true"; }
        public override string Serialize(object o) { return ((bool)o) ? "True" : "False"; }
    }

    internal class IntSerializer : DataTypeSerializer
    {
        public override object Deserialize(string s) { return Int32.Parse(s); }
        public override string Serialize(object o) { return o.ToString(); }
    }

    internal class FloatSerializer : DataTypeSerializer
    {
        public override object Deserialize(string s) { return float.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture); }
        public override string Serialize(object o) { return ((float)o).ToString(CultureInfo.InvariantCulture); }
    }

    internal class StringSerializer : DataTypeSerializer
    {
        public override object Deserialize(string s) { return s; }
        public override string Serialize(object o) { return (string)o; }
    }

    internal class ColorSerializer : DataTypeSerializer
    {
        public override object Deserialize(string s)
        {
            string[] c = s.Split(',');
            return Color.FromArgb(byte.Parse(c[3]), byte.Parse(c[0]), byte.Parse(c[1]), byte.Parse(c[2]));
        }
        public override string Serialize(object o)
        {
            Color c = (Color)o;
            return c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + "," + c.A.ToString();
        }
    }
}
