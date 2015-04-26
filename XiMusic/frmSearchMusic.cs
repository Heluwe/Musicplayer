using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;

namespace XiMusic
{

    public partial class frmSearchMusic : Form
    {
        #region 窗体边框阴影效果变量申明
        const int CS_DROPSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion
        //frmXiMusic frmxiMusic = new frmXiMusic();
        public frmXiMusic frmxiMusic = null;
        public int id;
        private bool isFirst = true;
        //frmXiMusic frmxiMusic;
        private System.Windows.Forms.Panel panName;
        private PageInfo pageInfo = new PageInfo() { PageSize = 12 };
        public List<Music> searchMusicList = new List<Music>();
        public List<Music> musicOnlineList = new List<Music>();
        private List<Music> list = new List<Music>();
        ScriptAfflux sa = new ScriptAfflux();

        public object LoadingPic { get; private set; }

        public frmSearchMusic(frmXiMusic fmxiMusic)
        {
            InitializeComponent();
            frmxiMusic = fmxiMusic;
            this.BringToFront();
        }

        public frmSearchMusic()
        {

            InitializeComponent();
        }



        /// <summary>
        /// 获得焦点清楚提示文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_Click(object sender, EventArgs e)
        {
            pMusicList.Controls["panelSearchMusic"].Controls["panelOnlineMusic"].Controls["txtSerarch"].Text = string.Empty;
        }

        /// <summary>
        /// 文本框回车响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)//如果输入的是回车键  
            {
                this.panelSearchMusic_Click(sender, e);//触发button事件  
            }
        }

        /// <summary>
        /// 点击搜索在线音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelSearchMusic_Click(object sender, EventArgs e)
        {
            string searchText = pMusicList.Controls["panelSearchMusic"].Controls["panelOnlineMusic"].Controls["txtSerarch"].Text;
            Task.Factory.StartNew(() =>
            {
                pageInfo.PageIndex = pageInfo.PageIndex == 0 ? 1 : pageInfo.PageIndex;
                var musicOnlineList = new SearchMusic().GetMusicByApi(searchText, pageInfo.PageIndex, pageInfo.PageSize);
                if (musicOnlineList != null && musicOnlineList.data != null)
                {
                    pageInfo.RecordCount = musicOnlineList.data.total;
                    pageInfo.PageCount = (int)Math.Ceiling((double)pageInfo.RecordCount / pageInfo.PageSize);
                    searchMusicList.Clear(); //这里如果清空那么正在播放的音乐完成后会接不上下一曲
                    foreach (var item in musicOnlineList.data.info)
                    {
                        var music = new Music()
                        {
                            MusciURL = new SearchMusic().GetMusicUrl(item.hash),
                            MusicName = item.filename,
                            MusicPic = new SearchMusic().GetMusicPic(item.singername),
                            MusicTime = MusicTimes.ConvertTime(item.duration),
                            Bitrate = item.bitrate + "kbps"
                        };
                        searchMusicList.Add(music);
                    }
                }
                pMusicList.Invoke(new Action(() =>
                {
                    this.pMusicList.Controls.Clear();
                    LoadData(searchMusicList);
                    CreateSearchControl(sender, e);
                }));
            });
        }

