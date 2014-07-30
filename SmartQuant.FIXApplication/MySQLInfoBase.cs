namespace SmartQuant.FIXApplication
{
	public class MySQLInfoBase : InfoBase
	{
		protected const string DEFAULT_DATABASE = "quickfix";
		protected const string DEFAULT_USER = "root";
		protected const string DEFAULT_PASSWORD = "";
		protected const string DEFAULT_HOST = "localhost";
		protected const uint DEFAULT_PORT = 3306U;
		protected const bool DEFAULT_USE_CONNECTION_POOL = false;

		protected MySQLInfoBase ()
		{
		}
	}
}
