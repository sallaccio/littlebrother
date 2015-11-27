using System;
using System.Diagnostics;
using System.Windows.Forms;

using Helpers;
using SmallBrother.Properties;

namespace SmallBrother
{
	/// <summary>
	/// The icon in the taskbar, user main interaction with the program.
	/// </summary>
	class ProcessIcon : IDisposable
	{
		/// <summary>
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		public ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();
		}

		/// <summary>
		/// Displays the icon in the system tray.
		/// </summary>
		public void Display()
		{
            // Put the icon in the system tray	
#if DEBUG
            ni.Icon = Resources.Calendar;
#else
            ni.Icon = Resources.Hourglass_Flat;
#endif

            ni.Text = "Simply record what task you are working on.";
			ni.Visible = true;

            // Allow it react to mouse clicks.	
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();
            ni.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
            TimerFile.addEndItem();
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

        /// <summary>
        /// Handles the MouseClick event of the ni control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        /// <remarks>Should bring some kind of Property Window.</remarks>
        void ni_MouseClick(object sender, MouseEventArgs e)
		{
			// Handle mouse button clicks.
			if (e.Button == MouseButtons.Left)
			{
				// Start Windows Explorer.
				Process.Start("explorer", null);
			}
		}

        /// <summary>
        /// Handles the creation of the menu of the ni control on open (right click).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            new ContextMenus().createMenuList(ni.ContextMenuStrip);
        }
    
    }
}