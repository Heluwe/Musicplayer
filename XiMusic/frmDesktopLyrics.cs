using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Threading;
using System.Xml;

namespace XiMusic
{
    public partial class frmDesktopLyrics : Form
    {      
        public enum LrcStyle
        {
            Spring,
            Summer,
            Fall
        }           
        private Bitmap canvas;
        public Bitmap Canvas
        {
            get { return this.canvas; }
            set { canvas = value; }
        }

        private Graphics g;
        public Graphics G
        {
            get { return g; }
            set { g = value; }
        }

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        private IList<Image> defaultButtonImage = new List<Image>();
        public IList<Image> DefaultButtonImage
        {
            get { return defaultButtonImage; }
            set { defaultButtonImage = value; }
        }

        private IList<Image> mouseLeaveButtonImage = new List<Image>();
        public IList<Image> MouseLeaveButtonImage
        {
            get { return mouseLeaveButtonImage; }
            set { mouseLeaveButtonImage = value; }
        }

        private IList<Image> mouseDownButtonImage = new List<Image>();
        public IList<Image> MouseDownButtonImage
        {
            get { return mouseDownButtonImage; }
            set { mouseDownButtonImage = value; }
        }
        
        private SizeF lrcSize;
        public SizeF LrcSize
        {
            get { return lrcSize; }
            set { lrcSize = value; }
        }
        private int currentOpacity;
        public int CurrentOpacity
        {
            get { return currentOpacity; }
            set { currentOpacity = value; }
        }

        private bool[] isControlMouseEnter = new bool[10];
        public bool[] IsControlMouseEnter
        {
            get { return isControlMouseEnter; }
            set { isControlMouseEnter = value; }
        }

        private Color[] beforeColor = new Color[2];
  
        public Color[] BeforeColor
        {
            get { return beforeColor; }
            set { beforeColor = value; }
        }

        private Color[] afterColor = new Color[2];
 
        public Color[] AfterColor
        {
            get { return afterColor; }
            set { afterColor = value; }
        }              
        private int id;
     
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string musicName;
   
        public string MusicName
        {
            get { return musicName; }
            set { musicName = value; }
        }

        private string lrcText;

        public string LrcText
        {
            get { return lrcText; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    lrcText = "MUSIC...";
                }
                else
                {
                    lrcText = value;
                }
            }
        }

        private double musicTime;
  
        public double MusicTime
        {
            get { return musicTime; }
            set { musicTime = value; }
        }

        private double send;
     
        public double Send
        {
            get { return send; }
            set { send = value; }
        }

        private double currentTime;
     
        public double CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        private double currentMusicTime;
      
        public double CurrentMusicStartTime
        {
            get { return currentMusicTime; }
            set { currentMusicTime = value; }
        }

        private double currentMusicEndTime;
      
        public double CurrentMusicEndTime
        {
            get { return currentMusicEndTime; }
            set { currentMusicEndTime = value; }
        }

        private bool isPlay;
     
        public bool IsPlay
        {
            get { return isPlay; }
            set { isPlay = value; }
        }        
        private bool isMouseEnter;
        public bool IsMouseEnter
        {
            get { return isMouseEnter; }
            set { isMouseEnter = value; }
        }

        private bool isAfreshDraw;
   
        public bool IsAfreshDraw
        {
            get { return isAfreshDraw; }
            set
            {
                isAfreshDraw = value;
                if (this.CurrentMusicStartTime - this.currentMusicEndTime != 0)
                {
                    this.send = (this.currentTime - this.CurrentMusicStartTime) / (this.currentMusicEndTime - this.CurrentMusicStartTime);                 
                }
                if (this.isAfreshDraw)
                {
                    this.manualReset.Set();
                }
            }
        }
        private bool isLockPanel;
        /// <summary>
        /// 是否锁定歌词
        /// </summary>
        public bool IsLockPanel
        {
            get { return isLockPanel; }
            set { isLockPanel = value; }
        }
        public Thread thread;
        //线程阻塞
        public ManualResetEvent manualReset = new ManualResetEvent(false);

