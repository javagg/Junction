using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SmartQuant.FIXApplication
{
	[TypeConverter (typeof(ExpandableObjectConverter))]
	public class InfoBase
	{
		protected InfoBase ()
		{
		}

		public override string ToString ()
		{
			return string.Empty;
		}

		public virtual void GetValues (Dictionary<string, object> values)
		{
			foreach (PropertyInfo propertyInfo in this.GetType().GetProperties()) {
				TypeConverter converter = TypeDescriptor.GetConverter (propertyInfo.PropertyType);
				if (converter != null && converter.CanConvertTo (typeof(string)) && converter.CanConvertFrom (typeof(string))) {
					object obj = propertyInfo.GetValue ((object)this, (object[])null);
					if (obj != null)
						values.Add (propertyInfo.Name, obj);
				}
			}
		}

		public virtual void SetValue (Dictionary<string, object> values)
		{
			foreach (KeyValuePair<string, object> keyValuePair in values) {
				PropertyInfo property = this.GetType ().GetProperty (keyValuePair.Key, keyValuePair.Value.GetType ());
				if (property != (PropertyInfo)null)
					property.SetValue ((object)this, keyValuePair.Value, (object[])null);
			}
		}
	}
}
