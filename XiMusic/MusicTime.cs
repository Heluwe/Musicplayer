using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell32;

namespace XiMusic
{
    public class MusicTimes
    {
        /// <summary>
        /// 获取指定的音乐的时间
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static string GetMusicTime(string fileUrl)
        {
            ShellClass sh = new ShellClass();
            Folder dir = sh.NameSpace(Path.GetDirectoryName(fileUrl));
            FolderItem item = dir.ParseName(Path.GetFileName(fileUrl));
            string str = dir.GetDetailsOf(item, 27);
            return str;
        }

        /// <summary>
        /// 获取比特率
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static string GetMusicBit(string fileUrl)
        {
            ShellClass sh = new ShellClass();
            Folder dir = sh.NameSpace(Path.GetDirectoryName(fileUrl));
            FolderItem item = dir.ParseName(Path.GetFileName(fileUrl));
            string str = dir.GetDetailsOf(item, 28);
            if (string.IsNullOrEmpty(str))
                str = "0kbps";
            return str;
        }

        /// <summary>
        /// 时间转换，将秒转换为对应的   mm：ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ConvertTime(int time)
        {
            string musicTime = "00:00";
            if (time > 0)
            {
                int minute = time / 60;
                int seconds = time % 60;
                musicTime = (minute < 10 ? "0" + minute.ToString() : minute.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
            }
            return musicTime;
        }
    }
}

