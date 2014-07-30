using SmartQuant.FIXApplication;
using System;
using System.ComponentModel;
using System.Globalization;

namespace SmartQuant.FIXApplication.Design
{
	class SessionsTypeConverter : ArrayConverter
	{
		public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is SessionInfo[] && destinationType == typeof(string))
				return string.Format ("{0} session(s)", ((SessionInfo[])value).Length);
			else
				return base.ConvertTo (context, culture, value, destinationType);
		}
	}
}
