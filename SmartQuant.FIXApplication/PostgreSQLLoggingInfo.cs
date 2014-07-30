using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class PostgreSQLLoggingInfo : PostgreSQLInfoBase
	{
		protected const string DEFAULT_INCOMING_TABLE = "message_log";
		protected const string DEFAULT_OUTGOING_TABLE = "message_log";
		protected const string DEFAULT_EVENT_TABLE = "event_log";

		[Description ("Name of PostgreSQL database to access for logging")]
		[DefaultValue ("quickfix")]
		public string PostgreSQLLogDatabase { get; set; }

		[Description ("User name logging in to PostgreSQL database")]
		[DefaultValue ("postgres")]
		public string PostgreSQLLogUser { get; set; }

		[PasswordPropertyText (true)]
		[Description ("Users password")]
		[DefaultValue ("")]
		public string PostgreSQLLogPassword { get; set; }

		[DefaultValue ("localhost")]
		[Description ("Address of PostgreSQL database")]
		public string PostgreSQLLogHost { get; set; }

		[DefaultValue (5432L)]
		[Description ("Port of PostgreSQL database")]
		public uint PostgreSQLLogPort { get; set; }

		[Description ("Use database connection pools. When possible, sessions will share a single database connection")]
		[DefaultValue (false)]
		public bool PostgreSQLLogUseConnectionPool { get; set; }

		[DefaultValue ("message_log")]
		[Description ("Name of table where incoming messages will be logged")]
		public string PostgreSQLLogIncomingTable { get; set; }

		[DefaultValue ("message_log")]
		[Description ("Name of table where outgoing messages will be logged")]
		public string PostgreSQLLogOutgoingTable { get; set; }

		[Description ("Name of table where events will be logged")]
		[DefaultValue ("event_log")]
		public string PostgreSQLLogEventTable { get; set; }

		public PostgreSQLLoggingInfo ()
		{
			this.PostgreSQLLogDatabase = "quickfix";
			this.PostgreSQLLogUser = "postgres";
			this.PostgreSQLLogPassword = "";
			this.PostgreSQLLogHost = "localhost";
			this.PostgreSQLLogPort = 5432U;
			this.PostgreSQLLogUseConnectionPool = false;
			this.PostgreSQLLogIncomingTable = "message_log";
			this.PostgreSQLLogOutgoingTable = "message_log";
			this.PostgreSQLLogEventTable = "event_log";
		}
	}
}
