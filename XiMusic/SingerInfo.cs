using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiMusic
{
    /// <summary>
    /// 歌手信息
    /// </summary>
    public class SingerInfo
    {
        public int status { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        public string singer { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string url { get; set; }
    }
}
