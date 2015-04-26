using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiMusic
{
    public class MusicOnline
    {
        /// <summary>
        /// 请求状态：1成功  0失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// 返回详细信息
        /// </summary>
        public MusicData data { get; set; }
    }
}
