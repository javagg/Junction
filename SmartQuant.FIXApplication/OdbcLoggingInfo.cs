using System.ComponentModel;

namespace SmartQuant.FIXApplication
{
	public class OdbcLoggingInfo : OdbcInfoBase
	{
		protected const string DEFAULT_INCOMING_TABLE = "message_log";
		protected const string DEFAULT_OUTGOING_TABLE = "message_log";
		protected const string DEFAULT_EVENT_TABLE = "event_log";

		[DefaultValue ("sa")]
		[Description ("User name logging in to ODBC database")]
		public string OdbcLogUser { get; set; }

		[DefaultValue ("")]
		[Description ("Users password")]
		[PasswordPropertyText (true)]
		public string OdbcLogPassword { get; set; }

		[DefaultValue ("DATABASE=quickfix;DRIVER={SQL Server};SERVER=(local);")]
		[Description ("ODBC connection string for database")]
		public string OdbcLogConnectionString { get; set; }

		[DefaultValue ("message_log")]
		[Description ("Name of table where incoming messages will be logged")]
		public string OdbcLogIncomingTable { get; set; }

		[DefaultValue ("message_log")]
		[Description ("Name of table where outgoing messages will be logged")]
		public string OdbcLogOutgoingTable { get; set; }

		[Description ("Name of table where events will be logged")]
		[DefaultValue ("event_log")]
		public string OdbcLogEventTable { get; set; }

		public OdbcLoggingInfo ()
		{
			this.OdbcLogUser = "sa";
			this.OdbcLogPassword = "";
			this.OdbcLogConnectionString = "DATABASE=quickfix;DRIVER={SQL Server};SERVER=(local);";
			this.OdbcLogIncomingTable = "message_log";
			this.OdbcLogOutgoingTable = "message_log";
			this.OdbcLogEventTable = "event_log";
		}
	}
}
