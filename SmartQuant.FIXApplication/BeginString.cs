using System.Collections.Generic;
using System.Reflection;

namespace SmartQuant.FIXApplication
{
	public class BeginString
	{
		public const string FIX40 = "FIX.4.0";
		public const string FIX41 = "FIX.4.1";
		public const string FIX42 = "FIX.4.2";
		public const string FIX43 = "FIX.4.3";
		public const string FIX44 = "FIX.4.4";

		public static string[] GetValues ()
		{
			List<string> list = new List<string> ();
			foreach (FieldInfo fieldInfo in typeof (BeginString).GetFields(BindingFlags.Static | BindingFlags.Public))
				list.Add ((string)fieldInfo.GetValue ((object)null));
			return list.ToArray ();
		}
	}
}
