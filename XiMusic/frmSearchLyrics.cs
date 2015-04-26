using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace XiMusic
{

    public partial class frmSearchLyrics : Form
    {
        protected internal frmXiMusic frmxiMusic;
        private string musicName;
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string MusicName
        {
            get { return musicName; }
            set { musicName = value; }
        }

        private string fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private LyricsHelper lyricsHelper;

        /// <summary>
        /// 加载音乐文件委托事件声音
        /// </summary>
        public delegate void LoadMusicLrcHandler();
        public event LoadMusicLrcHandler LoadMusicLrc;

        #region 窗体拖动
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键        
        private void frmSearchLyrics_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void frmSearchLyrics_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void frmSearchLyrics_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
        #endregion
       
 
        public frmSearchLyrics()
        {
            InitializeComponent();
            LoadMusicLrc += new LoadMusicLrcHandler(SearchLrc_LoadMusicLrc);
        }

        public frmSearchLyrics(string fileName, string musicName)
        {   
            InitializeComponent();

            //通过ImageList绑定,初始化ListView高度样式
            this.lvwLyricsList.SmallImageList = new ImageList();
            //设置高度
            this.lvwLyricsList.SmallImageList.ImageSize = new Size(1, 22);

            this.fileName = fileName;
            //this.musicName = this.txtSongName.Text = musicName;
            this.txtSongName.Text = musicName;
            //加true参数代表是网络访问是需要代理，否则设置为false。
            lyricsHelper = new LyricsHelper(false);
            //代理登录回调函数初始化
            lyricsHelper.InitializeProxy += new EventHandler(InitializeProxy);
            //在线搜索错误回调函数初始化
            lyricsHelper.WebException += new EventHandler(WebException);
            //返回搜索查询结果回调函数初始化
            lyricsHelper.SelectSong += new EventHandler(SelectSong);
        }

        private void picCloseWindow2_Click(object sender, EventArgs e)
        {
                  
            this.Close(); 
        }

        private void picCloseWindow2_MouseEnter(object sender, EventArgs e)
        {
            picCloseWindow2.Image = XiMusic.Properties.Resources.close_highlisht;
        }

        private void picCloseWindow2_MouseLeave(object sender, EventArgs e)
        {
            picCloseWindow2.Image = XiMusic.Properties.Resources.close_normal;
        }

        private void btnSearchLyrics_Click(object sender, EventArgs e)
        {
            this.btnSearchLyrics.Enabled = false;
            Thread thread = new Thread(new ThreadStart(frmSearchLyrics_OnSearchLrc));
            //线程执行搜索操作
            thread.Start();
        }

       
      

        public void SearchLrc_LoadMusicLrc()
        {
            if (LoadMusicLrc != null)
            {
                LoadMusicLrc();
            }
        }

        private void lvwLyricsList_Click(object sender, EventArgs e)
        {
            this.txtLrcName.Text = this.lvwLyricsList.SelectedItems[0].SubItems[1].Text + " - " + this.lvwLyricsList.SelectedItems[0].Text + ".lrc";
        }
        
        private void btnDownloadLyrics_Click(object sender, EventArgs e)
        {


            if (this.lvwLyricsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择一条歌词信息", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                
                //将当前选中歌词信息作为下载对象
                this.lyricsHelper.CurrentSong = (XmlNode)this.lvwLyricsList.SelectedItems[0].Tag;
                //开始下载歌词
                bool flag = this.lyricsHelper.DownloadLrc(this.txtLrcName.Text);
                //判断是否下载成功
                if (flag)
                {
                    IList<Attribute> list = new List<Attribute>();
                    list.Add(new Attribute() { AttributeName = "name", AttributeValue = this.fileName });
                    list.Add(new Attribute() { AttributeName = "lrc", AttributeValue = string.Format("{0}\\lrc\\{1}", Application.StartupPath, this.txtLrcName.Text) });
                    //将歌词文件与歌曲绑定存入XML
                   XmlHelper.Insert(Application.StartupPath + "\\xml\\lrclist.xml", "lrclist", "music", list);
                   // XmlHelper.Insert("\\xml\\lrclist.xml", "lrclist", "music", list);
                    // frmxiMusic.timer2.Enabled = true;
                    //加载歌词,歌词面板回调函数
                    //frmxiMusic.threadSearchLrc = new Thread(new ThreadStart(frmxiMusic.SerchLrc));
                    //frmxiMusic.threadSearchLrc.Start();
                    SearchLrc_LoadMusicLrc();
             
                    //关闭当前搜索窗
                    this.Close();
                }
                else
                {
                    MessageBox.Show("歌词文件下载失败，请检查网络是否连接畅通", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       
        /// <summary>
        /// 返回查询结果回调函数，显示到列表上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectSong(object sender, EventArgs e)
        {
            //返回Xml形式的搜索结果
            XmlNodeList list = sender as XmlNodeList;
            if (list != null)
            {
                if (list.Count == 0)
                {
                    this.lblMessage.Invoke(new EventHandler(delegate
                    {
                        this.lblMessage.Text = "没有找到相应的歌词,请修改一下关键字再试";
                    }));
                }
                else
                {
                    //通过线程委托调用填充数据
                    this.lvwLyricsList.Invoke(new EventHandler(delegate
                    {
                        this.lvwLyricsList.Items.Clear();
                        this.lvwLyricsList.BeginUpdate();
                        foreach (XmlNode node in list)
                        {
                            ListViewItem item = new ListViewItem(node.Attributes["title"].Value);
                            item.Tag = node;
                            item.SubItems.AddRange(new string[] { node.Attributes["artist"].Value });
                            this.lvwLyricsList.Items.Add(item);
                        }
                        this.lvwLyricsList.EndUpdate();
                    }));

                    this.lblMessage.Invoke(new EventHandler(delegate {
                        this.lblMessage.Text = "请选择搜索到的歌词进行下载";
                    }));

                }

                //还原搜索按钮
                this.btnSearchLyrics.Invoke(new EventHandler(delegate
                {
                    this.btnSearchLyrics.Enabled = true;
                }));
            }
        }

        /// <summary>
        /// 在线搜索错误接受回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WebException(object sender, EventArgs e)
        {
            this.btnSearchLyrics.Invoke(new EventHandler(delegate
            {
                this.btnSearchLyrics.Enabled = true;
            }));

            WebException ex = sender as WebException;
            MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 代理登录回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InitializeProxy(object sender, EventArgs e)
        {
            MessageBox.Show("暂不支持代理", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //WebProxy proxy = null;
            //frmProxy f2 = new frmProxy();
            //f2.OkButton.Click += new EventHandler(delegate(object _sender, EventArgs _e)
            //{
            //    proxy = new WebProxy(f2.Host.Text, int.Parse(f2.Port.Text));
            //    proxy.Credentials = new NetworkCredential(f2.Username.Text, f2.PwdText.Text);
            //    f2.Close();
            //});
            //f2.ShowDialog();
            //q.Proxy = proxy;
        }

        

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="LyricsHelper"></param>
        /// <param name="songer"></param>
        /// <param name="musicName"></param>
        public void frmSearchLyrics_OnSearchLrc()
        {
            //执行搜索
            this.lyricsHelper.SearchLrc(this.txtSingerName.Text, this.txtSongName.Text);
        }
    }
}

