using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SearchEntitiesFormsApp.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        static bool flag = false;
        static string url = @"D:\DDExcel\log";
        static string path = @"D:\MesAbpProject\TYMesAbpProject";
        static string txtName = @"Log_Datas.txt";
        static string txtName1 = @"Log_Datas.txt";
        static string txtName2 = @"entity";
        static void Main(string[] args)
        {
            //CreateFile(txtName1, txtName2);
            //if (File.Exists(url + @"\" + txtName))
            //{

            //    LogDatasDto dto= Read(url + @"\" + txtName);
            //    Console.WriteLine(dto.Path + "----" + dto.ProjectName);
            //    Console.ReadKey();

            //}

            GetFile(path);
        }

        public static void GetFile(string path)
        {
            DirectoryInfo inputfolder = new DirectoryInfo(path);
            //获取所有文件夹
            var list = inputfolder.GetDirectories();
            foreach (DirectoryInfo d in list) {
                Console.WriteLine(d);
                Console.ReadKey();
            }

        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="logDatasDto"></param>
        /// <returns></returns>
        public static bool CreateFile(string txtName1, string txtName2)
        {
            if (!string.IsNullOrEmpty(txtName1) && !string.IsNullOrEmpty(txtName2))
            {
                //创建隐藏文件夹
                SetHiddenFolder(url);
                //创建txt文件并且写入数据
                Write(url, txtName1, txtName2);
                flag = true;
            }
            return flag;
        }

        public static void Write(string path, string txtName1, string txtName2)
        {
            FileStream fs = (!File.Exists(path + @"\" + txtName)) ?
                new FileStream(path + @"\" + txtName, FileMode.Create, FileAccess.Write) :
                new FileStream(path + @"\" + txtName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(txtName1 + ":" + txtName2);//开始写入值
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 判断存储的路径是否存在，不存在则在创建默认路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DefaultPath(string url)
        {
            if (!Directory.Exists(url))
            {
                //创建默认路径
                Directory.CreateDirectory(url);
                return url;
            }
            return url;
        }

        /// <summary>
        /// 创建隐藏路径
        /// </summary>
        /// <param name="url"></param>
        public static void SetHiddenFolder(string url)
        {
            string hiddenUrl = DefaultPath(url);
            FileAttributes MyAttributes = File.GetAttributes(url);
            File.SetAttributes(hiddenUrl, MyAttributes | FileAttributes.Hidden);
        }

        public static LogDatasDto Read(string path)
        {
            LogDatasDto dto=new LogDatasDto();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
           if((line = sr.ReadLine()) != null)
            {
                string[] array= Regex.Split(line, ":", RegexOptions.IgnoreCase);
                dto.Path = array[0];
                dto.ProjectName = array[1];
            }
            return dto;
        }
    }

    

    public class LogDatasDto
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

    }

}
