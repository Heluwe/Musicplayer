using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
namespace XiMusic
{
    /// <summary>
    /// 搜索音乐、获取音乐信息、下载地址、歌手图片等
    /// </summary>
    public class SearchMusic
    {
        /// <summary>
        /// 获取搜索的音乐列表
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public MusicOnline GetMusicByApi(string searchCondition, int pageindex, int pagesize)
        {
            string url =
                string.Format(
                    @"http://mobilecdn.kugou.com/api/v3/search/song?format=json&keyword={0}&page={1}&pagesize={2}",
                    searchCondition, pageindex, pagesize);
            try
            {
                string str = RequestUrl(url);
                str = str.Replace("320hash", "_320hash").Replace("320filesize", "_320filesize");
                if (!string.IsNullOrEmpty(str))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var musicOnline = jss.Deserialize<MusicOnline>(str);
                    return musicOnline;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + ex.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// 获取歌曲链接（里面包含很多歌曲其他信息可供使用）
        /// </summary>
        /// <param name="musichash"></param>
        /// <returns></returns>
        public string GetMusicUrl(string musichash)
        {
            if (!string.IsNullOrEmpty(musichash))
            {
                string url = string.Format(@"http://m.kugou.com/app/i/getSongInfo.php?cmd=playInfo&hash={0}", musichash);
                string responseStr = RequestUrl(url);
                if (!string.IsNullOrEmpty(responseStr))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var musicInfo = jss.Deserialize<MusicInformation>(responseStr);
                    return musicInfo.url;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取歌手图片
        /// </summary>
        /// <param name="singerName"></param>
        /// <returns></returns>
        public string GetMusicPic(string singerName)
        {
            if (!string.IsNullOrEmpty(singerName))
            {
                string filePath = string.Format(@"{0}\images\{1}.jpg", System.Environment.CurrentDirectory, singerName);
                if (!Directory.Exists(string.Format(@"{0}\images", System.Environment.CurrentDirectory)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\images", System.Environment.CurrentDirectory));
                }
                if (File.Exists(filePath))
                {
                    return filePath;
                }
                string url = string.Format(@"http://mobilecdn.kugou.com/new/app/i/yueku.php?cmd=104&size=120&singer={0}&type=softhead", singerName);
                string responseStr = RequestUrl(url);
                if (!string.IsNullOrEmpty(responseStr))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var singer = jss.Deserialize<SingerInfo>(responseStr);
                    return DownloadPic(singerName, singer.url);
                }
            }
            return null;
        }

        /// <summary>
        /// 对指定的url地址发起get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string RequestUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "User-Agent	Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                myReq.Timeout = 30000;
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
                string str = readerOfStream.ReadToEnd();
                result.Close();
                if (receviceStream != null) receviceStream.Dispose();
                readerOfStream.Dispose();
                return str;
            }
            catch (Exception ex)
            {
                MessageBox.Show("请求地址错误，或没有联网");
            }
            return null;
        }

        private string DownloadPic(string singerName, string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 30000;
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();
            string filepath = string.Format(@"{0}\images\{1}.jpg", System.Environment.CurrentDirectory, singerName);
            FileStream writer = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[512];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, c);
            }
            writer.Close();
            writer.Dispose();
            reader.Close();
            reader.Dispose();
            response.Close();
            return filepath;
        }

        /// <summary>
        /// 通过文件地址获取Tag(歌曲歌手图片(内置))
        /// </summary>
        /// <param name="MusicPath">歌曲路径</param>
        /// <returns></returns>
        public List<Image> GetPic_Music(string MusicPath)
        {
            List<Image> imgList = new List<Image>();
            Tags.ID3.ID3Info file = new Tags.ID3.ID3Info(MusicPath, true);
            foreach (Tags.ID3.ID3v2Frames.BinaryFrames.AttachedPictureFrame item in file.ID3v2Info.AttachedPictureFrames)
            {
                System.Drawing.Image img = item.Picture; // 此段代码用于将获得的
                imgList.Add(img);
            }
            return imgList;
        }
    }

    public class PageInfo
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
    }
}

