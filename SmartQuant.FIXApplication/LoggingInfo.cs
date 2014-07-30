using System.Collections.Generic;

namespace SmartQuant.FIXApplication
{
	public class LoggingInfo : TypeInfo
	{
		public FileLoggingInfo FILE { get; private set; }

		public MySQLLoggingInfo MYSQL { get; private set; }

		public PostgreSQLLoggingInfo POSTGRESQL { get; private set; }

		public OdbcLoggingInfo ODBC { get; private set; }

		public LoggingInfo ()
		{
			this.FILE = new FileLoggingInfo ();
			this.MYSQL = new MySQLLoggingInfo ();
			this.POSTGRESQL = new PostgreSQLLoggingInfo ();
			this.ODBC = new OdbcLoggingInfo ();
		}

		public override void GetValues (Dictionary<string, object> values)
		{
			base.GetValues (values);
			this.FILE.GetValues (values);
			this.MYSQL.GetValues (values);
			this.POSTGRESQL.GetValues (values);
			this.ODBC.GetValues (values);
		}

		public override void SetValue (Dictionary<string, object> values)
		{
			base.SetValue (values);
			this.FILE.SetValue (values);
			this.MYSQL.SetValue (values);
			this.POSTGRESQL.SetValue (values);
			this.ODBC.SetValue (values);
		}
	}
}
