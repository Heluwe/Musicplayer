using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XiMusic
{
    [Serializable]
    class music
    {
        //获取播放列表
        public List<string> getList
        {
            get; set;
        }
    }

    public class Song
    {
        public string SongID { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public HtmlElement PlayHE { get; set; }
        public HtmlElement SearchHE { get; set; }
        public HtmlElement DelHE { get; set; }

    }

    public class Music
    {
        public string MusicName { get; set; }
        public string MusciURL { get; set; }
        public string MusicTime { get; set; }
        public string MusicPic { get; set; }
        /// <summary>
        /// 比特率
        /// </summary>
        public string Bitrate { get; set; }

    }
}