        private void LoadData(List<Music> list)
        {
            bool isLoad = list != null && list.Any();
            while (!isLoad)
            {
                if (PlayerType.type == PlayType.online)
                {
                    //todo:这里可以加入一个正在加载的图标
                    //list = ResolveOnlineMusicList.ResolveMusic(GetHtmlCode.WriteInfo(1));
                    isLoad = musicOnlineList != null && musicOnlineList.Any();
                    list = musicOnlineList;
                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }
            if (!isLoad)
                return;
            pMusicList.Invoke(new Action(() =>
            {
                string music = string.Empty;         
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    bool isCurrentMusic = list[i].MusicName.Equals(music);

                    panName = new System.Windows.Forms.Panel();
                    panName.Dock = System.Windows.Forms.DockStyle.Top;
                    panName.Location = new System.Drawing.Point(0, 0);
                    panName.Name = "panelSong" + (i + 1);
                    panName.Tag = i + 1;
                    panName.Size = new System.Drawing.Size(279, 35);
                    panName.TabIndex = i + 1;
                    panName.BackColor = Color.FromArgb(230, 239, 240);
                    panName.Font = new Font("宋体", panName.Font.Size);
             

                    System.Windows.Forms.Label lbName = new System.Windows.Forms.Label();
                    lbName.AutoSize = false;
                    lbName.Tag = i + 1;
                    lbName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                    lbName.Location = new System.Drawing.Point(7, 14);
                    lbName.Name = "lbSong";// + (i + 1);
                    lbName.Size = new System.Drawing.Size(175, 12);
                    lbName.Text = list[i].MusicName;
                    lbName.TabIndex = list[i].MusicName.Length > 16 ? 1 : 0;           
                    lbName.ForeColor = Color.FromArgb(106, 106, 106);

                    System.Windows.Forms.Label lbError = new System.Windows.Forms.Label();
                    lbError.AutoSize = true;
                    lbError.Tag = i + 1;
                    lbError.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                    lbError.Location = new System.Drawing.Point(80, 35);
                    lbError.Name = "lbError";// + (i + 1);
                    lbError.Size = new System.Drawing.Size(113, 12);
                    lbError.Text = "读取错误，文件不存在！";
                    lbError.ForeColor = Color.Red;
                    lbError.Visible = false;

                    System.Windows.Forms.Label lbTime = new System.Windows.Forms.Label();
                    lbTime.AutoSize = true;
                    lbTime.Tag = i + 1;
                    lbTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                    lbTime.Location = new System.Drawing.Point(200, 14);
                    lbTime.Name = "lbTime";
                    lbTime.Size = new System.Drawing.Size(113, 12);
                    lbTime.Text = list[i].MusicTime;
                    lbTime.ForeColor = Color.FromArgb(106, 106, 106);

                    panName.Controls.Add(lbTime);
                    panName.Controls.Add(lbName);
                    panName.Controls.Add(lbError);
                    //if (lbBit != null)
                    //    panName.Controls.Add(lbBit);

                    //为生成的lable注册事件
                    lbName.MouseEnter += new System.EventHandler(lbName_MouseEnter);
                    lbName.MouseLeave += new System.EventHandler(lbName_MouseLeave);               
                    lbName.DoubleClick += new EventHandler(lbName_DoubleClick);

                    //为生成的panel注册事件
                    panName.MouseEnter += new System.EventHandler(panName_MouseEnter);
                    panName.MouseLeave += new System.EventHandler(panName_MouseLeave);
                    panName.DoubleClick += new System.EventHandler(panName_DoubleClick);
                    this.pMusicList.Controls.Add(panName);
                }
            }));
        }

