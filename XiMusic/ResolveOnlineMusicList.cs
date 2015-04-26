using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiMusic
{
    public class ResolveOnlineMusicList
    {
        public static List<Music> ResolveMusic(List<string> list)
        {
            List<Music> musicList = null;
            if (list != null)
            {
                musicList = new List<Music>();
                for (int i = 0; i < list.Count; i++)
                {
                    string[] str = list[i].Split(new string[] { "___" }, StringSplitOptions.RemoveEmptyEntries);
                    musicList.Add(new Music()
                    {
                        MusciURL = str[0],
                        MusicTime = TimeFormat(str[1]),
                        MusicName = i + 1 + "." + str[3] + "    ",
                        MusicPic = str[2]
                    });
                }
            }
            return musicList;
        }

        /// <summary>
        /// 将总秒数格式化为mm:ss
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        private static string TimeFormat(string timeString)
        {
            string timeFormat;
            int second;
            if (!int.TryParse(timeString, out second))
            {
                timeFormat = "error";
            }
            else
            {
                string minutes = (second / 60).ToString().Length == 1 ? "0" + second / 60 : (second / 60).ToString();
                string seconds = (second % 60).ToString().Length == 1 ? "0" + second % 60 : (second % 60).ToString();
                timeFormat = minutes + ":" + seconds;
            }
            return timeFormat;
        }
    }
}
