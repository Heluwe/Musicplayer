using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace XiMusic
{
    public class MusicInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        public string singername { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long filesize { get; set; }

        /// <summary>
        /// 文件hash值
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 歌曲比特率
        /// </summary>
        public int bitrate { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string extname { get; set; }

        /// <summary>
        /// 歌曲时长
        /// </summary>
        public int duration { get; set; }

        /// <summary>
        /// mv的hash值
        /// </summary>
        public string mvhash { get; set; }

        /// <summary>
        /// m4a格式文件大小
        /// </summary>
        public long m4afilesize { get; set; }

        /// <summary>
        /// 320比特率文件hash值
        /// </summary>
        public string _320hash { get; set; }

        /// <summary>
        /// 320比特率文件大小
        /// </summary>
        public long _320filesize { get; set; }

        /// <summary>
        /// 无损音乐hash值
        /// </summary>
        public string sqhash { get; set; }

        /// <summary>
        /// 无损音乐大小
        /// </summary>
        public long sqlfilesize { get; set; }

        public int isnew { get; set; }

        /// <summary>
        /// 收费类型
        /// </summary>
        public int feetype { get; set; }

        public int ownercount { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public int srctype { get; set; }
    }
}
