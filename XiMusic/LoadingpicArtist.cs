using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XiMusic.Properties;

namespace XiMusic
{
    public class Loadingpic
    {
        /// <summary>
        /// 加载网络上的图片并返回
        /// </summary>
        /// <param name="urlImagePath">网络地址</param>
        /// <returns></returns>
        public static Image ReadImageFromUrl(string urlImagePath)
        {
            if (string.IsNullOrEmpty(urlImagePath))
            {
                return Resources.default_album;
            }
            try
            {
                Uri uri = new Uri(urlImagePath);
                WebRequest webRequest = WebRequest.Create(uri);
                using (Stream stream = webRequest.GetResponse().GetResponseStream())
                {
                    Image res = Image.FromStream(stream);
                    return res;
                }
            }
            catch
            {
                return Resources.local_artist;
            }
        }
    }
}
