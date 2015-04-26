using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace XiMusic
{
    public class LyricsHelper
    {
        /// <summary>
        /// 代理登录声明构造函数
        /// </summary>
        /// <param name="needproxy"></param>
        public LyricsHelper(bool needproxy)
        {
            this.needproxy = needproxy;
        }

        /// <summary>
        /// 歌词搜索地址
        /// </summary>
        public static readonly string SearchPath = "http://ttlrcct2.qianqian.com/dll/lyricsvr.dll?sh?Artist={0}&Title={1}&Flags=0";
        /// <summary>
        /// 歌词下载地址
        /// </summary>
        public static readonly string DownloadPath = "http://ttlrcct2.qianqian.com/dll/lyricsvr.dll?dl?Id={0}&Code={1}";

        private bool needproxy = false;
        /// <summary>
        /// 是否使用代理模式
        /// </summary>
        public bool NeedProxy
        {
            get { return needproxy; }
            set { needproxy = value; }
        }

        private WebProxy proxy;
        /// <summary>
        /// 代理设置类
        /// </summary>
        public WebProxy Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        private XmlNode currentSong;
        /// <summary>
        /// 当前指定下载歌词XmlNode类
        /// </summary>
        public XmlNode CurrentSong
        {
            get { return currentSong; }
            set { currentSong = value; }
        }

        /// <summary>
        /// 获取指定Url的Http请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RequestUrl(string url)
        {
            WebRequest request = WebRequest.Create(url);
            //判断是否使用代理
            if (this.needproxy)
            {
                if (this.proxy == null)
                {
                    //如果使用代理则回调代理函数加载代理登录框
                    this.OnInitializeProxy();
                }
                request.Proxy = this.proxy;
            }

            StringBuilder sb = new StringBuilder();
            try
            {
                //获取数据
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    sb.Append(sr.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                this.OnWebException(ex);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 下载歌词文件
        /// </summary>
        /// <returns></returns>
        public bool DownloadLrc()
        {
            return DownloadLrc(this.currentSong.Attributes["title"].Value + ".lrc");
        }

        /// <summary>
        /// 下载歌词文件
        /// </summary>
        /// <returns></returns>
        public bool DownloadLrc(string lrcName)
        {
            bool flag = false;
            //获取当前指定的XmlNode歌词信息
            XmlNode node = this.currentSong;
            int lrcId = -1;
            //判断当前XmlNode是否完整
            if (node != null && node.Attributes != null && node.Attributes["id"] != null)
            {
                string id = node.Attributes["id"].Value;
                //判断当前Id是否是Int类型
                if (int.TryParse(id, out lrcId))
                {
                    string xSinger = node.Attributes["artist"].Value;
                    string xTitle = node.Attributes["title"].Value;
                    string xId = node.Attributes["id"].Value;
                    //请求Http，获取歌词信息
                    string htmlCode = RequestUrl(string.Format(DownloadPath, int.Parse(xId), HtmlHelperEncode.CreateEncode(xSinger, xTitle, int.Parse(xId))));

                    using (StreamWriter sw = new StreamWriter(string.Format("{0}\\lrc\\{1}", Application.StartupPath, lrcName), false, Encoding.UTF8))
                    {
                        sw.WriteLine(htmlCode);
                    }

                    flag = true;
                }
            }

            return flag;
        }

        public event EventHandler InitializeProxy;
        public event EventHandler SelectSong;
        public event EventHandler WebException;
        protected void OnInitializeProxy()
        {
            if (this.InitializeProxy != null)
            {
                this.InitializeProxy(this, new EventArgs());
            }
        }

        protected void OnWebException(WebException ex)
        {
            if (this.WebException != null)
            {
                this.WebException(ex, new EventArgs());
            }
            else
            {
                throw ex;
            }
        }

        protected void OnSelectSong(XmlNodeList list)
        {
            if (this.SelectSong != null)
            {
                this.SelectSong(list, new EventArgs());
            }
        }

        /// <summary>
        /// 歌词搜索
        /// </summary>
        /// <param name="singer"></param>
        /// <param name="title"></param>
        public void SearchLrc(string singer, string title)
        {
            singer = singer.ToLower().Replace(" ", "").Replace("'", "");
            title = title.ToLower().Replace(" ", "").Replace("'", "");
            string htmlCode = RequestUrl(string.Format(SearchPath,
            HtmlHelperEncode.HexString(singer, Encoding.Unicode), HtmlHelperEncode.HexString(title, Encoding.Unicode)));
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(htmlCode);
                XmlNodeList list = xml.SelectNodes("/result/lrc");

                //将歌词列表以XML的形式返回到窗体
                this.OnSelectSong(list);
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class SongBase
    {

        public SongBase(int id, string artist, string title)
        {
            this.id = id;
            this.artist = artist;
            this.title = title;
        }

        private int id;
        /// <summary>
        /// 歌曲ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string title;
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string artist;
        /// <summary>
        /// 歌手
        /// </summary>
        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

    }
}
