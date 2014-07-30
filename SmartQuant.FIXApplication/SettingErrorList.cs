using System.Collections.Generic;

namespace SmartQuant.FIXApplication
{
	public class SettingErrorList
	{
		private List<SettingError> errors;

		public bool HasErrors { get; private set; }

		internal IEnumerable<SettingError> All {
			get {
				return (IEnumerable<SettingError>)this.errors;
			}
		}

		internal SettingErrorList ()
		{
			this.errors = new List<SettingError> ();
			this.HasErrors = false;
		}

		public void AddError (string text)
		{
			this.Add (false, text);
		}

		public void AddWarning (string text)
		{
			this.Add (true, text);
		}

		private void Add (bool isWarning, string text)
		{
			this.errors.Add (new SettingError (isWarning, text));
			if (isWarning)
				return;
			this.HasErrors = true;
		}
	}
}
