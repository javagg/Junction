using SmartQuant.Xml;

namespace SmartQuant.FIXApplication.Xml
{
	internal class SessionListXmlNode : ListXmlNode<SessionXmlNode>
	{
		public override string NodeName {
			get {
				return "sessions";
			}
		}

		public SessionXmlNode AppendNew ()
		{
			return this.AppendChildNode ();
		}
	}
}