        /// <summary>
        /// 双击列表播放音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panName_DoubleClick(object sender, EventArgs e)
        {
            if (frmxiMusic.isOnFm == true)
            {
                frmxiMusic.webBrowser1.Navigate("about: blank");
               // frmxiMusic.isOnFm = false;
            }
            PlayerType.type = PlayType.collection;
            Panel p = (Panel)sender;
            foreach (Control control in p.Parent.Controls)
            {
                if (control is Panel)
                {
                    if (control == p)
                    {
                        id = (int)control.Tag;     //读取歌曲编号                      
                     if (PlayerType.type == PlayType.collection)
                        {
                            PlayerType.PlayerGroup = PlayType.collection;
                            string url = searchMusicList[id - 1].MusciURL;
                            // frmxiMusic.axWindowsMediaPlayer1.URL = searchMusicList[id - 1].MusciURL; //给播放器设置文件路径
                            frmxiMusic.axWindowsMediaPlayer1.Ctlcontrols.stop();
                            frmxiMusic.lblMusicName.Text = searchMusicList[id - 1].MusicName;
                            //    frmxiMusic.threadSearchLrc.Start();
                            frmxiMusic.timer1.Enabled = false;
                            frmxiMusic.timer2.Enabled = false;
                            //CommonData.songUrl = searchMusicList[id].MusciURL;
                           // CommonData.songId = id;
                         //   CommonData.songUrl = searchMusicList[CommonData.songId].MusciURL;
                         //   CommonData.songName = searchMusicList[CommonData.songId].MusicName;
                         //   id = CommonData.songId;
                            frmxiMusic.PlayMusic(searchMusicList[id - 1].MusciURL);
                            
                            //string musicName = searchMusicList[id - 1].MusicName;
                            //frmxiMusic.lblMusicName.Text = musicName;
                            // frmxiMusic.threadSearchLrc = new Thread(new ThreadStart(frmxiMusic.SerchLrc));
                            // frmxiMusic.threadSearchLrc.Start();
                            //加载网络图片
                            frmxiMusic.picArtist.Image = string.IsNullOrEmpty(searchMusicList[id - 1].MusicPic)
                                ? Properties.Resources.default_album
                                : Image.FromFile(searchMusicList[id - 1].MusicPic);
                            // frmxiMusic.picArtist.Image = Image.FromFile(searchMusicList[id - 1].MusicPic);
                        }
                        PlayHelper.PlayStatu = 1; //设置播放器状态为正在播放
                        PlayHelper.MusicId = id;    //保存当前播放的歌曲Id
                       
                    }
                }
            }
        }

        private void lbName_DoubleClick(object sender, EventArgs e)
        {
            if (frmxiMusic.isOnFm == true)
            {
                frmxiMusic.webBrowser1.Navigate("about: blank");
               // frmxiMusic.isOnFm = false;
            }
            PlayerType.type = PlayType.collection;
            Label l = (Label)sender;
            foreach (Control control in l.Parent.Controls)
            {
                if (control is Label)
                {
                    if (control == l)
                    {
                        id = (int)control.Tag;     //读取歌曲编号                     
                        if (PlayerType.type == PlayType.collection)
                        {
                            PlayerType.PlayerGroup = PlayType.collection;
                            string url = searchMusicList[id - 1].MusciURL;
                            //  frmxiMusic.axWindowsMediaPlayer1.URL = searchMusicList[id - 1].MusciURL; //给播放器设置文件路径 
                            frmxiMusic.axWindowsMediaPlayer1.Ctlcontrols.stop();
                            frmxiMusic.timer2.Enabled = false;
                            frmxiMusic.timer1.Enabled = false;
                            // frmxiMusic.threadSearchLrc.Abort();
                            frmxiMusic.lblMusicName.Text = searchMusicList[id - 1].MusicName;
                            // frmxiMusic.threadSearchLrc.Start();
                            //CommonData.songUrl = searchMusicList[id].MusciURL;
                         //   CommonData.songId = id;
                            
                         //   CommonData.songUrl = searchMusicList[CommonData.songId].MusciURL;
                        //    CommonData.songName = searchMusicList[CommonData.songId].MusicName;
                         //   id = CommonData.songId;
                            frmxiMusic.PlayMusic(searchMusicList[id - 1].MusciURL);
                            
                            // frmxiMusic.threadSearchLrc = new Thread(new ThreadStart(frmxiMusic.SerchLrc));
                            // frmxiMusic.threadSearchLrc.Start();
                            //string musicName = searchMusicList[id - 1].MusicName;
                            //frmxiMusic.lblMusicName.Text = musicName;
                            //加载网络图片
                            // pictureBox1.Image = Image.FromFile(searchMusicList[id - 1].MusicPic);

                            frmxiMusic.picArtist.Image = string.IsNullOrEmpty(searchMusicList[id - 1].MusicPic) ? Properties.Resources.default_album : Image.FromFile(searchMusicList[id - 1].MusicPic);

                        }
                        PlayHelper.PlayStatu = 1; //设置播放器状态为正在播放
                        PlayHelper.MusicId = id;    //保存当前播放的歌曲Id
                        
                    }
                }
            }
        }

