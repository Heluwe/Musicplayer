using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace XiMusic
{
    public partial class frmXiMusic : Form
    {
        //

        List<System.Windows.Forms.Timer> timers = new List<System.Windows.Forms.Timer>();
        #region 窗体边框阴影效果变量申明
        const int CS_DROPSHADOW = 0x20000;
        const int GCL_STYLE = (-26);
        //声明Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        #endregion
        string[] names;//歌曲路径
        string[] lists = new string[100];
        List<string> list;
        //int voice;//声音
        int musicNum = 0;
        private int fmIndex = 0;
        bool isMute = false;
        bool isListAmpty = true;
        bool playState = false;
        bool isControlRoll = false;
        bool isVisible = false;
        public bool isOnFm = false;
        bool isSkinShow = false;
        int musicPage = 1;
        string picfile;//保存copy源
        string picName;
        string[] str = { "BBC World Service", "豆瓣fm", "音乐之声", "CRI News Center", "虾米电台", "CRI Language Studio", "乔布斯斯坦福大学演讲" };
        private string[] Ltime = new string[200];
        private string[] Ltext = new string[200];
        //计算左右偏移
        private string bigTime;
        private string smallTime;
        private bool nextLrc = false;

        public static bool isLyricsOpen
        {
            get; set;
        }
        public static bool isFrmSearchMusicShow
        {
            get; set;
        }
        protected internal AnchorStyles StopAanhor = AnchorStyles.None;
        protected internal frmDesktopLyrics frmdesktopLyrics;
        protected internal LyricsHelper lyricsHelper;
        protected internal LrcRegex lrcRegex = new LrcRegex();
        protected internal Thread threadSearchLrc;
        public frmXiMusic()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            userctlMusicList.BackColor = Color.FromArgb(230, 238, 240);
            loadDesktopLyrics();
            isLyricsOpen = true;
            isFrmSearchMusicShow = false;
            IniFiles ini = new IniFiles(Application.StartupPath + "\\Ini\\config.ini");
            //LoadDiskLrcPanel();
            //加true参数代表是网络访问是需要代理，否则设置为false。
            this.lyricsHelper = new LyricsHelper(false);
            //代理登录回调函数初始化
            this.lyricsHelper.InitializeProxy += new EventHandler(InitializeProxy);
            //在线搜索错误回调函数初始化
            this.lyricsHelper.WebException += new EventHandler(WebException);
            //返回搜索查询结果回调函数初始化
            this.lyricsHelper.SelectSong += new EventHandler(SelectSong);
        }

        #region 窗体拖动
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键        
        private void frmXiMusic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void frmXiMusic_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void frmXiMusic_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
        #endregion
        public void ControlBringToFromt()
        {
            picArtist.BringToFront();
            picPlaySong.BringToFront();
            picPrevSong.BringToFront();
            picNextSong.BringToFront();      
            picSoundControl.BringToFront();
            picAddMusic.BringToFront();
            picSkin.BringToFront();
            picSearchShow.BringToFront();
            picSettings.BringToFront();
            picLyricsShow.BringToFront();
            picNetwork.BringToFront();
            lblTopic.BringToFront();
            picTopic.BringToFront();
            picClearList.BringToFront();
        }  
        public void PlayMusic(string namepath)
        {        
            if (isControlRoll == true)
            {         
                ControlRoll();
            }
            this.axWindowsMediaPlayer1.URL = namepath;
            if (PlayType.collection == PlayerType.type)
            {
                picPlaySong.Image = XiMusic.Properties.Resources.pause_down;
                playState = true;
                timer1.Enabled = true;
                timer2.Enabled = true;
                axWindowsMediaPlayer1.settings.autoStart = true;
                threadSearchLrc = new Thread(new ThreadStart(SerchLrc));
                threadSearchLrc.Start();
            }
            else
            {
                picArtist.Image = Properties.Resources.local_artist;
                lblMusicName.Text = axWindowsMediaPlayer1.currentMedia.name;
                picPlaySong.Image = XiMusic.Properties.Resources.pause_down;
                playState = true;
                timer1.Enabled = true;
                timer2.Enabled = true;
                axWindowsMediaPlayer1.settings.autoStart = true;
                threadSearchLrc = new Thread(new ThreadStart(SerchLrc));
                threadSearchLrc.Start();
            }
        }

  

        private void SearchLrcPanel()
        {
            if (PlayerType.type == PlayType.collection)
            {
                frmSearchLyrics frmsearchLyrics = new frmSearchLyrics("", lblMusicName.Text);
                frmsearchLyrics.LoadMusicLrc += new frmSearchLyrics.LoadMusicLrcHandler(SerchLrc);
                frmsearchLyrics.Show();
            }
            else
            {
                frmSearchLyrics frmsearchLyrics = new frmSearchLyrics(getAbsolutePath(names[musicNum]), getFileName(names[musicNum]));

                frmsearchLyrics.LoadMusicLrc += new frmSearchLyrics.LoadMusicLrcHandler(SerchLrc);
                frmsearchLyrics.Show();
            }
        }

        private void picPlaySong_Click(object sender, EventArgs e)
        {
            if (isOnFm == false)
            {
                if (!File.Exists(".//Music.lst"))
                {
                    MessageBox.Show("列表为空，请添加歌曲或播放网络歌曲");
                }
                else
                {
                    if (playState == false)
                    {

                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        // PlayMusic(names[musicNum]);
                        // timer2.Enabled = true;
                        picPlaySong.Image = XiMusic.Properties.Resources.pause_down;
                        //PlayMusic(names[musicNum]);
                        playState = true;
                        threadSearchLrc = new Thread(new ThreadStart(SerchLrc));
                        threadSearchLrc.Start();
                        //if (axWindowsMediaPlayer1.playState.ToString() ==  "wmmppsstoped")
                        //    PlayMusic(names[musicNum++]);
                    }
                    else
                    {
                        // threadSearchLrc.Abort();
                        axWindowsMediaPlayer1.Ctlcontrols.pause();
                        timer2.Enabled = false;
                        picPlaySong.Image = XiMusic.Properties.Resources.play_down;
                        playState = false;
                        // threadSearchLrc.Suspend();
                    }
                }
            }
        }


        private void picAddMusic_Click(object sender, EventArgs e)
        {
            picAddMusic.Image = Properties.Resources.add_click;
            list = new List<string>();
            string[] oldFile;//以前的列表
            string[] newFile;//新的列表
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "*.mp3;*.wma;*.wav|*.mp3;*.wma;*.wav";
            ofd.RestoreDirectory = true;
            ofd.FilterIndex = 1;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                int k = 0;
                int same = 0; //记录相同数量
                if (names == null)
                {
                    oldFile = new string[ofd.FileNames.Length];
                    foreach (var i in ofd.FileNames)
                    {
                        oldFile[k] = i;
                        k++;
                    }
                }
                else
                {
                    oldFile = new string[ofd.FileNames.Length + names.Length];
                    for (int y = 0; y < names.Length; y++)
                    {
                        oldFile[k] = names[y];
                        k++;
                    }
                    foreach (var i in ofd.FileNames)
                    {
                        oldFile[k] = i;
                        k++;

                    }
                }
                for (int i = 0; i < oldFile.Length; i++)
                {
                    for (int j = i + 1; j < oldFile.Length; j++)
                    {
                        if (oldFile[i] == oldFile[j])
                        {
                            same++;
                        }
                    }
                }
                for (int i = 0; i < oldFile.Length; i++)
                {
                    for (int j = i + 1; j < oldFile.Length; j++)
                    {
                        if (oldFile[i] == oldFile[j])
                        {
                            oldFile[i] = "null";
                        }
                    }
                }
                //消除重复歌曲
                int w = 0;
                newFile = new string[oldFile.Length - same];
                for (int i = 0; i < oldFile.Length; i++)
                {
                    if (oldFile[i] != "null")
                    {
                        newFile[w] = oldFile[i];
                        w++;
                    }
                }

                names = newFile;
                for (int i = 0; i < names.Length; i++)
                {
                    list.Add(names[i]);
                }
                SaveuserMusicList();
            }
        }

        //save方法
        public void SaveuserMusicList()
        {

            if (File.Exists(".\\Music.lst") == true)
            {
                File.Delete(".\\Music.lst");
            }
            SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = "Music.lst";
            sf.RestoreDirectory = true;
            sf.FilterIndex = 1;
            FileStream fs = new FileStream(string.Format("{0}", sf.FileName), FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, list);
            fs.Close();
            musicNum = names.Length - 1;
            userctlMusicList.Clear();
            GetuserMusicList();
            //  userctlMusicList.Refresh();
            //  PlayMusic(names[musicNum]);
            picPlaySong.Image = XiMusic.Properties.Resources.pause_down;
        }
        //读取方法
        public void GetuserMusicList()
        {

            string[] musicFile;
            if (File.Exists(".\\Music.lst") == false)
            {
                MessageBox.Show("列表为空，请添加歌曲");
            }
            else
            {
                isListAmpty = false;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = "Music.lst";
                ofd.RestoreDirectory = true;
                ofd.FilterIndex = 1;
                FileStream fs = new FileStream(string.Format("{0}", ofd.FileName), FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                this.list = ((List<string>)bf.Deserialize(fs));
                fs.Close();
                musicFile = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    musicFile[i] = list[i];
                }
                names = musicFile;
            }
            int j = 0;
            if (isListAmpty == false)
            {
                for (j = 0; j < list.Count; j++)
                {
                    var song = new Song();
                    song.Title = getFileName(names[j]);
                    userctlMusicList.AddItem(song);
                }
            }

        }
        protected void CloseLrcPanel()
        {

            this.frmdesktopLyrics.manualReset.Set();
            this.frmdesktopLyrics.thread.Abort();
            this.frmdesktopLyrics.Close();
            this.frmdesktopLyrics = null;
        }     
        /// <summary>
        /// 解锁歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void tsmiUnLockLrcPanel_Click(object sender, EventArgs e)
        //{
        //    if (this.frmdesktopLyrics != null)
        //    {
        //        this.frmdesktopLyrics.IsLockPanel = false;
        //    }
        //}

        #region 歌词搜索处理

        /// <summary>
        /// 返回查询结果回调函数，显示到列表上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
       // string path;
        protected void SelectSong(object sender, EventArgs e)
        {
            //返回Xml形式的搜索结果
            XmlNodeList list = sender as XmlNodeList;
            string musicName = getFileName(names[musicNum]);// 
            //判断是否搜索到歌词信息
            if (list != null)
            {
                //如果歌词集合为0，表示没有搜索结果
                if (list.Count == 0)
                {
                    this.tmrLrc.Stop();
                    //如果没有搜索结果，歌词面板显示手动下载链接
                    this.frmdesktopLyrics.LrcText = "未搜索到歌词！请尝试手动下载。";
                    this.frmdesktopLyrics.IsAfreshDraw = true;
                    timer2.Enabled = false;
                    timer1.Enabled = true;
                    //SetMusicTime();
                    //lblCurrentTime.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
                    //lblTotalTime.Text = axWindowsMediaPlayer1.currentMedia.durationString;
                }
                else
                {
                    //默认选择第一条歌词信息作为下载对象
                    this.lyricsHelper.CurrentSong = list[0];
                    //下载歌词文件
                    bool flag = this.lyricsHelper.DownloadLrc(musicName + ".lrc");
                    //判断歌词文件是否下载成功
                    if (flag)
                    {
                        IList<Attribute> attributeList = new List<Attribute>();
                        attributeList.Add(new Attribute() { AttributeName = "name", AttributeValue = getAbsolutePath(names[musicNum]) });
                        attributeList.Add(new Attribute() { AttributeName = "lrc", AttributeValue = string.Format("{0}\\lrc\\{1}.lrc", Application.StartupPath, musicName) });

                        //将歌词文件与歌曲绑定存入XML
                        // XmlHelper.Insert("\\xml\\lrclist.xml", "lrclist", "music", attributeList);
                        XmlHelper.Insert(Application.StartupPath + "\\xml\\lrclist.xml", "lrclist", "music", attributeList);
                        // XmlHelper.Insert()
                        //加载歌词
                        ChangeLable(string.Format("{0}\\lrc\\{1}.lrc", Application.StartupPath, musicName));///
                    }
                    else
                    {
                        //如果下载不成功，歌词面板显示手动下载链接
                        LoadLrcText("歌词下载失败！请尝试手动下载。");
                    }
                }
            }
            else
            {
                //如果下载不成功，歌词面板显示手动下载链接
                LoadLrcText("未搜索到歌词！请尝试手动下载。");
            }
        }

        /// <summary>
        /// 加载歌词
        /// </summary>
        /// <param name="lrcText">歌词信息</param>
        protected void LoadLrcText(string lrcText)
        {
            if (this.frmdesktopLyrics != null)
            {
                //如果下载不成功，歌词面板显示手动下载链接
                this.frmdesktopLyrics.LrcText = lrcText;
                this.frmdesktopLyrics.IsAfreshDraw = true;
            }
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="lyricsHelper"></param>
        /// <param name="songer"></param>
        /// <param name="musicName"></param>
        public void SearchLrcFrm_OnSearchLrc()
        {
            //搜索歌词,歌手、歌名
            this.lyricsHelper.SearchLrc("", getFileName(names[musicNum]));//当前播放曲目
        }

        /// <summary>
        /// 在线搜索错误接受回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WebException(object sender, EventArgs e)
        {
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

        }

        /// <summary>
        /// 搜索歌词信息
        /// </summary>
        /// 

        public void SerchLrc()
        {

            LoadLrcText("LOADING...");
            timer2.Enabled = true;
            //timer1.Enabled = false;
            try
            {
                //if (PlayerType.type == PlayType.collection)
                //{

                if (File.Exists(string.Format("{0}\\lrc\\{1}.lrc", Application.StartupPath, lblMusicName.Text)))
                    ChangeLable(string.Format("{0}\\lrc\\{1}.lrc", Application.StartupPath, lblMusicName.Text));
                else
                {
                    LoadLrcText("未搜索到歌词！请尝试手动下载。");

                    ChangeLable(string.Format("{0}\\lrc\\{1}.lrc", Application.StartupPath, lblMusicName.Text));


                }

            }
            //    else
            //    {
            //        string attribute = XmlHelper.ReadElementAttributeValue(string.Format("{0}\\xml\\lrclist.xml", Application.StartupPath), "music", "name", getAbsolutePath(names[musicNum]), "lrc");
            //        if (File.Exists(attribute))
            //        {
            //            ChangeLable(attribute);
            //        }
            //        else
            //        {
            //            if (PlayerType.type == PlayType.collection)
            //            {
            //                this.lyricsHelper.SearchLrc("", lblMusicName.Text);
            //            }
            //            else
            //                this.lyricsHelper.SearchLrc("", getFileName(names[musicNum]));
            //        }
            //    }
            //}

            //if (File.Exists(attribute))
            //  {
            //       ChangeLable(attribute);
            // }
            //else
            //  {
            //       if(PlayerType.type==PlayType.collection)
            //       {
            //           this.lyricsHelper.SearchLrc("", lblMusicName.Text);
            //       }
            //           else
            //      this.lyricsHelper.SearchLrc("", getFileName(names[musicNum]));
            //   }
            // }
            catch (Exception)
            {

                LoadLrcText("未搜索到歌词！请尝试手动下载。");
                // ChangeLable(string.Format("{0}\\lrc\\{1}.lrc", Application.StartupPath, lblMusicName.Text));
                //  lblMusicName.Text = frmsearchMusic.searchMusicList[frmsearchMusic.id - 1].MusicName;
            }
        }

        /// <summary>
        /// 改变歌词lable的方法 并且加载显示歌词方法
        /// </summary> 
        /// <param name="text">传入的返回值</param>
        private void ChangeLable(string lrcPath)
        {
            this.Ltime = new string[200];
            this.Ltext = new string[200];
            this.lrcRegex.GetLrc(lrcPath);
            this.Ltext = lrcRegex.ReturnText();
            this.Ltime = lrcRegex.ReturnTime();
            if (this.frmdesktopLyrics != null)
            {
                //设置当前歌曲时间   改
                this.frmdesktopLyrics.MusicTime = axWindowsMediaPlayer1.Ctlcontrols.currentPosition * 1000;//(int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition; //this.bassPlayer.ChannelLength.TotalMilliseconds;231471.0;
            }
            //      this.tmrLrc.Start();
        }

        /// <summary>
        /// 偏移量计算
        /// </summary>
        public void LeftRight()
        {
            string time = this.lblCurrentTime.Text + ":00";
            try
            {
                int start = int.Parse(time.Substring(0, 2));
                string zj = time.Substring(3, 2);
                int zjnum = int.Parse(zj);
                this.bigTime = time.Substring(0, 3) + (zjnum + 2).ToString() + ":00";
                if (zjnum >= 2)
                {
                    this.smallTime = time.Substring(0, 3) + (zjnum - 2).ToString() + ":00";
                }
                else if (zjnum == 00 && start > 0)
                {
                    this.smallTime = "0" + (start - 1).ToString() + ":" + 58.ToString() + ":00";
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 歌词字幕切换计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrLrc_Tick(object sender, EventArgs e)
        {
            string time = this.lblCurrentTime.Text + ":00";

            if (this.nextLrc == true)
            {
                LeftRight();    //计算偏移量
                for (int i = 0; i < 100; i++)
                {
                    if (Ltime[i] == this.bigTime || Ltime[i] == this.smallTime)
                    {
                        //判断当前歌词窗是否被打开，如果被打开则更新歌词字幕
                        if (this.frmdesktopLyrics != null)
                        {
                            //this.lyricsFrm.IsSearch = true;
                            //更新设置当前歌词文字
                            //this.lyricsFrm.LrcText = Ltext[i];
                            DateTime startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:") + Ltime[i].Substring(0, 5));

                            this.frmdesktopLyrics.CurrentMusicStartTime = startTime.Minute * 60 * 1000 + startTime.Second * 1000 + startTime.Millisecond;
                            if (Ltime[i + 1] != null)
                            {
                                DateTime endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:") + Ltime[i + 1].Substring(0, 5));
                                this.frmdesktopLyrics.CurrentMusicEndTime = endTime.Minute * 60 * 1000 + endTime.Second * 1000 + endTime.Millisecond; ;
                            }
                            else
                            {
                                this.frmdesktopLyrics.CurrentMusicEndTime = axWindowsMediaPlayer1.currentMedia.duration * 1000;
                            }
                            LoadLrcText(Ltext[i]);
                            nextLrc = false;
                        }
                    }
                }
            }

            for (int i = 0; i < 100; i++)
            {
                //判断当前时间是否有匹配的歌词字幕
                if (time == Ltime[i])
                {
                    //判断当前歌词窗是否被打开，如果被打开则更新歌词
                    if (this.frmdesktopLyrics != null)
                    {
                        //this.lyricsFrm.IsSearch = true;
                        //更新设置当前歌词字幕
                        //this.lyricsFrm.LrcText = Ltext[i];     
                        DateTime startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:") + Ltime[i].Substring(0, 5));

                        this.frmdesktopLyrics.CurrentMusicStartTime = startTime.Minute * 60 * 1000 + startTime.Second * 1000 + startTime.Millisecond;
                        if (Ltime[i + 1] != null)
                        {
                            DateTime endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:") + Ltime[i + 1].Substring(0, 5));
                            this.frmdesktopLyrics.CurrentMusicEndTime = endTime.Minute * 60 * 1000 + endTime.Second * 1000 + endTime.Millisecond; ;
                        }
                        else
                        {
                            this.frmdesktopLyrics.CurrentMusicEndTime = axWindowsMediaPlayer1.currentMedia.duration * 1000;//this.bassPlayer.ChannelLength.TotalMilliseconds;
                        }
                        LoadLrcText(Ltext[i]);
                    }
                }
            }

            //更新当前歌词指针
            if (this.frmdesktopLyrics != null)
            {
                this.frmdesktopLyrics.CurrentTime = axWindowsMediaPlayer1.Ctlcontrols.currentPosition * 1000;//(int)axWindowsMediaPlayer1.currentMedia.duration;//this.bassPlayer.ChannelPosition.TotalMilliseconds;
                //更新可调面板
                this.frmdesktopLyrics.IsAfreshDraw = true;
            }
        }

        /// <summary>
        /// 将时间表示成00:00的格式。
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        private string GetStardandTimeString(TimeSpan timeSpan)
        {
            return string.Format("{0}:{1}", timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
        }

        #endregion    

        protected void loadDesktopLyrics()
        {
            this.frmdesktopLyrics = new frmDesktopLyrics();
            //关闭歌词面板回调函数
            this.frmdesktopLyrics.CloseLrcPanel += new frmDesktopLyrics.CloseLrcPanelHandler(CloseLrcPanel);   
            //搜索歌词回调函数
            this.frmdesktopLyrics.SearchLrcPanel += new frmDesktopLyrics.SearchLrcPanelHandler(SearchLrcPanel);
            this.frmdesktopLyrics.Init("WAITING...", 0);
            this.frmdesktopLyrics.Show();
        }


        private void frmXiMusic_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            GetuserMusicList();
            userMusicListPlay();
            try
            {
                // axWindowsMediaPlayer1.settings.autoStart = false;
                this.axWindowsMediaPlayer1.settings.volume = 40;
                pnlCurrentVoice.Size = new Size(40, 3);
                //PlayMusic(names[musicNum]);
            }
            catch (Exception)
            {

            }

     
        }

        private void fmUrl(string fm)
        {
            //webBrowser1.ObjectForScripting = sa;
            // webBrowser1.AllowWebBrowserDrop = false;
            //webBrowser1.IsWebBrowserContextMenuEnabled = false;
            // webBrowser1.ScriptErrorsSuppressed = true;
            // webBrowser1.WebBrowserShortcutsEnabled = false;

            if (fm == "BBC World Service")
                webBrowser1.Navigate("http://www.941gb.com/guowai-dt/bbc-zxst");//英国bbc
            else if (fm == "豆瓣fm")
                webBrowser1.Navigate("http://douban.fm");//心理fm
            else if (fm == "音乐之声")
                webBrowser1.Navigate("http://www.zueiai.net/radio/zy/8.html");//音乐之声                            
            else if (fm == "CRI News Center")
                webBrowser1.Navigate("http://www.zueiai.net/radio/zy/21.html");//CRI News Center（AM846）
            else if (fm == "虾米电台")
                webBrowser1.Navigate("http://www.xiami.com/radio");
            else if (fm == "CRI Language Studio")
                webBrowser1.Navigate("http://www.zueiai.net/radio/zy/22.html");
            else if (fm == "乔布斯斯坦福大学演讲")
                webBrowser1.Navigate("http://v.163.com/movie/2006/8/3/8/M7BC8JMHJ_M7BC8PA38.html");
            lblMusicName.Text = str[fmIndex];
        }

        int j = 0;
        private void userMusicListPlay()
        {



            userctlMusicList.PlayItemDoubleClick += (s, e) =>
            {
                if (isOnFm == false)
                {
                    userctlMusicList.Items[e.Index].PlayHE.OnClick();
                    musicNum = e.Index;
                    PlayerType.type = PlayType.local;
                    PlayMusic(names[musicNum + (musicPage - 1) * userctlMusicList.MaxSize]);
                    picPlaySong.Image = Properties.Resources.pause_down;
                }
                else
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    fmIndex = e.Index;
                    fmUrl(str[e.Index]);
                }
            };
            userctlMusicList.DelPlayItem += (s, e) =>
            {
                j = 0;
                string[] namesCopy = new string[names.Length - 1];
                userctlMusicList.Items[e.Index].DelHE.OnClick();
                userctlMusicList.DelItem(e.Index);
                for (int i = 0; i < names.Length; i++)
                {
                    if (e.Index != i)
                        namesCopy[j++] = names[i];
                }
                names = namesCopy;
                if (musicNum > e.Index)
                    musicNum--;
                if (musicNum == e.Index && PlayType.local == PlayerType.type)
                {
                    //musicNum++;
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    try
                    {
                        PlayMusic(names[musicNum]);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            PlayMusic(names[--musicNum]);
                        }
                        catch (Exception)
                        {
                            axWindowsMediaPlayer1.Ctlcontrols.stop();
                        }
                    }
                }
                //else if(musicNum>e.Index)
                //    musci

                //ArrayList ar = new ArrayList(names);
                //ar.Remove(e.Index);
                // PlayMusic(names[e.Index + 1]);
                //userctlMusicList.Clear();
                //GetuserMusicList();

            };
        }

        private string getFileName(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }
        private string getFileDirectory(string path)
        {
            return Path.GetDirectoryName(path);
        }
        private string getAbsolutePath(string path)
        {
            return getFileDirectory(names[musicNum]) + "\\" + Path.GetFileName(names[musicNum]);
        }
        double alltime;//全部时间
        double currenttime;//当前时间
        double bfb;//百分比
        double thisX;
        private void SetMusicTime()
        {
            currenttime = this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            alltime = this.axWindowsMediaPlayer1.currentMedia.duration;
            bfb = currenttime / alltime;
            thisX = 255 * bfb;
            pnlCurrentProgress.Size = new Size((int)thisX, 3);
        }
        //public frmSearchMusic frmsearchMusic = null;
        // public frmSearchMusic frmsearchMusic(this);
        private void picNextSong_Click(object sender, EventArgs e)
        {
            //frmSearchMusic frmsearchMusic = new frmSearchMusic(this);
            if (PlayerType.type == PlayType.local && isOnFm == false)
            {
                if (!File.Exists(".//Music.lst"))
                {
                    MessageBox.Show("列表为空，请添加歌曲");
                }
                else
                {
                    musicNum++;
                    try
                    {
                        if (playState == false)
                            picPlaySong.Image = XiMusic.Properties.Resources.pause_down;
                        PlayMusic(names[musicNum]);
                    }
                    catch (Exception)
                    {
                        musicNum = 0;
                        PlayMusic(names[musicNum]);
                        //  MessageBox.Show("已是最后一曲了");
                    }
                }
            }
            else
            {
                try
                {
                    fmIndex++;
                    fmUrl(str[fmIndex]);
                }
                catch (Exception)
                {
                    fmIndex = 0;
                    fmUrl(str[fmIndex]);
                }



                // frmsearchMusic.id += 1;
                // PlayMusic(frmsearchMusic.searchMusicList[frmsearchMusic.id - 1].MusciURL);
                //  CommonData.songId += 1;

                // axWindowsMediaPlayer1.URL = CommonData.songUrl;

                //  lblMusicName.Text = CommonData.songName;
                //  PlayMusic(CommonData.songUrl);
            }
            //网络列表中下一曲
        }
        private void picPrevSong_Click(object sender, EventArgs e)
        {
            if (PlayerType.type == PlayType.local && isOnFm == false)
            {
                if (!File.Exists(".//Music.lst"))
                {
                    MessageBox.Show("列表为空，请添加歌曲");
                }
                else
                {
                    musicNum--;
                    // threadSearchLrc.Abort();
                    try
                    {
                        if (playState == false)
                            picPlaySong.Image = XiMusic.Properties.Resources.pause_down;
                        PlayMusic(names[musicNum]);
                    }
                    catch (Exception)
                    {
                        //musicNum++;
                        // MessageBox.Show("已是最后一曲了");
                        musicNum = names.Length - 1;
                        PlayMusic(names[musicNum]);
                    }
                }
            }
            else
            {
                try
                {
                    fmIndex--;
                    fmUrl(str[fmIndex]);
                }
                catch (Exception)
                {
                    fmIndex = str.Length - 1;
                    fmUrl(str[fmIndex]);
                }

                // CommonData.songId -= 1;
                //// axWindowsMediaPlayer1.URL = CommonData.songUrl;
                // lblMusicName.Text = CommonData.songName;
                // PlayMusic(CommonData.songUrl);
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            tmrLrc.Enabled = true;
            SetMusicTime();
            lblCurrentTime.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            lblTotalTime.Text = axWindowsMediaPlayer1.currentMedia.durationString;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // tmrLrc.Start();
            SetMusicTime();
            lblCurrentTime.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            lblTotalTime.Text = axWindowsMediaPlayer1.currentMedia.durationString;
        }
        int currentTimePanleX;//获取当前panle的X
                              //  bool next = false;
                              //  double oldTime, newTime;
                              // private void timer1_Tick(object sender, EventArgs e)

        private void pnlCurrentProgress_MouseDown(object sender, MouseEventArgs e)
        {
            pnlCurrentProgress.Size = new Size(e.Location.X, 3);
            currentTimePanleX = e.Location.X;
            ChangeTime(255, currentTimePanleX);
            // next = true;
        }

        private void pnlCurrentProgress_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pnlCurrentProgress_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void pnlProgressLength_MouseDown(object sender, MouseEventArgs e)
        {
            pnlCurrentProgress.Size = new Size(e.Location.X, 3);//改变到鼠标点击的位置,e为鼠标点击事件
            currentTimePanleX = e.Location.X;//currentProgress长度
            ChangeTime(255, currentTimePanleX);
            //next = true;
        }

        private void pnlProgressLength_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pnlProgressLength_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        //获取当前进度
        double thisTime;
        Double b;

        private void pnlCurrentVoice_MouseDown(object sender, MouseEventArgs e)
        {

            axWindowsMediaPlayer1.settings.volume = e.Location.X;
            pnlCurrentVoice.Size = new Size(e.Location.X, 3);
        }

        private void pnlCurrentVoice_MouseEnter(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pnlCurrentVoice_MouseLeave(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void pnlVoiceLength_MouseDown(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = e.Location.X;
            pnlCurrentVoice.Size = new Size(e.Location.X, 3);
        }

        private void pnlVoiceLength_MouseEnter(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.Hand;
        }
        private void pnlVoiceLength_MouseLeave(object sender, EventArgs e)
        {
            Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void picSoundControl_Click(object sender, EventArgs e)
        {
            if (isMute == false)
            {
                axWindowsMediaPlayer1.settings.mute = true;
                isMute = true;
                picSoundControl.Size = new Size(11, 17);
                picSoundControl.SizeMode = PictureBoxSizeMode.Normal;
                picSoundControl.Location = new Point(241, 164);
            }
            else
            {
                axWindowsMediaPlayer1.settings.mute = false;
                isMute = false;
                picSoundControl.Size = new Size(28, 24);
                picSoundControl.SizeMode = PictureBoxSizeMode.CenterImage;
                picSoundControl.Location = new Point(237, 160);
            }
        }



        private void picCloseWindow1_Click(object sender, EventArgs e)
        {
            picCloseWindow1.Image = Properties.Resources.close_click;
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void picMinimizeWindow_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void lbllikeList_Click(object sender, EventArgs e)
        {

        }

        private void lblDefaultList_Click(object sender, EventArgs e)
        {

        }

        private void picMinimizeWindow_MouseEnter(object sender, EventArgs e)
        {
            picMinimizeWindow.Image = Properties.Resources.minimize_highlight;
        }

        private void picMinimizeWindow_MouseLeave(object sender, EventArgs e)
        {
            picMinimizeWindow.Image = Properties.Resources.minimize_normal;
        }

        private void picCloseWindow1_MouseEnter(object sender, EventArgs e)
        {
            // picCloseWindow1.Image = XiMusic.Properties.Resources.btn_close_highlight;
            picCloseWindow1.Image = Properties.Resources.close_highlisht;
        }

        private void picCloseWindow1_MouseLeave(object sender, EventArgs e)
        {
            //picCloseWindow1.Image = XiMusic.Properties.Resources.btn_close_normal;
            picCloseWindow1.Image = Properties.Resources.close_normal;
        }


        public void ChangeTime(double all, double thisp)//改变到鼠标指向的地方
        {
            try
            {
                b = thisp / all;
                alltime = this.axWindowsMediaPlayer1.currentMedia.duration;
                thisTime = alltime * b;
                this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition = thisTime;
            }
            catch (Exception)
            {
                //
            }
        }

        private void pnlListContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void pnlLocalList_MouseEnter(object sender, EventArgs e)
        //{
        //    pnlLocalList.BackColor = Color.LightGray;
        //}

        //private void pnlLocalList_MouseLeave(object sender, EventArgs e)
        //{
        //    pnlLocalList.BackColor = Color.White;
        //}

        private void picLyricsShow_Click(object sender, EventArgs e)
        {
            if (isLyricsOpen == false)
            {
                loadDesktopLyrics();
                isLyricsOpen = true;
            }
            else
            {
                CloseLrcPanel();
                isLyricsOpen = false;
            }
        }

        private void picSkin_Click(object sender, EventArgs e)
        {
            //picSkin.Image = Properties.Resources.skin_click;
            if (isSkinShow == false)
            {
                pnlSkinCollection.Visible = true;
                isSkinShow = true;
            }
            else
            {
                pnlSkinCollection.Visible = false;
                isSkinShow = false;
            }
        }

        private void picAddMusic_MouseEnter(object sender, EventArgs e)
        {
            picAddMusic.Image = Properties.Resources.add_click;
        }

        private void picAddMusic_MouseLeave(object sender, EventArgs e)
        {
            picAddMusic.Image = Properties.Resources.add_highlight;
        }

        private void lblClearAll_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            File.Delete(".\\Music.lst");
            userctlMusicList.Clear();
            names = null;
            musicNum = 0;
            lblMusicName.Text = "";
        }

        private void picPlaySong_MouseEnter(object sender, EventArgs e)
        {
            if (playState == false)
                picPlaySong.Image = Properties.Resources.play_hover;
            else
                picPlaySong.Image = Properties.Resources.pause_hover;
        }

        private void picPlaySong_MouseLeave(object sender, EventArgs e)
        {
            if (playState == false)
                picPlaySong.Image = Properties.Resources.play_down;
            else
                picPlaySong.Image = Properties.Resources.pause_down;

        }

        private void picNextSong_MouseEnter(object sender, EventArgs e)
        {
            picNextSong.Image = Properties.Resources.next_hover;
        }

        private void picNextSong_MouseLeave(object sender, EventArgs e)
        {
            picNextSong.Image = Properties.Resources.next;
        }

        private void picPrevSong_MouseEnter(object sender, EventArgs e)
        {
            picPrevSong.Image = Properties.Resources.prev_hover;
        }

        private void picPrevSong_MouseLeave(object sender, EventArgs e)
        {
            picPrevSong.Image = Properties.Resources.prev;
        }

        private void picSoundControl_MouseEnter(object sender, EventArgs e)
        {
            picSoundControl.Image = Properties.Resources.soundhover;
        }

        private void picSoundControl_MouseLeave(object sender, EventArgs e)
        {
            picSoundControl.Image = Properties.Resources.sound;
        }

        private void userctlMusicList_PlayItemClick(object sender, ctlMusicList.PlayEventArgs e)
        {
            //userctlMusicList.ItemHoverColor = Color.SkyBlue;
        }
        public void Roll(System.Windows.Forms.Control ctl)
        {
            var rd = new Random((int)DateTime.Now.Ticks);
            ctl.Location = new Point(rd.Next(0, Width), rd.Next(20, 100));
            double g = 0.5, vx = rd.NextDouble() * 30, vy = 0;
            var tr = new System.Windows.Forms.Timer { Interval = 28, Enabled = true };
            timers.Add(tr);
            tr.Tick += (s, e) =>
            {
                this.Invoke(new EventHandler((o, j) =>
                {
                    ctl.Left += (int)vx;
                    ctl.Top += (int)vy;
                    vy += g;
                    var h = (Height - ctl.Height);
                    var w = (Width - ctl.Width);
                    if (ctl.Top <= 0)
                    {
                        ctl.Top = 0;
                        vy = Math.Abs(vy);
                    }
                    if (ctl.Top >= h)
                    {
                        ctl.Top = h;
                        vy = -(vy * 0.9);//模拟摩擦力
                    }
                    if (ctl.Left <= 0)
                    {
                        ctl.Left = 0;
                        vx = Math.Abs(vx);
                    }
                    if (ctl.Left >= w)
                    {
                        ctl.Left = w;
                        vx = -vx;
                    }
                }));
            };
        }

        public void ControlRoll()
        {
            //停止正在进行的动画效果
            timers.ForEach(a =>
            {
                a.Stop();
                a.Dispose();
            });
            //开始动画效果
            Roll(picArtist);             
            Roll(picPlaySong);        
            Roll(picNextSong);            
            Roll(picPrevSong);        
            Roll(picSoundControl);
            Roll(picAddMusic);
            Roll(picSkin);
            Roll(picSettings);
            Roll(picLyricsShow);
            Roll(picSearchShow);
            //Roll(picCtlRoll);
            Roll(picNetwork);
            Roll(picClearList);
            Roll(picTopic);
            ControlBringToFromt();
        }

        private void picArtist_MouseEnter(object sender, EventArgs e)
        {
            panel2.BackgroundImage = Properties.Resources.AlbumSelectBg;
        }

        private void picArtist_MouseLeave(object sender, EventArgs e)
        {
            panel2.BackgroundImage = null;
        }

        private void picSkin_MouseEnter(object sender, EventArgs e)
        {
            picSkin.Image = Properties.Resources.skin_click;
        }

        private void picSkin_MouseLeave(object sender, EventArgs e)
        {
            picSkin.Image = Properties.Resources.skin_highlight;
        }

        private void picMini_MouseEnter(object sender, EventArgs e)
        {
            picMini.Image = Properties.Resources.mini_highlight;
        }

        private void picMini_MouseLeave(object sender, EventArgs e)
        {
            picMini.Image = Properties.Resources.mini_normal;
        }

        private void picMini_Click(object sender, EventArgs e)
        {
            picMini.Image = Properties.Resources.mini_click;
            if (isVisible == false)
            {

                picNetwork.Visible = true;
                picSettings.Visible = true;
                picAddMusic.Visible = true;
                picSkin.Visible = true;
                picCtlRoll.Visible = true;
                isVisible = true;
                picLyricsShow.Visible = true;
                picSearchShow.Visible = true;
                lblTopic.Visible = false;
            }
            else
            {
                picNetwork.Visible = false;
                picSettings.Visible = false;
                picAddMusic.Visible = false;
                picSkin.Visible = false;
                picCtlRoll.Visible = false;
                isVisible = false;
                lblTopic.Visible = true;
                picLyricsShow.Visible = false;
                picSearchShow.Visible = false;
                
            }
        }

        private void picSettings_MouseEnter(object sender, EventArgs e)
        {
            picSettings.Image = Properties.Resources.settings_click;
        }

        private void picSettings_MouseLeave(object sender, EventArgs e)
        {
            picSettings.Image = Properties.Resources.settings_normal;
        }

        private void picSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("开发中...");
        }

        private void lblWebList_Click(object sender, EventArgs e)
        {

        }

        private void picClearList_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除列表中全部歌曲？", "", MessageBoxButtons.OKCancel) == DialogResult.OK&&isOnFm==false)
            {
                if (PlayerType.type == PlayType.local)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    File.Delete(".\\Music.lst");
                    userctlMusicList.Clear();
                    names = null;
                    musicNum = 0;
                    lblMusicName.Text = "";
                    musicPage = 1;
                }

                else
                {
                    File.Delete(".\\Music.lst");
                    userctlMusicList.Clear();
                    names = null;
                    musicNum = 0;
                    musicPage = 1;
                }
            }
        }
        private void picClearList_MouseEnter(object sender, EventArgs e)
        {
            picClearList.Image = Properties.Resources.clear_click;
        }

        private void picClearList_MouseLeave(object sender, EventArgs e)
        {
            picClearList.Image = Properties.Resources.clear_normal;
        }

        private void picSearchShow_Click(object sender, EventArgs e)
        {
            // frmSearchMusic frmsearchMusic = new frmSearchMusic(ref frmXiMusic frmxiMusic);
            frmSearchMusic frmsearchMusic = new frmSearchMusic(this);
            Multiwindows test2 = new Multiwindows(this, frmsearchMusic, MultiwindowsPosition.Right);
            frmsearchMusic.CreateSearchControl(sender, e);
            if (isFrmSearchMusicShow == false)
            {
                frmsearchMusic.Show();
                isFrmSearchMusicShow = true;
                //frmsearchMusic.pictureBox1.Image = Properties.Resources.btn_close_highlight;
            }
            else
            {
                //frmsearchMusic.Close();
                //isFrmSearchMusicShow = false;
            }
        }

        private void picCtlRoll_MouseEnter(object sender, EventArgs e)
        {
            picCtlRoll.Image = Properties.Resources.ctlroll_click;
        }

        private void picCtlRoll_MouseLeave(object sender, EventArgs e)
        {
            picCtlRoll.Image = Properties.Resources.ctlroll_highlight;
        }

        private void picCtlRoll_Click(object sender, EventArgs e)
        {
            if (isControlRoll == false)
            {
                isControlRoll = true;
                ControlRoll();
            }
            else
            {
                isControlRoll = false;
                timers.ForEach(a =>
                {
                    a.Stop();
                    a.Dispose();
                });
                picSoundControl.Location = new Point(237, 160);
                picNextSong.Location = new Point(141, 143);
                picPlaySong.Location = new Point(87, 132);
                picPrevSong.Location = new Point(41, 143);
                picClearList.Location = new Point(217, 3);
                picSkin.Location = new Point(187, -2);
                picSearchShow.Location = new Point(163, -1);
                picLyricsShow.Location = new Point(138, 0);
                picSettings.Location = new Point(87, -1);
                picAddMusic.Location = new Point(63, -1);
                lblTopic.Location = new Point(24, 3);
                picTopic.Location = new Point(6, 4);
                picNetwork.Location = new Point(31, 3);
                picArtist.Location = new Point(5, 5);
            }
        }

        private void picLocalList_Click(object sender, EventArgs e)
        {
            frmdesktopLyrics.Show();
            isOnFm = false;
            webBrowser1.Navigate("about:blank");
            //  webBrowser1.Stop();
            userctlMusicList.ShowDelButton = true;
            try
            {
                GetChanageList();
            }
            catch (Exception)
            {
                MessageBox.Show("请添加歌曲或搜索网络歌曲");
            }

        }

        private void GetChanageList()
        {
            userctlMusicList.Clear();
            for (j = 0; j < names.Length; j++)
            {
                var song = new Song();
                song.Title = getFileName(names[j]);
                userctlMusicList.AddItem(song);
            }
        }

        private void picBroadcast_Click(object sender, EventArgs e)
        {

            axWindowsMediaPlayer1.Ctlcontrols.stop();
            GetBroadcastList();
        }
        //private void GetBroadcastList()
        //{
        //    frmdesktopLyrics.Hide();
        //    userctlMusicList.ShowDelButton = false;
        //    isOnFm = true;
        //    userctlMusicList.Clear();
        //    string[] str = { "BBC World Service","豆瓣fm", "音乐之声", "CRI News Center","虾米电台","CRI Language Studio" };
        //    foreach(string s in str)
        //    {
        //        var song = new Song();
        //        song.Title = s;
        //        userctlMusicList.AddItem(song);
        //    }                  
        //    fmUrl(str[3]);
        //    lblMusicName.Text = str[3];
        //}

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void picLyricsShow_MouseEnter(object sender, EventArgs e)
        {
            picLyricsShow.Image = Properties.Resources.lyrics_click;
        }

        private void picLyricsShow_MouseLeave(object sender, EventArgs e)
        {
            picLyricsShow.Image = Properties.Resources.lyrics_highlisht;
        }

        private void picSearchShow_MouseEnter(object sender, EventArgs e)
        {
            picSearchShow.Image = Properties.Resources.searchwnd_click;
        }

        private void picLocalList_MouseEnter(object sender, EventArgs e)
        {
            //picLocalList.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picLocalList_MouseLeave(object sender, EventArgs e)
        {
            // picLocalList.BorderStyle = BorderStyle.None;
        }

        private void picBroadcast_MouseEnter(object sender, EventArgs e)
        {
            // picBroadcast.BorderStyle= BorderStyle.FixedSingle;
        }

        private void picBroadcast_MouseLeave(object sender, EventArgs e)
        {
            //picBroadcast.BorderStyle = BorderStyle.None;
        }

        //private void lblPrevPage_Click(object sender, EventArgs e)
        //{

        //}

        private void lblLocalList_Click(object sender, EventArgs e)
        {
            //axWindowsMediaPlayer1.settings.autoStart = true;
           
            if (PlayType.collection != PlayerType.type)
            {
                try
                {
                    lblMusicName.Text = axWindowsMediaPlayer1.currentMedia.name;
                }
                catch (Exception)
                {

                }
            }
            picArtist.Image = Properties.Resources.local_artist;
            try
            {
                frmdesktopLyrics.Show();
            }
            catch (Exception)
            {

            }
            isOnFm = false;
            webBrowser1.Navigate("about:blank");
            //  webBrowser1.Stop();
            userctlMusicList.ShowDelButton = true;
            try
            {
                GetChanageList();
            }
            catch (Exception)
            {
                MessageBox.Show("请添加歌曲或搜索网络歌曲");
            }

        }

        private void lblFmList_Click(object sender, EventArgs e)
        {
         
            picArtist.Image = Properties.Resources.local_artist;
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            GetBroadcastList();
        }
        private void GetBroadcastList()
        {
            try
            {
                frmdesktopLyrics.Hide();
            }
            catch (Exception)
            {
            }
            userctlMusicList.ShowDelButton = false;
            isOnFm = true;
            userctlMusicList.Clear();

            foreach (string s in str)
            {
                var song = new Song();
                song.Title = s;
                userctlMusicList.AddItem(song);
            }
            fmUrl(str[fmIndex]);

        }

        private void lblLocalList_MouseEnter(object sender, EventArgs e)
        {
            lblLocalList.BackColor = Color.FromArgb(209, 219, 219);
        }

        private void lblLocalList_MouseLeave(object sender, EventArgs e)
        {
            lblLocalList.BackColor = Color.Transparent;//Color.FromArgb(230, 239, 240);
        }

        private void lblFmList_MouseEnter(object sender, EventArgs e)
        {
            lblFmList.BackColor = Color.FromArgb(209, 219, 219);
        }

        private void lblFmList_MouseLeave(object sender, EventArgs e)
        {
            lblFmList.BackColor = Color.Transparent;//Color.FromArgb(230, 239, 240);
        }

        private void picSearchShow_MouseLeave(object sender, EventArgs e)
        {
            picSearchShow.Image = Properties.Resources.searchwnd_highlight;
        }

        private void picSkin1_MouseEnter(object sender, EventArgs e)
        {
            picSkin1.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picSkin1_MouseLeave(object sender, EventArgs e)
        {
            picSkin1.BorderStyle = BorderStyle.None;
        }

        private void picSkin1_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.CadetBlue;
            pnlListContainer.BackColor = Color.FromArgb(230, 239, 240);
            pnlSeparate.Visible = true;
        }

        private void picSkin2_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.back__4_;
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.Transparent;
            pnlListContainer.BackColor = Color.Transparent;
            pnlSeparate.Visible = false;
        }

        private void picSkin3_Click(object sender, EventArgs e)
        {
            //userctlMusicList.ItemHoverColor=Color.
            this.BackgroundImage = Properties.Resources.back1;
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.Transparent;
            pnlListContainer.BackColor = Color.Transparent;
            pnlSeparate.Visible = false;
        }

        private void picSkin4_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.back__2_;
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.Transparent;
            pnlListContainer.BackColor = Color.Transparent;
            pnlSeparate.Visible = false;
        }

        private void picSkin5_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.back__3_;
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.Transparent;
            pnlListContainer.BackColor = Color.Transparent;
            pnlSeparate.Visible = false;
        }

        private void picUserSkin_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(".\\Background") == false)
            {
                Directory.CreateDirectory(".\\Background");
                OpenFileDialog of1 = new OpenFileDialog();
                of1.InitialDirectory = "c:\\";
                of1.Filter = "png|*.png|jpg|*.jpg|bmp|*.bmp";
                of1.RestoreDirectory = true;
                of1.FilterIndex = 1;
                if (of1.ShowDialog() == DialogResult.OK)
                {
                    picfile = of1.FileName;
                    picName = of1.SafeFileName;
                    try
                    {
                        File.Copy(picfile, string.Format("Background\\{0}", picName, true));
                    }
                    catch (Exception)
                    {

                        //
                    }
                    this.BackgroundImage = Image.FromFile(string.Format("Background\\{0}", picName));
                }
            }
            else
            {
                OpenFileDialog of = new OpenFileDialog();
                of.InitialDirectory = "c:\\";
                of.Filter = "png|*.png|jpg|*.jpg|bmp|*.bmp";
                of.RestoreDirectory = true;
                of.FilterIndex = 1;
                if (of.ShowDialog() == DialogResult.OK)
                {
                    picfile = of.FileName;
                    picName = of.SafeFileName;
                    try
                    {
                        File.Copy(picfile, string.Format("Background\\{0}", picName, true));
                    }
                    catch (Exception)
                    {

                        //
                    }
                    this.BackgroundImage = Image.FromFile(string.Format("Background\\{0}", picName));
                }
            }
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.Transparent;
            pnlListContainer.BackColor = Color.Transparent;
            pnlSeparate.Visible = false;
        }

        private void picSkin2_MouseEnter(object sender, EventArgs e)
        {
            picSkin2.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picSkin2_MouseLeave(object sender, EventArgs e)
        {
            picSkin2.BorderStyle = BorderStyle.None;
        }

        private void picSkin3_MouseEnter(object sender, EventArgs e)
        {
            picSkin3.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picSkin3_MouseLeave(object sender, EventArgs e)
        {
            picSkin3.BorderStyle = BorderStyle.None;
        }

        private void picSkin4_MouseEnter(object sender, EventArgs e)
        {
            picSkin4.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picSkin4_MouseLeave(object sender, EventArgs e)
        {
            picSkin4.BorderStyle = BorderStyle.None;
        }

        private void picSkin5_MouseEnter(object sender, EventArgs e)
        {
            picSkin5.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picSkin5_MouseLeave(object sender, EventArgs e)
        {
            picSkin5.BorderStyle = BorderStyle.None;
        }

        private void picUserSkin_MouseEnter(object sender, EventArgs e)
        {
            picUserSkin.BorderStyle = BorderStyle.FixedSingle;
        }

        private void picUserSkin_MouseLeave(object sender, EventArgs e)
        {
            picUserSkin.BorderStyle = BorderStyle.None;
        }

        private void lblNextPage_Click(object sender, EventArgs e)
        {
           // musicPage = (names.Length + 1) / userctlMusicList.MaxSize + 1;
            lblNextPage.ForeColor = Color.Red;
            if (names.Length + 1 > musicPage * 10&&isOnFm == false)
            {
                userctlMusicList.Clear();
                for (int i = musicPage * userctlMusicList.MaxSize; i < names.Length; i++)
                {
                    var song = new Song();
                    song.Title = getFileName(names[i]);
                    userctlMusicList.AddItem(song);
                    //names[musicNum - userctlMusicList.MaxSize + 1];
                }
                musicPage++;
            }
        }

        private void lblPrevPage_Click(object sender, EventArgs e)
        {
            lblPrevPage.ForeColor = Color.Red;
            if (isOnFm == false && musicPage > 1)
            {
                userctlMusicList.Clear();
                for (int i = (musicPage - 2) * userctlMusicList.MaxSize; i < names.Length; i++)
                {
                    var song = new Song();
                    song.Title = getFileName(names[i]);
                    userctlMusicList.AddItem(song);
                    //names[musicNum - userctlMusicList.MaxSize + 1];
                }
                musicPage--;
            }
        }

        //private void lblPrevList_MouseEnter(object sender, EventArgs e)
        //{
        //    lblPrevPage.Cursor = Cursors.Hand;
        //}

        //private void lblPrevList_MouseLeave(object sender, EventArgs e)
        //{
        //    lblPrevPage.Cursor = Cursors.Default;
        //}

        private void lblNextPage_MouseEnter(object sender, EventArgs e)
        {
            lblNextPage.Cursor = Cursors.Hand;
        }

        private void lblNextPage_MouseLeave(object sender, EventArgs e)
        {
            lblNextPage.ForeColor = Color.Black;
            lblNextPage.Cursor = Cursors.Default;
        }

        private void lblPrevPage_MouseEnter(object sender, EventArgs e)
        {
            lblPrevPage.Cursor = Cursors.Hand;
        }

        private void lblPrevPage_MouseLeave(object sender, EventArgs e)
        {
            lblPrevPage.Cursor = Cursors.Default;
            lblPrevPage.ForeColor = Color.Black;
        }

        private void picLyricsLock_Click(object sender, EventArgs e)
        {
            if (this.frmdesktopLyrics != null)
            {
                this.frmdesktopLyrics.IsLockPanel = false;
            }
        }

        private void picTopic_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.night;
            pnlSkinCollection.Visible = false;
            isSkinShow = false;
            userctlMusicList.BackColor = Color.Transparent;
            pnlButtom.BackColor = Color.Transparent;
            pnlListContainer.BackColor = Color.Transparent;
            pnlSeparate.Visible = false;
        }
    }

}
