using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace windowMediaPlayerDM
{
   
    class clipboard
    {


        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);


    }
}
