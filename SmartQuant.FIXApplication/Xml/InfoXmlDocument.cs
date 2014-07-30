using SmartQuant.Xml;

namespace SmartQuant.FIXApplication.Xml
{
	class InfoXmlDocument : XmlDocumentBase
	{
		public PropertyListXmlNode Properties {
			get {
				return this.GetChildNode<PropertyListXmlNode> ();
			}
		}

		public InfoXmlDocument () : base ("info")
		{
		}
	}
}
