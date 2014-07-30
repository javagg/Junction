using SmartQuant.FIXApplication;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace SmartQuant.FIXApplication.Design
{
	class BeginStringEditor : ObjectSelectorEditor
	{
		protected override void FillTreeWithData (ObjectSelectorEditor.Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
		{
			if (context != null && context.Instance != null) {
				object obj = context.PropertyDescriptor.GetValue (context.Instance);
				selector.Clear ();
				foreach (string label in BeginString.GetValues()) {
					ObjectSelectorEditor.SelectorNode selectorNode = selector.AddNode (label, (object)label, (ObjectSelectorEditor.SelectorNode)null);
					if (((object)label).Equals (obj))
						selector.SelectedNode = (TreeNode)selectorNode;
				}
			} else
				base.FillTreeWithData (selector, context, provider);
		}
	}
}
