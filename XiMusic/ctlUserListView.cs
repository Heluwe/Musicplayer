//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace XiMusic
//{
//    public partial class ctlUserListView : UserControl
//    {
//        public ctlUserListView()
//        {
//            InitializeComponent();
//        }
//    }
//}

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XiMusic
{
    public partial class ctlUserListView : ListView
    {
        [DllImport("user32.dll")]
        public static extern int ShowScrollBar(IntPtr hWnd, int iBar, int bShow);
        const int SB_HORZ = 0;
        const int SB_VERT = 1;

        public ctlUserListView()
        {
            //InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (this.View == View.List || this.View == View.Details)
            {
                ShowScrollBar(this.Handle, SB_HORZ, 0);
            }
            base.WndProc(ref m);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {

        }
    }
}
