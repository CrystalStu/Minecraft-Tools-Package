using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace ModernInstallerMinecraft

{
    class Web
    {
        public static bool CheckFile(string fileUrl)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            try
            {
                re = (HttpWebRequest)WebRequest.Create(fileUrl);
                res = (HttpWebResponse)re.GetResponse();
                if (res.ContentLength != 0)
                {
                    //MessageBox.Show("文件存在");
                    return true;
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("无此文件");
                return false;
            }
            finally
            {
                if (re != null)
                {
                    re.Abort();//销毁关闭连接
                }
                if (res != null)
                {
                    res.Close();//销毁关闭响应
                }
            }
            return false;
        }

        public static string DownloadText(string url)
        {
            WebClient client = new WebClient();
            byte[] buffer = client.DownloadData(url);
            return Encoding.ASCII.GetString(buffer);
        }
    }
}
