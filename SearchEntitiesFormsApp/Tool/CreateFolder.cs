using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEntitiesFormsApp.Tool
{
    public class CreateFolder
    {

        /// <summary>
        /// 判断存储的路径是否存在，不存在则在创建默认路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string DefaultPath(string url)
        {
            if (!Directory.Exists(url))
            {
                //创建默认路径
                Directory.CreateDirectory(url);
                //var file = File.Create(@url);
                //file.Close();
                return url;
            }
            return url;
        }

        /// <summary>
        /// 创建隐藏路径
        /// </summary>
        /// <param name="url"></param>
        public void SetHiddenFolder(string url) {
            string hiddenUrl=DefaultPath(url);
            FileAttributes MyAttributes = File.GetAttributes(url);
            File.SetAttributes(hiddenUrl, MyAttributes | FileAttributes.Hidden);
        }
    }
}
