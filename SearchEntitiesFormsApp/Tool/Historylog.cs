using SearchEntitiesFormsApp.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchEntitiesFormsApp.Tool
{
    public class Historylog
    {
        bool flag = false;
        string url = @"D:\DDExcel\log";
        string txtName = @"Log_Datas.txt";
        /// <summary>
        /// 创建历史纪录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public LogDatasDto Checked(string path,string name) {
            LogDatasDto dto = new LogDatasDto();
            dto.Path = path;
            dto.ProjectName = name;
            
            if (File.Exists(url + @"\" + txtName))
            {
                dto = Read(url + @"\" + txtName);
            }
            else {
                CreateFile(dto);
            }
            return dto;
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="logDatasDto"></param>
        /// <returns></returns>
        public bool CreateFile(LogDatasDto logDatasDto) {
            if (!string.IsNullOrEmpty(logDatasDto.Path)&&!string.IsNullOrEmpty(logDatasDto.ProjectName)) {
                //创建隐藏文件夹
                new CreateFolder().SetHiddenFolder(url);
                //创建txt文件并且写入数据
                Write(url, logDatasDto);
                flag = true;
            }
            return flag;
        }
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="logDatasDto"></param>
        public void Write(string path,LogDatasDto logDatasDto)
        {
            FileStream fs = new FileStream(path + @"\" + txtName, FileMode.Create, FileAccess.Write,FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.WriteLine(logDatasDto.Path + "&" + logDatasDto.ProjectName);//开始写入值
            //sw.Flush();
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LogDatasDto Read(string path)
        {
            LogDatasDto dto = new LogDatasDto();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            if ((line = sr.ReadLine()) != null)
            {
                string[] array = Regex.Split(line, "&", RegexOptions.IgnoreCase);
                dto.Path = array[0];
                dto.ProjectName = array[1];
            }
            return dto;
        }
    }
}
