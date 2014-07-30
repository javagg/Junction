using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class MySQLLoggingInfo : MySQLInfoBase
	{
		protected const string DEFAULT_INCOMING_TABLE = "message_log";
		protected const string DEFAULT_OUTGOING_TABLE = "message_log";
		protected const string DEFAULT_EVENT_TABLE = "event_log";

		[DefaultValue ("quickfix")]
		[Description ("Name of MySQL database to access for logging")]
		public string MySQLLogDatabase { get; set; }

		[DefaultValue ("root")]
		[Description ("User name logging in to MySQL database")]
		public string MySQLLogUser { get; set; }

		[PasswordPropertyText (true)]
		[DefaultValue ("")]
		[Description ("Users password")]
		public string MySQLLogPassword { get; set; }

		[Description ("Address of MySQL database")]
		[DefaultValue ("localhost")]
		public string MySQLLogHost { get; set; }

		[Description ("Port of MySQL database")]
		[DefaultValue (3306L)]
		public uint MySQLLogPort { get; set; }

		[DefaultValue (false)]
		[Description ("Use database connection pools. When possible, sessions will share a single database connection")]
		public bool MySQLLogUseConnectionPool { get; set; }

		[DefaultValue ("message_log")]
		[Description ("Name of table where incoming messages will be logged")]
		public string MySQLLogIncomingTable { get; set; }

		[Description ("Name of table where outgoing messages will be logged")]
		[DefaultValue ("message_log")]
		public string MySQLLogOutgoingTable { get; set; }

		[Description ("Name of table where events will be logged")]
		[DefaultValue ("event_log")]
		public string MySQLLogEventTable { get; set; }

		public MySQLLoggingInfo ()
		{
			this.MySQLLogDatabase = "quickfix";
			this.MySQLLogUser = "root";
			this.MySQLLogPassword = "";
			this.MySQLLogHost = "localhost";
			this.MySQLLogPort = 3306U;
			this.MySQLLogUseConnectionPool = false;
			this.MySQLLogIncomingTable = "message_log";
			this.MySQLLogOutgoingTable = "message_log";
			this.MySQLLogEventTable = "event_log";
		}
	}
}
