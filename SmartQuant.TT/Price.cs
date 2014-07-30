using System;

namespace SmartQuant.TT
{
	internal struct Price : IComparable<Price>
	{
		private Decimal value;

		public Price (Decimal value)
		{
			this.value = value;
		}

		public Price (double value)
		{
			this = new Price ((Decimal)value);
		}

		public static explicit operator Price (double value)
		{
			return new Price (value);
		}

		public static explicit operator Price (Decimal value)
		{
			return new Price (value);
		}

		public static implicit operator double (Price value)
		{
			return Decimal.ToDouble (value.value);
		}

		public static Price operator * (Price valueX, Decimal valueY)
		{
			return new Price (Decimal.Multiply (valueX.value, valueY));
		}

		public static Price operator / (Price valueX, Decimal valueY)
		{
			return new Price (Decimal.Divide (valueX.value, valueY));
		}

		public int CompareTo (Price other)
		{
			return Decimal.Compare (this.value, other.value);
		}

		public override string ToString ()
		{
			return this.value.ToString ();
		}
	}
}
