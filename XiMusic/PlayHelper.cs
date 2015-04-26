using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiMusic
{
    public class PlayHelper
    {
        /// <summary>
        /// 播放状态：-1，停止、0，暂停、1，播放
        /// </summary>
        public static int PlayStatu { get; set; }

        /// <summary>
        /// 当前播放歌曲编号
        /// </summary>
        public static int MusicId { get; set; }

        /// <summary>
        /// 快进快退标志位
        /// </summary>
        public static int PlayFlag { get; set; }

        /// <summary>
        /// 音乐标题是否需要滚动
        /// </summary>
        public static bool MusicTitleRolling { get; set; }

        /// <summary>
        /// 当前播放的音乐所在panel
        /// </summary>
        public static int PanelTag { get; set; }
    }
}
