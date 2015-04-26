
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
    [DefaultEvent("PlayItemClick")]
    public partial class ctlMusicList : UserControl
    {
        public delegate void PlayItemHandler(object sender, PlayEventArgs e);
        private List<Song> _Items = new List<Song>();
        private int _MaxSize = 8;
        private int _ItemHeigth = 20;
        private int _ItemPadding = 4;
        private Color _ItemHoverColor = Color.FromArgb(200, ColorTranslator.FromHtml("#008cd6"));
        private Color _ItemTextColor = Color.White;
        private Color _ItemTextHoverColor = Color.Yellow;
        private bool _ShowDelButton = false;

        public ctlMusicList()
        {
            InitializeComponent();
            SetStyle(
                     ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.Selectable
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor,
                     true);    
            InitList();
        }

     
        public Color ItemHoverColor
        {
            get { return _ItemHoverColor; }
            set { _ItemHoverColor = value; }
        }
   
        public Color ItemTextHoverColor
        {
            get { return _ItemTextHoverColor; }
            set { _ItemTextHoverColor = value; }
        }

     
        public int ItemHeigth
        {
            get { return _ItemHeigth; }
            set { _ItemHeigth = value; InitList(); }
        }

        public int ItemPadding
        {
            get { return _ItemPadding; }
            set { _ItemPadding = value; InitList(); }
        }

      
        public int MaxSize
        {
            get { return _MaxSize; }
            set { _MaxSize = value; InitList(); }
        }

      
        public Color ItemTextColor
        {
            get { return _ItemTextColor; }
            set { _ItemTextColor = value; }
        }

        public bool ShowDelButton
        {
            get { return _ShowDelButton; }
            set { _ShowDelButton = value; InitList(); }
        }

      
        [Description("播放列表项"), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<Song> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                if (_Items != null)
                {
                    SetList();
                }
            }
        }

     
        public event PlayItemHandler PlayItemDoubleClick;

      //public event PlayItemHandler PlayItemClick;
        public event PlayItemHandler DelPlayItem;

        private void InitList()
        {
            this.SuspendLayout();
            Controls.Clear();
            Panel[] arr = new Panel[MaxSize];
            for (int i = 0; i < MaxSize; i++)
            {
                var del = new Label { Name = "del", TabIndex = i, Size = new Size(16, 16), Image = Properties.Resources.btn_close_normal, Location = new Point(Width - 26, ItemHeigth * (-1)) };
                del.Click += (s, e) =>
                {
                    var obj = s as Label;
                    DelPlayItem(s, new PlayEventArgs { Index = obj.TabIndex });
                };
                del.MouseEnter += (s, e) =>
                {
                    var obj = s as Label;
                    (obj.Parent as Panel).BackColor = ItemHoverColor;
                    obj.ForeColor = ItemTextHoverColor;
                };
                del.MouseLeave += (s, e) =>
                {
                    var obj = s as Label;
                    (obj.Parent as Panel).BackColor = Color.Transparent;
                    obj.ForeColor = ItemTextColor;
                };
                var title = new Label
                {
                    BackColor = Color.Transparent,
                    ForeColor = ItemTextColor,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new System.Drawing.Font("微软雅黑", 11F),
                    Name = "title",
                    TabIndex = i,
                    Size = new Size(Width - 26, ItemHeigth),
                    Location = new Point(0, ItemHeigth * (-1)),
                };
                title.MouseEnter += (s, e) =>
                {
                    var obj = s as Label;
                    (obj.Parent as Panel).BackColor = ItemHoverColor;
                    obj.ForeColor = ItemTextHoverColor;
                };
                title.MouseLeave += (s, e) =>
                {
                    var obj = s as Label;
                    (obj.Parent as Panel).BackColor = Color.Transparent;
                    obj.ForeColor = ItemTextColor;
                };
                title.DoubleClick += (s, e) =>
                {
                    var obj = s as Label;
                    PlayItemDoubleClick(s, new PlayEventArgs { Index = obj.TabIndex });
                };
                //title.Click += (s, e) =>
                //{
                //    var obj = s as Label;
                //    PlayItemClick(s, new PlayEventArgs { Index = obj.TabIndex });
                //};
                var item = new Panel
                {
                    BackColor = Color.Transparent,
                    Width = Width,
                    Height = ItemHeigth,
                    Location = new Point(0, (ItemHeigth * i) + ItemPadding),
                };
                item.Controls.Add(title);
                item.Controls.Add(del);


                arr[i] = item;
            }

            Controls.AddRange(arr);
            this.ResumeLayout();
        }

        /// <summary>
        /// 创建列表
        /// </summary>
        private void SetList()
        {
            foreach (System.Windows.Forms.Control c in Controls)
            {
                var item = c as Panel;
                var title = item.Controls.Find("title", true)[0];
                var del = item.Controls.Find("del", true)[0];
                title.Location = new Point(0, ItemHeigth * (-1));
                del.Location = new Point(del.Location.X, ItemHeigth * (-1));
                if (Items.Count > item.TabIndex)
                {
                    title.Text = Items[item.TabIndex].Title;
                    title.Location = new Point(0, 0);

                    if (ShowDelButton) { del.Location = new Point(del.Location.X, (ItemHeigth - del.Height) / 2); }
                }

            }
        }

        /// <summary>
        /// 调整列表高度
        /// </summary>
        private void ChangeHeight()
        {
            // Height = MaxSize * (ItemHeigth + ItemPadding);
            //if (Height < 40) Height = 40;
        }

        /// <summary>
        /// 添加Item
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(Song value)
        {
            Items.Add(value);
            SetList();
        }

        public void DelItem(int index)
        {
            Items.RemoveAt(index);
            SetList();
        }

        /// <summary>
        /// 清除Item
        /// </summary>
        public void Clear()
        {
            Items.Clear();
            SetList();
        }

        public override string Text
        {
            get
            {
                return string.Empty;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            InitList();
            base.OnSizeChanged(e);
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        //[Category("JadeControl")]
        //[Description("列表文本颜色")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        public class PlayEventArgs : EventArgs
        {
            public int Index { get; set; }
        }

    }
}

