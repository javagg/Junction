using SmartQuant.FIXApplication;
using System.ComponentModel;

namespace SmartQuant.TT
{
	class TTSessionInfo : SessionInfo
	{
		private const string CATEGORY_TT = "TT";
		private const bool DEFAULT_RESET_SEQUENCE = true;

		[PasswordPropertyText (true)]
		[Category ("TT")]
		[Description ("Password as associated with this FIX session")]
		public string Password { get; set; }

		[DefaultValue (true)]
		[Description ("Indicates that the both sides of the order session should reset sequence numbers")]
		[Category ("TT")]
		public bool ResetSequence { get; set; }

		public TTSessionInfo ()
		{
			this.Password = string.Empty;
			this.ResetSequence = true;
		}
	}
}
