namespace SmartQuant.FIXApplication
{
	class SettingError
	{
		public bool IsWarning { get; private set; }

		public string Text { get; private set; }

		public SettingError (bool isWarning, string text)
		{
			this.IsWarning = isWarning;
			this.Text = text;
		}
	}
}
