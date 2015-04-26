using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiMusic
{
    public class MusicInformation
    {
        /// <summary>
        /// 请求状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 歌曲链接
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 歌曲大小
        /// </summary>
        public long fileSize { get; set; }

        /// <summary>
        /// 歌曲时长
        /// </summary>
        public int timeLength { get; set; }

        /// <summary>
        /// 歌曲比特率
        /// </summary>
        public int bitRate { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string extName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 歌手
        /// </summary>
        public string singerHead { get; set; }

        /// <summary>
        /// hash值
        /// </summary>
        public string hash { get; set; }
    }
}
