using System;
using System.Drawing;
using System.Windows.Forms;

namespace TheFirstFarm.Utilities {
	public static class ControlExtension {
		public static void SafeInvoke(this Control control, Action<Control> action, bool synchronize = true) {
			if (control.InvokeRequired) {
				var invoker = new MethodInvoker(delegate { action(control); });
				if (synchronize)
					control.Invoke(invoker);
				else
					control.BeginInvoke(invoker);
			}
			else
				action(control);
		}

		public static void AppendText(this RichTextBox box, string text, Color color) {
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;
			box.SelectionColor = color;
			box.AppendText(text);
			box.SelectionColor = box.ForeColor;
		}

		public static void AppendLine(this RichTextBox box, string text) => box.AppendText(text + Environment.NewLine);

		public static void AppendLine(this RichTextBox box, string text, Color color) => box.AppendText(text + Environment.NewLine, color);
	}
}