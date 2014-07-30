
using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class TypeInfo : InfoBase
	{
		protected const InfoType DEFAULT_TYPE = InfoType.FILE;

		[DefaultValue (InfoType.FILE)]
		public InfoType Type { get; set; }

		protected TypeInfo ()
		{
			this.Type = InfoType.FILE;
		}

		public override string ToString ()
		{
			return this.Type.ToString ();
		}
	}
}
