namespace SmartQuant.FIXApplication
{
	public class OdbcInfoBase : InfoBase
	{
		protected const string DEFAULT_USER = "sa";
		protected const string DEFAULT_PASSWORD = "";
		protected const string DEFAULT_CONNECTION_STRING = "DATABASE=quickfix;DRIVER={SQL Server};SERVER=(local);";

		protected OdbcInfoBase ()
		{
		}
	}
}
