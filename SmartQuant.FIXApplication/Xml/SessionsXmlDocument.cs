using SmartQuant.Xml;

namespace SmartQuant.FIXApplication.Xml
{
	internal class SessionsXmlDocument : XmlDocumentBase
	{
		public SessionListXmlNode Sessions {
			get {
				return this.GetChildNode<SessionListXmlNode> ();
			}
		}

		public SessionsXmlDocument () : base ("data")
		{
		}
	}
}
