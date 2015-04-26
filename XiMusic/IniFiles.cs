using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace XiMusic
{
    public class IniFiles : IDisposable
    {
        public delegate void EventHandler(object sender, EventArgs e);


        public event EventHandler IniFileChanged;
        public event EventHandler Initialization;
        protected string IniFileName;

        public string FileName
        {
            get
            {
                return IniFileName;
            }
            set
            {
                if (value != IniFileName)
                {
                    IniFileName = value;
                    OnIniFileChanged(new EventArgs());
                }
            }
        }
        protected void OnIniFileChanged(EventArgs e)
        {
            if (IniFileChanged != null)
                IniFileChanged(null, e);
        }
        protected void OnInitialization(EventArgs e)
        {
            if (Initialization != null)
                Initialization(null, e);
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        /*
        section: 要写入的段落名
        key: 要写入的键，如果该key存在则覆盖写入
        val: key所对应的值
        filePath: INI文件的完整路径和文件名
        */

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, System.Text.StringBuilder retVal, int size, string filePath);
        /*
        section：要读取的段落名
        key: 要读取的键
        defVal: 读取异常的情况下的缺省值
        retVal: key所对应的值，如果该key不存在则返回空值
        size: 值允许的大小
        filePath: INI文件的完整路径和文件名

        */

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="INIPath">文件路径</param>
        public IniFiles(string FileName)
        {
            IniFileName = FileName;
        }
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.IniFileName);
        }
        /// <summary>
        /// 读出INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        public string ReadValue(string Section, string Key, string Default)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, Default, temp, 500, this.IniFileName);
            return temp.ToString();
        }
        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        /// <returns>布尔值</returns>
        public bool ExistINIFile()
        {
            return File.Exists(IniFileName);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">路径</param>
        private void NewDirectory(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// 添加一行注释
        /// </summary>
        /// <param name="Notes">注释</param>
        public void AddNotes(string Notes)
        {
            string filename = IniFileName;
            string path;
            path = Directory.GetParent(filename).ToString();
            NewDirectory(path);
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(@";" + Notes);
            sw.Flush();
            sw.Close();
            fs.Close();
            sw.Dispose();
            fs.Dispose();
        }
        /// <summary>
        /// 添加一行文本
        /// </summary>
        /// <param name="Text">文本</param>
        public void AddText(string Text)
        {
            string filename = IniFileName;
            string path;
            path = Directory.GetParent(filename).ToString();
            NewDirectory(path);
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(Text);
            sw.Flush();
            sw.Close();
            fs.Close();
            sw.Dispose();
            fs.Dispose();
        }

        #region 重载
        public void WriteValue(string Section, string Key, int Value)
        {
            WriteValue(Section, Key, Value.ToString());
        }
        public void WriteValue(string Section, string Key, Boolean Value)
        {
            WriteValue(Section, Key, Value.ToString());
        }
        public void WriteValue(string Section, string Key, DateTime Value)
        {
            WriteValue(Section, Key, Value.ToString());
        }
        public void WriteValue(string Section, string Key, object Value)
        {
            WriteValue(Section, Key, Value.ToString());
        }
        public int ReadValue(string Section, string Key, int Default)
        {
            return Convert.ToInt32(ReadValue(Section, Key, Default.ToString()));
        }
        public decimal ReadValue(string Section, string Key, decimal Default)
        {
            return Convert.ToDecimal(ReadValue(Section, Key, Default.ToString()));
        }

        public bool ReadValue(string Section, string Key, bool Default)
        {
            return Convert.ToBoolean(ReadValue(Section, Key, Default.ToString()));
        }


        public DateTime ReadValue(string Section, string Key, DateTime Default)
        {
            return Convert.ToDateTime(ReadValue(Section, Key, Default.ToString()));
        }


        public string ReadValue(string Section, string Key)
        {
            return ReadValue(Section, Key, "");
        }
        #endregion

        public void Dispose()
        {

        }
    }
}

