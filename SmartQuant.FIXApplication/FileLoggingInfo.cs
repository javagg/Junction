using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SmartQuant.FIXApplication
{
	public class FileLoggingInfo : FileInfoBase
	{
		[Editor (typeof(FolderNameEditor), typeof(UITypeEditor))]
		[Description ("Directory to store logs")]
		public string FileLogPath { get; set; }

		[Editor (typeof(FolderNameEditor), typeof(UITypeEditor))]
		[Description ("Directory to store backup logs")]
		public string FileLogBackupPath { get; set; }

		public FileLoggingInfo ()
		{
			this.FileLogPath = string.Empty;
			this.FileLogBackupPath = string.Empty;
		}
	}
}
