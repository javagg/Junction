using SmartQuant.FIXApplication;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace SmartQuant.FIXApplication.Design
{
	class SessionsEditor : ArrayEditor
	{
		private QuickFIXProvider provider;

		public SessionsEditor ()
			: base (typeof(SessionInfo[]))
		{
		}

		public override object EditValue (ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context != null && context.Instance is QuickFIXProvider)
				this.provider = (QuickFIXProvider)context.Instance;
			return base.EditValue (context, provider, value);
		}

		protected override object CreateInstance (Type itemType)
		{
			if (itemType == typeof(SessionInfo))
				return Activator.CreateInstance (this.provider.GetSessionInfoType ());
			else
				return base.CreateInstance (itemType);
		}
	}
}