        //关闭歌词面板
        public delegate void CloseLrcPanelHandler();
        public event CloseLrcPanelHandler CloseLrcPanel;
        //锁定歌词面板
        public delegate void LockMusicLrcPanelHandler();
        public event LockMusicLrcPanelHandler LockMusicLrcPanel;

        /// <summary>
        /// 歌词搜索
        /// </summary>
        public delegate void SearchLrcPanelHandler();
        public event SearchLrcPanelHandler SearchLrcPanel;

        public frmDesktopLyrics()
        {                     
            InitializeComponent();          
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            //定位到屏幕中间下方
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, Screen.PrimaryScreen.Bounds.Height - 50 - this.Height);

            string startupPath = Application.StartupPath;
            ////加载图片按钮
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minpre.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minplay.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minpause.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minnext.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minset.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minlock.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minstyle.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\minclose.png"));
            this.defaultButtonImage.Add(Image.FromFile(startupPath + "\\images\\mini\\mindown.png"));
            using (IniFiles ini = new IniFiles(startupPath + "\\ini\\config.ini"))
            {
                string id = ini.ReadValue("Lyrics", "LrcStyle");

                XmlNodeList xmlNodeList = XmlHelper.Read(Application.StartupPath + "\\xml\\lrcstyle.xml", "/lrclist/color");
                foreach (XmlNode node in xmlNodeList)
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
                    toolStripMenuItem.Tag = node;
                    toolStripMenuItem.Text = node.Attributes["name"].Value;
                    toolStripMenuItem.Click += new EventHandler(ToolStripMenuItem_Click);

                    if (id == node.Attributes["id"].Value)
                    {
                        //涂色前
                        this.beforeColor[0] = ColorTranslator.FromHtml(node.ChildNodes[0].Attributes["colorUp"].Value);
                        this.beforeColor[1] = ColorTranslator.FromHtml(node.ChildNodes[0].Attributes["colorDown"].Value);
                        //涂色后
                        this.afterColor[0] = ColorTranslator.FromHtml(node.ChildNodes[1].Attributes["colorUp"].Value);
                        this.afterColor[1] = ColorTranslator.FromHtml(node.ChildNodes[1].Attributes["colorDown"].Value);

                        toolStripMenuItem.Checked = true;
                    }
                    this.cmsLrcColorStyle.Items.Add(toolStripMenuItem);
                }
               
            }
        }

        #region 重写事件

        /// <summary>  
        /// 重写OnControlAdded方法，为每个子控件添加MouseLeave事件  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void OnControlAdded(ControlEventArgs e)
        {
            Control control = e.Control; // 获取添加的子控件  
            control.MouseLeave += this.SubControlLeave; // 当鼠标离开该子控件时判断是否是离开SelfDefinePanel  
            base.OnControlAdded(e);
        }

        /// <summary>  
        /// 重写OnMouseLeave事件，如果是离开本身的矩形区域则发生 base.OnMouseLeave(e);  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void OnMouseLeave(EventArgs e)
        {
            //判断鼠标是否还在本控件的矩形区域内  
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(Control.MousePosition)) // this.RectangleToScreen(this.ClientRectangle) 映射为屏幕的矩形  
            {
                base.OnMouseLeave(e);
            }
        }

        /// <summary>  
        /// 子控件鼠标离开时也要做相应的判断  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        public void SubControlLeave(Object sender, EventArgs e)
        {
            //判断鼠标是否还在本控件的矩形区域内  
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(Control.MousePosition))
            {
                base.OnMouseLeave(e);
            }
        }

        #endregion

        private void DiskLrcFrm_Load(object sender, EventArgs e)
        {
            this.thread = new Thread(DrawPanelThread);
            this.thread.Start();
        }

        /// <summary>
        /// 设置歌词颜色风格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            XmlNode node = (XmlNode)item.Tag;
            //涂色前
            this.beforeColor[0] = ColorTranslator.FromHtml(node.ChildNodes[0].Attributes["colorUp"].Value);
            this.beforeColor[1] = ColorTranslator.FromHtml(node.ChildNodes[0].Attributes["colorDown"].Value);
            //涂色后
            this.afterColor[0] = ColorTranslator.FromHtml(node.ChildNodes[1].Attributes["colorUp"].Value);
            this.afterColor[1] = ColorTranslator.FromHtml(node.ChildNodes[1].Attributes["colorDown"].Value);

            ToolStripItemCollection collection = this.cmsLrcColorStyle.Items;
            foreach (ToolStripItem toolStripItem in collection)
            {
                //清除所有菜单选中项
                if (toolStripItem is ToolStripMenuItem)
                {
                    ((ToolStripMenuItem)toolStripItem).Checked = false;
                }
            }
            //选中当前项
            item.Checked = true;

            using (IniFiles ini = new IniFiles(Application.StartupPath + "\\ini\\config.ini"))
            {
                //将当前选中风格保存到配置文件
                ini.WriteValue("Lyrics", "LrcStyle", node.Attributes["id"].Value);
            }
        }

        /// <summary>
        /// 自定义颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void Init(string musicName, int musicTime)
        {
            this.musicName = this.lrcText = musicName;
            this.musicTime = musicTime;
            this.isAfreshDraw = true;
        }

        protected void DrawPanel()
        {
            try
            {
                //创建新的画布
                this.canvas = new Bitmap(this.Width, this.Height);
                using (this.g = Graphics.FromImage(canvas))
                {
                    lock (g)
                    {
                        //绘制背景
                        using (Brush brush = new SolidBrush(Color.FromArgb(this.currentOpacity, Color.Black)))
                        {
                            //抗锯齿消除
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            //绘制字符的模式
                            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                            //绘制背景
                            g.FillRectangle(brush, new Rectangle(0, 0, this.Width, this.Height));
                            //绘制菜单
                          // g.FillRectangle(new SolidBrush(Color.FromArgb(this.isMouseEnter ? 255 : 0, 40, 42, 41)), new Rectangle((this.Width - 280) / 2, 0, 280, 28));

                            if (this.isMouseEnter)
                            {
                                ImageAttributes imageAttributes = new ImageAttributes();
                                //设置图片透明度
                                imageAttributes.SetColorKey(Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255));
                                ColorMatrix myColorMatrix = new ColorMatrix();
                                // 红色伐值
                                myColorMatrix.Matrix00 = 1.00f;
                                // 绿色伐值
                                myColorMatrix.Matrix11 = 1.00f;
                                // 蓝色伐值
                                myColorMatrix.Matrix22 = 1.00f;
                                //透明度伐值
                                myColorMatrix.Matrix33 = 1.00f;
                                // 亮度伐值
                                myColorMatrix.Matrix44 = 1.00f;
                                //设置颜色矩阵
                               imageAttributes.SetColorMatrix(myColorMatrix);                           
                                //绘制样式按钮
                                g.DrawImage(
                                        new Bitmap(this.defaultButtonImage[6]),
                                        new Rectangle(this.Width / 2 + 38, 7/*通过Y轴坐标绘制产生按钮从顶部进入的效果*/, 17, 16),
                                        0, 0, 17, 16,
                                        GraphicsUnit.Pixel,
                                        imageAttributes
                                );

                                //绘制下载按钮
                                g.DrawImage(
                                        new Bitmap(this.defaultButtonImage[8]),
                                        new Rectangle(this.Width / 2 + 65, 7/*通过Y轴坐标绘制产生按钮从顶部进入的效果*/, 16, 16),
                                        0, 0, 16, 16,
                                        GraphicsUnit.Pixel,
                                        imageAttributes
                                );

                                //绘制锁定按钮
                                g.DrawImage(
                                        new Bitmap(this.defaultButtonImage[5]),
                                        new Rectangle(this.Width / 2 + 95, 7/*通过Y轴坐标绘制产生按钮从顶部进入的效果*/, 15, 15),
                                        0, 0, 15, 15,
                                        GraphicsUnit.Pixel,
                                        imageAttributes
                                );

                                //绘制关闭按钮
                                g.DrawImage(
                                        new Bitmap(this.defaultButtonImage[7]),
                                        new Rectangle(this.Width / 2 + 120, 7/*通过Y轴坐标绘制产生按钮从顶部进入的效果*/, 15, 14),
                                        0, 0, 15, 14,
                                        GraphicsUnit.Pixel,
                                        imageAttributes
                                );

                            }
                        }

                        using (GraphicsPath myPath = new GraphicsPath())
                        {
                            //初始化文字样式
                            Font f = new Font("黑体", 44, FontStyle.Bold, GraphicsUnit.Pixel, (Byte)134);
                            //获取文字尺寸
                            this.lrcSize = g.MeasureString(this.lrcText, f);
                            Point point = new Point((this.Width - (int)this.lrcSize.Width) / 2, 50);

                            point.X = point.X < 0 ? 0 : point.X;
                            //计算要涂色的歌词范围
                            int spread = (int)(this.LrcSize.Width * this.send);

                            //绘制一个矩形，大小为当前歌词的尺寸
                            Rectangle rectangle = new Rectangle(point, new Size((int)this.lrcSize.Width, (int)this.lrcSize.Height));

                            //判断当前歌词是否超出面板
                            if ((int)this.lrcSize.Width > this.Width)
                            {
                                //判断当前歌词涂抹是否超出面板
                                if (spread > this.Width / 2)
                                {
                                    if (spread - this.Width / 2 >= this.lrcSize.Width - this.Width)
                                    {
                                        point.X = rectangle.X = this.Width - (int)this.lrcSize.Width;
                                    }
                                    else
                                    {
                                        //位移X坐标向左走动超出部分
                                        point.X = this.Width / 2 - spread;
                                        rectangle.X = point.X;
                                    }
                                }
                                else
                                {
                                    point.X = rectangle.X = 10;

                                }
                            }

                            //声明一个线性渐变绘制类
                            using (LinearGradientBrush myBrush = new LinearGradientBrush(rectangle, this.beforeColor[0], this.beforeColor[1], LinearGradientMode.Vertical))
                            {
                                ColorBlend colorBlend = new ColorBlend();
                                //定义多种颜色
                                colorBlend.Colors = new Color[] { this.beforeColor[0], this.beforeColor[1], this.beforeColor[0] };
                                colorBlend.Positions = new float[] { 0 / 3f, 1 / 2f, 3 / 3f };
                                //设置多色渐变颜色
                                myBrush.InterpolationColors = colorBlend;
                                //添加画刷路径歌词和字符中样式
                                myPath.AddString(
                                    //歌词内容
                                    this.lrcText,
                                    //歌词字体
                                    new FontFamily("黑体"),
                                    //加粗样式
                                    (int)FontStyle.Bold,
                                    //字体大小
                                    44,
                                    //歌词在面板上的定位为水平居中、垂直居中
                                    point,
                                    //字符版式
                                    StringFormat.GenericTypographic
                                );

                                //创建新的画布
                                Bitmap bt = new Bitmap(this.Width, this.Height);
                                //创建新的绘图作为歌词覆盖层
                                using (Graphics gCover = Graphics.FromImage(bt))
                                {
                                    //抗锯齿消除
                                    gCover.SmoothingMode = SmoothingMode.AntiAlias;
                                    //绘制字符的模式
                                    gCover.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                                    using (LinearGradientBrush myBrushCover = new LinearGradientBrush(rectangle, this.afterColor[0], this.afterColor[1], LinearGradientMode.Vertical))
                                    {
                                        gCover.FillPath(myBrushCover, myPath);
                                    }
                                }

                                //绘制渐变描边
                                g.DrawPath(new Pen(Color.FromArgb(200, 0, 0, 0), 1), myPath);
                                g.DrawPath(new Pen(Color.FromArgb(150, 0, 0, 0), 2), myPath);
                                g.DrawPath(new Pen(Color.FromArgb(100, 0, 0, 0), 2), myPath);

                                //填充文字颜色
                                g.FillPath(myBrush, myPath);


                                rectangle.Size = new Size((spread), (int)this.lrcSize.Height);

                                g.DrawImage(bt, point.X, point.Y, rectangle, GraphicsUnit.Pixel);
                                bt.Dispose();
                            }
                        }

                        //开始绘制
                        new NativeMethods().UpdateLayeredWindow(this.canvas, this);

                        this.canvas.Dispose();
                        this.manualReset.Reset();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        GraphicsPath GetStringPath(string s, float dpi, RectangleF rect, Font font, StringFormat format)
        {
            GraphicsPath path = new GraphicsPath();
            float emSize = dpi * font.SizeInPoints / 72;
            path.AddString(s, font.FontFamily, (int)font.Style, emSize, rect, format);

            return path;
        }

        /// <summary>
        /// 窗体的CreateParams属性,窗体参数
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;
                CreateParams cParms = base.CreateParams;
                if (!this.DesignMode)//设计模式下不执行代码(Site.DesignMode设计模式)
                {
                    cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED层样式
                }
                cParms.Style = cParms.Style | WS_MINIMIZEBOX;   // 允许最小化操作

                return cParms;
            }
        }

        /// <summary>   
        /// 文字颜色开始渐变   
        /// </summary>   
        /// <returns></returns>   
        //public int Play()
        //{
        //    this.tmrLrcReflash.Start();
        //    return 0;
        //}

        private void DiskLrcFrm_MouseEnter(object sender, EventArgs e)
        {
            //判断是否已锁定歌词，如果已锁定歌词则鼠标移动上去不作显示处理
            if (this.isLockPanel)
            {
                this.currentOpacity = 0;
                this.isMouseEnter = false;
            }
            else
            {
                this.currentOpacity = 80;
                this.isMouseEnter = true;

                this.manualReset.Set();
            }
        }

        /// <summary>
        /// 线程侦听绘制面板
        /// </summary>
        protected void DrawPanelThread()
        {
            while (true)
            {
               
                DrawPanel();
                //线程阻塞
                this.manualReset.WaitOne();
            }
        }

        private void DiskLrcFrm_MouseLeave(object sender, EventArgs e)
        {
            this.currentOpacity = 0;
            this.isMouseEnter = false;

            this.manualReset.Set();
        }

        /// <summary>
        /// 窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiskLrcFrm_MouseDown(object sender, MouseEventArgs e)
        {
            //如果锁定歌词则无法移动歌词面板
            if (!this.isLockPanel)
            {
                if (MouseButtons.Left == e.Button)
                {
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(this.Handle, NativeMethods.WM_SYSCOMMAND, NativeMethods.SC_MOVE + NativeMethods.HTCAPTION, 0);
                }
            }
        }

        private void lblStyle_Click(object sender, EventArgs e)
        {
            //设置当前
            this.cmsLrcColorStyle.Show(new Point(this.Location.X + this.lblStyle.Location.X - (this.cmsLrcColorStyle.Width / 2), this.Location.Y - this.cmsLrcColorStyle.Height));

           
        }

  
        /// <summary>
        /// 锁定面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblLock_Click(object sender, EventArgs e)
        {
            this.isLockPanel = true;
            if (this.LockMusicLrcPanel != null)
            {
                this.LockMusicLrcPanel();
            }
        }
        
        /// <summary>
        /// 关闭歌词面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblClose_Click(object sender, EventArgs e)
        {
            
            this.manualReset.Set();
            this.thread.Abort();
            frmXiMusic.isLyricsOpen = false;
            if (this.CloseLrcPanel != null)
            {
                this.CloseLrcPanel();
            }
        }

        /// <summary>
        /// 下载歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDownload_Click(object sender, EventArgs e)
        {
            if (this.SearchLrcPanel != null)
            {
                this.SearchLrcPanel();
            }
        }

    
    }
}

