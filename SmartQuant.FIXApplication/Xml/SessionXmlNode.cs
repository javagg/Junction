using SmartQuant.Xml;
using System;

namespace SmartQuant.FIXApplication.Xml
{
	internal class SessionXmlNode : XmlNodeBase
	{
		private const string ATTR_TYPE = "type";

		public override string NodeName {
			get {
				return "session";
			}
		}

		public Type Type {
			get {
				return this.GetTypeAttribute ("type");
			}
			set {
				this.SetAttribute ("type", value);
			}
		}

		public PropertyListXmlNode Properties {
			get {
				return this.GetChildNode<PropertyListXmlNode> ();
			}
		}
	}
}
