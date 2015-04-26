using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XiMusic
{
    [DefaultEvent("Search")]
    public partial class ctlSearchBox : UserControl
    {
        public delegate void SearchEventHandler(object sender, EventArgs e, string kw);

        public override string Text
        {
            get
            {
                return txtKeyWord.Text;
            }
            set
            {
                txtKeyWord.Text = value;
            }
        }
        public ctlSearchBox()
        {
            InitializeComponent();
            btnWebSearch.Click += (s, e) =>
            {

                Search(s, e, txtKeyWord.Text);
            };
        }
        [Category("JadeControl")]
        [Description("单击搜索")]
        public event SearchEventHandler Search;
    }
}
