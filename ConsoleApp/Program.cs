using ConsoleApp.Tools;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SearchEntitiesFormsApp.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    class Program
    {
        
        
        static List<DataDetailDictionaryDTO> detailList = new List<DataDetailDictionaryDTO>();
        
        static void Main(string[] args)
        {
         
            string test2 = @"D:\TFProject.WithAuthority.Core\BaseSetting2";
            string test = @"D:\TFProject.WithAuthority.Core\BaseSetting";
            GetDetailFilesList(test, "test");
            
        }

        public static  void GetDetailFilesList(string entityPath, string projectCode)
        {
            string suffix = ".cs";
            string type = " ";
            string entity = "";

            DirectoryInfo inputfolder = new DirectoryInfo(entityPath);
            //获取所有文件夹
            var list = inputfolder.GetDirectories();
            var inputFileList = list.Where(a => a.Name != "Properties").ToArray();
            if (inputFileList.Count() > 0) {
                //获取表头类型
                for (int i = 0; i < inputFileList.Count(); i++) {
                    type = inputFileList[i].ToString();
                    //Console.WriteLine("文件夹名称：" + type);
                    DirectoryInfo entityfolder = new DirectoryInfo(entityPath + "\\" + type);
                    //获取所有的cs文件
                    var entityFileList = entityfolder.GetFiles("*" + suffix);
                    if (entityFileList.Count() > 0) {
                        foreach (FileInfo files in entityFileList) {
                            //获取实体名
                            entity = files.ToString();
                            entity = entity.Replace(suffix, "");
                            //Console.WriteLine("实体名：" + entity);
                            //判断文件是否存在
                            if (File.Exists(files.FullName)) {
                                var summary = "";
                                var entityField = "";
                                var types = "";
                                var tableField = "";
                                try {
                                    using (StreamReader sr = File.OpenText(files.FullName)) {
                                        string str = File.ReadAllText(files.FullName);
                                       // Console.WriteLine(str);
                                        string[] strArray = str.Split(new char[1] { ':', });
                                        var entityArray = strArray[0].Replace("\r\n", "");
                                        string[] entityStrArray = Regex.Split(entityArray, "class", RegexOptions.IgnoreCase);
                                        entityField = entityStrArray[1];
                                        Console.WriteLine("实体名：" + entityField);

                                        var last = strArray[1].Replace("\r\n", "");
                                        string[] strArray1 = last.Split(new char[1] { '{' });
                                        for (int x = 0; x < strArray1.Length; x++) {
                                            var st = strArray1[x].Replace("\r\n", "");
                                            Console.WriteLine(strArray1[x]);
                                            if (strArray1[x].Contains("///"))
                                            {
                                                string[] strArray2 = Regex.Split(st, "///", RegexOptions.IgnoreCase);
                                                Console.WriteLine("test:"+strArray2[2]);
                                                if (!strArray2[0].Contains("Entity"))
                                                {
                                                    //获取Summary strArray2[2]
                                                    summary = strArray2[2];
                                                    Console.WriteLine("注释名：" + summary);
                                                    //Console.WriteLine("strArray2[3]：" + strArray2[3]);
                                                    var st2 = strArray2[3].Replace("\r\n", "");
                                                    string[] strArray3 = Regex.Split(st2, "public", RegexOptions.IgnoreCase);
                                                    //Console.WriteLine("strArray3[3]：" + strArray3[1]);
                                                    var st3 = strArray3[1].Replace("\r\n", "");
                                                    if (strArray3[1].Contains(" ?"))
                                                    {
                                                        st3 = st3.Replace(" ?", "?");
                                                        Console.WriteLine("strArray4[1]：" + st3);
                                                    }
                                                    else if (strArray3[1].Contains("  "))
                                                    {
                                                        st3 = st3.Replace("  ", " ");
                                                        Console.WriteLine("strArray4[1]：" + st3);
                                                    }
                                                    string[] strArray4 = Regex.Split(st3, " ", RegexOptions.IgnoreCase);

                                                    types = strArray4[1];
                                                    //Console.WriteLine("类型：" + types);
                                                    tableField = strArray4[2];
                                                    // Console.WriteLine("字段：" + tableField);
                                                    var detailDto = new DataDetailDictionaryDTO();
                                                    detailDto.EntityPath = entityPath;
                                                    detailDto.ProjectCode = projectCode;
                                                    detailDto.EntityName = (entity == null) ? detailDto.EntityName = "命名不规范" : detailDto.EntityName = entity;
                                                    detailDto.TableField = (tableField == null) ? detailDto.TableField = "命名不规范" : detailDto.TableField = tableField;
                                                    detailDto.Type = (types == null) ? detailDto.Type = "命名不规范" : detailDto.Type = types;
                                                    Console.WriteLine("类型1：" + types);
                                                    Console.WriteLine("类型2：" + detailDto.Type);
                                                    detailDto.Summary = (summary == null) ? detailDto.Summary = "命名不规范" : detailDto.Summary = summary;
                                                    detailList.Add(detailDto);

                                                }
                                            }
                                            //else
                                            //{
                                            //    summary = "命名不规范";
                                            //    string[] strArray3 = Regex.Split(st, "public", RegexOptions.IgnoreCase);
                                            //    Console.WriteLine("strArray3[1]：" + strArray3[1]);

                                            //    var st3 = strArray3[1].Replace("\r\n", "");
                                            //    if (strArray3[1].Contains(" ?"))
                                            //    {
                                            //        st3 = st3.Replace(" ?", "?");
                                            //        //Console.WriteLine("strArray4[1]：" + st3);
                                            //    }
                                            //    else if (strArray3[1].Contains("  "))
                                            //    {
                                            //        st3 = st3.Replace("  ", " ");
                                            //        //Console.WriteLine("strArray4[1]：" + st3);
                                            //    }
                                            //    string[] strArray4 = Regex.Split(st3, " ", RegexOptions.IgnoreCase);

                                            //    types = strArray4[1];
                                            //    //Console.WriteLine("类型：" + types);
                                            //    tableField = strArray4[2];
                                            //    //Console.WriteLine("字段：" + tableField);
                                            //    var detailDto = new DataDetailDictionaryDTO();
                                            //    detailDto.EntityPath = entityPath;
                                            //    detailDto.ProjectCode = projectCode;
                                            //    detailDto.EntityName = (entity == null) ? detailDto.EntityName = "命名不规范" : detailDto.EntityName = entity;
                                            //    detailDto.TableField = (tableField == null) ? detailDto.TableField = "命名不规范" : detailDto.TableField = tableField;
                                            //    detailDto.Type = (types == null) ? detailDto.Type = "命名不规范" : detailDto.Type = types;
                                            //    detailDto.Summary = (summary == null) ? detailDto.Summary = "命名不规范" : detailDto.Summary = summary;
                                            //    detailList.Add(detailDto);
                                            //}

                                        }
                                            
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }

            SaveDetailToExcelFile(detailList, DefaultPath(@"C:\DDExcelTest") + @"\" + projectCode + "_DataDetailDictionary");
            Console.WriteLine("运行完毕");
            Console.ReadKey();
        }

        private static void SaveDetailToExcelFile(List<DataDetailDictionaryDTO> list, string filePath) {
            try
            {
                HSSFWorkbook wb = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)wb.CreateSheet("数据字典"); //创建工作表
                //sheet.CreateFreezePane(0, 1); //冻结列头行
                HSSFRow row_Title = (HSSFRow)sheet.CreateRow(0); //创建列头行
                row_Title.HeightInPoints = 19.5F; //设置列头行高
                #region 设置列宽
                sheet.SetColumnWidth(0, 50 * 256);
                sheet.SetColumnWidth(1, 24 * 256);
                sheet.SetColumnWidth(2, 37 * 256);
                sheet.SetColumnWidth(3, 22 * 256);
                sheet.SetColumnWidth(4, 20 * 256);
                sheet.SetColumnWidth(5, 20 * 256);
                #endregion

                #region 设置列头单元格样式                
                HSSFCellStyle cs_Title = (HSSFCellStyle)wb.CreateCellStyle(); //创建列头样式
                cs_Title.Alignment = HorizontalAlignment.Center; //水平居中
                cs_Title.VerticalAlignment = VerticalAlignment.Center; //垂直居中
                HSSFFont cs_Title_Font = (HSSFFont)wb.CreateFont(); //创建字体
                cs_Title_Font.IsBold = true; //字体加粗
                cs_Title_Font.FontHeightInPoints = 12; //字体大小
                cs_Title.SetFont(cs_Title_Font); //将字体绑定到样式
                #endregion

                #region 生成列头
                for (int i = 0; i < 5; i++)
                {
                    HSSFCell cell_Title = (HSSFCell)row_Title.CreateCell(i); //创建单元格
                    cell_Title.CellStyle = cs_Title; //将样式绑定到单元格
                    switch (i)
                    {
                        case 0:
                            cell_Title.SetCellValue("项目代码");
                            break;
                        case 1:
                            cell_Title.SetCellValue("实体名字");
                            break;
                        case 2:
                            cell_Title.SetCellValue("实体字段");
                            break;
                        case 3:
                            cell_Title.SetCellValue("字段类型");
                            break;
                        case 4:
                            cell_Title.SetCellValue("注释");
                            break;
                    }
                }
                #endregion

                int iCount = 0;
                if (list.Count()>0) {
                    for (int i = 0; i < list.Count; i++) {
                        #region 设置内容单元格样式
                        HSSFCellStyle cs_Content = (HSSFCellStyle)wb.CreateCellStyle(); //创建列头样式
                        cs_Content.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
                        cs_Content.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
                        #endregion
                        #region 生成内容单元格
                        HSSFRow row_Content = (HSSFRow)sheet.CreateRow(i + 1); //创建行
                        row_Content.HeightInPoints = 16;
                        for (int j = 0; j < 5; j++)
                        {
                            HSSFCell cell_Conent = (HSSFCell)row_Content.CreateCell(j); //创建单元格
                            cell_Conent.CellStyle = cs_Content;
                            switch (j)
                            {
                                case 0:
                                    cell_Conent.SetCellValue(list[i].ProjectCode);
                                    break;
                                case 1:
                                    cell_Conent.SetCellValue(list[i].EntityName);
                                    break;
                                case 2:
                                    cell_Conent.SetCellValue(list[i].TableField);
                                    break;
                                case 3:
                                    cell_Conent.SetCellValue(list[i].Type);
                                    break;
                                case 4:
                                    cell_Conent.SetCellValue(list[i].Summary);
                                    break;
                            }
                        }
                        #endregion
                    }
                }
                using (FileStream file = new FileStream(filePath, FileMode.Create))
                {
                    wb.Write(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }


       

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

    }
}
