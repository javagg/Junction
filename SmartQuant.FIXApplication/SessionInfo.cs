using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	[TypeConverter (typeof(ExpandableObjectConverter))]
	public class SessionInfo
	{
		public const string CATEGORY_TYPE = "Type";
		public const string CATEGORY_SESSION_ID = "Session ID";
		public const string CATEGORY_INITIATOR = "Initiator";
		public const string CATEGORY_BEHAVIOR = "Behavior";
		public const bool DEFAULT_IS_ENABLED = true;
		public const SessionType DEFAULT_SESSION_TYPE = SessionType.UNDEFINED;
		public const string DEFAULT_SOCKET_CONNECT_HOST = "localhost";
		public const uint DEFAULT_SOCKET_CONNECT_PORT = 0U;

		[DefaultValue (true)]
		[Category ("Behavior")]
		[Description ("Indicates whether the session is enabled")]
		public bool IsEnabled { get; set; }

		[DefaultValue (SessionType.UNDEFINED)]
		[Category ("Type")]
		public SessionType Type { get; set; }

		[Description ("Your ID as associated with this FIX session")]
		[Category ("Session ID")]
		public string SenderCompID { get; set; }

		[Description ("Counter parties ID as associated with this FIX session")]
		[Category ("Session ID")]
		public string TargetCompID { get; set; }

		[DefaultValue ("localhost")]
		[Category ("Initiator")]
		[Description ("Host to connect to")]
		public string SocketConnectHost { get; set; }

		[Category ("Initiator")]
		[Description ("Socket port for connecting to a session")]
		[DefaultValue (0L)]
		public uint SocketConnectPort { get; set; }

		public SessionInfo ()
		{
			this.IsEnabled = true;
			this.Type = SessionType.UNDEFINED;
			this.SenderCompID = string.Empty;
			this.TargetCompID = string.Empty;
			this.SocketConnectHost = "localhost";
			this.SocketConnectPort = 0U;
		}

		public override string ToString ()
		{
			return string.Format ("{0} {1}->{2}{3}", this.Type, this.SenderCompID, this.TargetCompID, this.IsEnabled ? string.Empty : " (disabled)");
		}
	}
}
