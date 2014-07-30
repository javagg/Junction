using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SmartQuant.FIXApplication
{
	public class FileStorageInfo : FileInfoBase
	{
		[Description ("Directory to store sequence number and message files")]
		[Editor (typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string FileStorePath { get; set; }

		public FileStorageInfo ()
		{
			this.FileStorePath = string.Empty;
		}
	}
}