        #region 音乐列表事件
        /// <summary>
        /// 为动态生成的panel注册鼠标移入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panName_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                Panel p = (Panel)sender;
                foreach (Control control in p.Parent.Controls)  //遍历PanelList下的所有panel控件
                {
                    if (control is Panel && control == p)   //找到当前鼠标所在的panel
                    {
                        if (control.Height != 60)   //如果它的高度不是60的话
                        {
                            p.BackColor = Color.FromArgb(209, 219, 219);    //将颜色设置为默认
                            return;
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        private void lbName_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                Label l = (Label)sender;
                foreach (Control control in l.Parent.Parent.Controls)
                {
                    if (control is Panel && control.Controls["lbSong"] == l)
                    {
                        if (control.Height != 60)
                        {
                            l.Parent.BackColor = Color.FromArgb(209, 219, 219); //调用父控件的backcolor属性设置
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panName_Click(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            foreach (Control control in p.Parent.Controls)
            {
                if (control is Panel)
                {
                    if (control == p)
                    {
                        control.Height = 60;
                        control.BackColor = Color.FromArgb(143, 173, 178);
                        control.Controls["lbSong"].ForeColor = Color.White;
                        control.Controls["lbTime"].ForeColor = Color.White;
                        // AddBitRate(control);
                    }
                    else
                    {
                        control.Height = 35;
                        control.BackColor = Color.FromArgb(230, 239, 240);
                        if (control.Controls["lbSong"] != null)
                        {
                            control.Controls["lbSong"].ForeColor = Color.FromArgb(106, 106, 106);
                            control.Controls["lbTime"].ForeColor = Color.FromArgb(106, 106, 106);
                        }
                        if (PlayerType.type == PlayType.collection)
                            control.Parent.Controls["panelSearchMusic"].BackColor = Color.FromArgb(148, 187, 193);
                    }
                }
            }
        }

        private void lbName_Click(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            foreach (Control control in l.Parent.Parent.Controls)
            {
                if (control is Panel)
                {
                    if (control.Controls["lbSong"] == l)
                    {
                        control.Height = 60;
                        control.BackColor = Color.FromArgb(143, 173, 178);
                        l.ForeColor = Color.White;
                        control.Controls["lbTime"].ForeColor = Color.White;
                        //AddBitRate(control);
                    }
                    else
                    {
                        control.Height = 35;
                        control.BackColor = Color.FromArgb(230, 239, 240);
                        if (control.Controls["lbSong"] != null)
                        {
                            control.Controls["lbSong"].ForeColor = Color.FromArgb(106, 106, 106);
                            control.Controls["lbTime"].ForeColor = Color.FromArgb(106, 106, 106);
                        }
                        if (PlayerType.type == PlayType.collection)
                            control.Parent.Controls["panelSearchMusic"].BackColor = Color.FromArgb(148, 187, 193);
                    }
                }
            }
        }

        /// <summary>
        /// 为动态生成的panel注册鼠标移出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panName_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                Panel p = (Panel)sender;
                foreach (Control control in p.Parent.Controls)
                {
                    if (control is Panel && control == p)
                    {
                        if (control.Height != 60)
                        {
                            p.BackColor = Color.FromArgb(230, 239, 240);
                            return;
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        private void lbName_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                Label l = (Label)sender;
                foreach (Control control in l.Parent.Parent.Controls)
                {
                    if (control is Panel && control.Controls["lbSong"] == l)
                    {
                        if (control.Height != 60)
                        {
                            l.Parent.BackColor = Color.FromArgb(230, 239, 240);
                            return;
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
          
        }

        #endregion

      

        //private void SearchForm_Load(object sender, EventArgs e)
        //{
        //    webBrowser1.ObjectForScripting = sa;
        //    webBrowser1.AllowWebBrowserDrop = false;
        //    webBrowser1.IsWebBrowserContextMenuEnabled = false;
        //    webBrowser1.ScriptErrorsSuppressed = true;
        //    webBrowser1.WebBrowserShortcutsEnabled = false;
        //    webBrowser1.Navigate("http://player.kuwo.cn/webmusic/play");
        //    webBrowser1.Hide();
        //    btnWebSearch.Text = "东风破";

        //    init();



        //}
        private void init()
        {
            //GetSearchList();
            //var song1 = new Song();
            //song1.Title = "1";
            //userctlWebList.AddItem(song1);
            //txtPlayList.PlayItemClick += (s, e) =>
            //{
            //    txtPlayList.Items[e.Index].PlayHE.OnClick();
            //};
            //txtPlayList.DelPlayItem += (s, e) =>
            //{
            //    txtPlayList.Items[e.Index].DelHE.OnClick();
            //    txtPlayList.DelItem(e.Index);
            //};
            userctlWebList.PlayItemDoubleClick += (s, e) =>
            {

                userctlWebList.Items[e.Index].PlayHE.OnClick();
            };
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 800 };
            timer.Tick += (s, e) =>
            {
                if (webBrowser1.Document == null) return;
                var dom = webBrowser1.Document.GetElementById("artist_Image");
                if (dom == null) return;

                // lblPlayTime.Text = webBrowser1.Document.GetElementById("wp_playTime").InnerText;
                // lblTotalTime.Text = webBrowser1.Document.GetElementById("wp_totalTime").InnerText;
                //public System.Windows.Forms.PictureBox picArtist;
                var src = dom.GetAttribute("src");

                if (src == frmxiMusic.picArtist.ImageLocation) return;
                frmxiMusic.picArtist.ImageLocation = src;
                // lblTitle.Text = webBrowser1.Document.GetElementById("wp_text").InnerText.Split(' ')[0];
                // Text = lblTitle.Text;
                //播放列表
                // txtPlayList.Clear();             
                foreach (HtmlElement he in webBrowser1.Document.GetElementById("playlbId1").GetElementsByTagName("a"))
                {
                    if (he.Attr("title") != "播放歌曲" && he.Attr("href").IndexOf("javascript:playSong(") == 0)
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            var song = new Song();
                            song.PlayHE = he;
                            song.Title = he.InnerText;
                            song.SongID = GetNumber(he.OuterHtml);

                            he.Parent.Parent.EachByTagName("li", (o) =>
                            {
                                if (o.OuterHtml.IndexOf("iSonger") > -1)
                                {
                                    song.SearchHE = o.Children[0];
                                    song.Artist = o.Children[0].InnerText;
                                }
                            });
                            he.Parent.Parent.EachByTagName("a", (o) =>
                            {
                                if (o.OuterHtml.IndexOf("lj_icon") > -1)
                                {
                                    song.DelHE = o;
                                }
                            });
                            // txtPlayList.AddItem(song);
                        }));
                    }
                }
            };
            timer.Start();

        }

        private void GetSearchList()
        {
            //延时获取数据
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 800 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                userctlWebList.Clear();
                foreach (HtmlElement he in webBrowser1.Document.GetElementById("search_con").GetElementsByTagName("a"))
                {
                    if (he.GetAttribute("title") != "播放歌曲" && he.GetAttribute("href").IndexOf("javascript:webBrowser1_playSong(") == 0)
                    {
                        this.Invoke((EventHandler)(delegate
                        {
                            var song = new Song { Title = he.InnerText, PlayHE = he };
                            userctlWebList.AddItem(song);
                        }));
                    }
                }
            };
            timer.Start();
            var song1 = new Song();
            song1.Title = "1";
            userctlWebList.AddItem(song1);
        }


        public static string GetNumber(string str)
        {
            return str = Regex.Replace(str, @"[^/d./d]", "");
        }

        private void picCloseWindow2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs ex, string kw)
        {
            webBrowser1.Document.GetElementById("search_key").Val(kw);
            webBrowser1.Document.GetElementById("search_key").NextSibling.OnClick();
            GetSearchList();
        }

        private void picCloseWindow3_Click(object sender, EventArgs e)
        {
            picCloseWindow3.Image = Properties.Resources.close_click;
            this.Close();
            frmXiMusic.isFrmSearchMusicShow = false;

        }

        private void picCloseWindow3_MouseEnter(object sender, EventArgs e)
        {
            picCloseWindow3.Image = Properties.Resources.close_highlisht;
        }

        private void picCloseWindow3_MouseLeave(object sender, EventArgs e)
        {
            picCloseWindow3.Image = Properties.Resources.close_normal;
        }

        private  void timer1_Tick(object sender, EventArgs e)
        {
           if(CommonData.songId!=id)
            {
                try
                {
                    CommonData.songUrl = searchMusicList[CommonData.songId].MusciURL;
                    CommonData.songName = searchMusicList[CommonData.songId].MusicName;
                }
                catch (Exception)
                {
                    CommonData.songUrl = searchMusicList[CommonData.songId-1].MusciURL;
                    CommonData.songName = searchMusicList[CommonData.songId-1].MusicName;
                }
            }
        }


        public void CreateSearchControl(object sender, EventArgs e)
        {
            Panel panName1 = new System.Windows.Forms.Panel();
            panName1.Dock = System.Windows.Forms.DockStyle.Top;
            panName1.Location = new System.Drawing.Point(0, 0);
            panName1.Name = "panelSearchMusic";
            panName1.Size = new System.Drawing.Size(279, 36);
            panName1.BackColor = Color.CadetBlue;

            Panel panelSearch = new Panel();
            panelSearch.Name = "panelOnlineMusic";
            panelSearch.Size = new Size(220, 26);
            panelSearch.Location = new Point(60, 5);
            panelSearch.BackgroundImage = Properties.Resources.search_edit;
            panelSearch.BackgroundImageLayout = ImageLayout.Stretch;

            System.Windows.Forms.TextBox txtSearch = new TextBox();
            txtSearch.Name = "txtSerarch";
            txtSearch.Location = new Point(8, 7);
            txtSearch.Width = 150;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Text = "";

            Panel panelSearchMusic = new Panel();
            panelSearchMusic.Size = new Size(17, 17);
            panelSearchMusic.Location = new Point(195, 4);
            panelSearchMusic.BackgroundImage = Properties.Resources.search_btn;
            panelSearchMusic.BackgroundImageLayout = ImageLayout.Zoom;
            panelSearchMusic.BackColor = Color.White;
            panelSearchMusic.Cursor = Cursors.Hand;

            //搜索按钮点击事件
            panelSearchMusic.Click += new EventHandler(panelSearchMusic_Click);
            txtSearch.KeyPress += new KeyPressEventHandler(txtSearch_KeyPress);
            txtSearch.Click += new EventHandler(txtSearch_Click);

            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(panelSearchMusic);
            panName1.Controls.Add(panelSearch);
            this.pMusicList.Controls.Add(panName1);
            this.pMusicList.Refresh();
            if (isFirst == true)
            {
                txtSearch.Text = "katy perry";
                panelSearchMusic_Click(sender, e);
                isFirst = false;
            }
        }


    }



    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ScriptAfflux
    {
        public List<Song> PlayList { get; set; }

        /// <summary>
        /// 获得播放列表
        /// </summary>
        /// <param name="str"></param>
        public void GetPlayList(string str)
        {
            PlayList = new List<Song>();
            var arr = JArray.Parse(str);
            foreach (JToken t in arr)
            {
                PlayList.Add(new Song
                {
                    SongID = t.Value<string>("rid"),
                    Album = t.Value<string>("album"),
                    Artist = t.Value<string>("artist"),
                    Title = t.Value<string>("name"),
                });
            }
        }
    }

    public class CommonData
    {
        public static int songId;
        public static string songUrl;
        public static string songName;
    }

}







