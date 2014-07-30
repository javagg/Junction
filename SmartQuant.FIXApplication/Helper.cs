using SmartQuant.FIXApplication.Xml;
using SmartQuant.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace SmartQuant.FIXApplication
{
	internal static class Helper
	{
		public static string SessionsToXml (SessionInfo[] sessions)
		{
			SessionsXmlDocument sessionsXmlDocument = new SessionsXmlDocument ();
			foreach (SessionInfo sessionInfo in sessions) {
				Type type = sessionInfo.GetType ();
				SessionXmlNode sessionXmlNode = sessionsXmlDocument.Sessions.AppendNew ();
				sessionXmlNode.Type = type;
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
				foreach (PropertyInfo propertyInfo in type.GetProperties(bindingAttr)) {
					if (propertyInfo.CanRead && propertyInfo.CanWrite) {
						TypeConverter converter = TypeDescriptor.GetConverter (propertyInfo.PropertyType);
						if (converter != null && converter.CanConvertTo (typeof(string)) && converter.CanConvertFrom (typeof(string))) {
							object obj = propertyInfo.GetValue ((object)sessionInfo, (object[])null);
							if (obj != null)
								sessionXmlNode.Properties.Add (propertyInfo.Name, propertyInfo.PropertyType, converter.ConvertToInvariantString (obj));
						}
					}
				}
			}
			return sessionsXmlDocument.InnerXml;
		}

		public static SessionInfo[] SessionsFromXml (string xml)
		{
			List<SessionInfo> list = new List<SessionInfo> ();
			try {
				SessionsXmlDocument sessionsXmlDocument = new SessionsXmlDocument ();
				sessionsXmlDocument.InnerXml = xml;
				foreach (SessionXmlNode sessionXmlNode in (ListXmlNode<SessionXmlNode>) sessionsXmlDocument.Sessions) {
					Type type = sessionXmlNode.Type;
					SessionInfo sessionInfo = (SessionInfo)Activator.CreateInstance (type);
					list.Add (sessionInfo);
					foreach (PropertyXmlNode propertyXmlNode in (ListXmlNode<PropertyXmlNode>) sessionXmlNode.Properties) {
						PropertyInfo property = type.GetProperty (propertyXmlNode.Name, propertyXmlNode.Type);
						if (property != (PropertyInfo)null && property.CanRead && property.CanWrite) {
							TypeConverter converter = TypeDescriptor.GetConverter (propertyXmlNode.Type);
							if (converter != null && converter.CanConvertFrom (typeof(string)))
								property.SetValue ((object)sessionInfo, converter.ConvertFromInvariantString (propertyXmlNode.Value), (object[])null);
						}
					}
				}
			} catch {
			}
			return list.ToArray ();
		}

		public static string InfoToXml (InfoBase info)
		{
			Dictionary<string, object> values = new Dictionary<string, object> ();
			info.GetValues (values);
			InfoXmlDocument infoXmlDocument = new InfoXmlDocument ();
			foreach (KeyValuePair<string, object> keyValuePair in values) {
				Type type = keyValuePair.Value.GetType ();
				TypeConverter converter = TypeDescriptor.GetConverter (type);
				infoXmlDocument.Properties.Add (keyValuePair.Key, type, converter.ConvertToInvariantString (keyValuePair.Value));
			}
			return infoXmlDocument.InnerXml;
		}

		public static void InfoFromXml (InfoBase info, string xml)
		{
			try {
				InfoXmlDocument infoXmlDocument = new InfoXmlDocument ();
				infoXmlDocument.InnerXml = xml;
				Dictionary<string, object> values = new Dictionary<string, object> ();
				foreach (PropertyXmlNode propertyXmlNode in (ListXmlNode<PropertyXmlNode>) infoXmlDocument.Properties) {
					TypeConverter converter = TypeDescriptor.GetConverter (propertyXmlNode.Type);
					if (converter != null && converter.CanConvertFrom (typeof(string)))
						values.Add (propertyXmlNode.Name, converter.ConvertFromInvariantString (propertyXmlNode.Value));
				}
				info.SetValue (values);
			} catch {
			}
		}
	}
}
