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
    public class GetDatas
    {
        List<DataDetailDictionaryDTO> detailList = new List<DataDetailDictionaryDTO>();
        List<EntityInfoDTO> entityList = new List<EntityInfoDTO>();
        string suffix = ".cs";
        string type = " ";
        string entity = "";
        public GetDatas(string entityPath, string projectCode,string urls) {
            GetDetailFilesList(entityPath, projectCode, urls);           
        }

        /// <summary>
        /// 截取.cs文件的关键字
        /// </summary>
        /// <param name="entityPath"></param>
        /// <param name="projectCode"></param>
        public void GetDetailFilesList(string entityPath, string projectCode, string urls)
        {
            List<string> FileNames = new List<string>();
            string[] folderName = entityPath.Split('\\');
            DirectoryInfo inputfolder = new DirectoryInfo(entityPath);
            #region 获取文件夹中的.cs文件
            GetDetailList(inputfolder, projectCode, entityPath, folderName[folderName.Length-1]);
            #endregion
            #region 获取下一级所有文件夹及其旗下的.cs文件
            var list = inputfolder.GetDirectories();
            var inputFileList = list.Where(a => a.Name != "Properties").ToArray();
            if (inputFileList.Count() > 0)
            {

                //获取表头类型
                for (int i = 0; i < inputFileList.Count(); i++)
                {
                    //文件夹名称
                    type = inputFileList[i].ToString();
                    FileNames.Add(type);
                    DirectoryInfo entityfolder = new DirectoryInfo(entityPath + "\\" + type);
                    GetDetailList(entityfolder, projectCode, entityPath, type);
                }


            }
            #endregion
            #region 生成Excel文件
            //生成存储文件夹
            string url = new CreateFolder().DefaultPath(urls);
            var fileName = (url + @"\" + projectCode + "_" + folderName[folderName.Length - 1] + ".xls");
            var fileNames = (url + @"\" + projectCode + "_" + folderName[folderName.Length - 1] + "_实体信息.xls");
            new ExcelTool(detailList, fileName);
            new GetEntityExcel(entityList, fileNames);
            #endregion
        }
        /// <summary>
        /// 截取CS文件所需的内容
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="projectCode">项目代码</param>
        /// <param name="entityPath">路径</param>
        /// <param name="name">文件名称</param>
        public void GetDetailList(DirectoryInfo directoryInfo,string projectCode, string entityPath,string name) {
            var entityFileList = directoryInfo.GetFiles("*" + suffix);
            if (entityFileList.Count() > 0)
            {
                foreach (FileInfo files in entityFileList)
                {
                    EntityInfoDTO entityInfo = new EntityInfoDTO();
                    entityInfo.ProjectCode = projectCode.Trim();
                    entityInfo.FileNames = name.Trim();
                    //获取实体名
                    entity = files.ToString();
                    entity = entity.Replace(suffix, "");
                    //判断文件是否存在
                    if (File.Exists(files.FullName))
                    {
                        var summary = "";
                        var entityField = "";
                        var types = "";
                        var tableField = "";
                        try
                        {
                            #region 截取操作
                            using (StreamReader sr = File.OpenText(files.FullName))
                            {
                                string str = File.ReadAllText(files.FullName);
                                string[] strArray = str.Split(new char[1] { ':', });
                                var entityArray = strArray[0].Replace("\r\n", "");
                                string[] entityStrArray = Regex.Split(entityArray, "class", RegexOptions.IgnoreCase);
                                string[] entitySummary = Regex.Split(entityStrArray[0], "///", RegexOptions.IgnoreCase);
                                entityInfo.Summary = entitySummary[2] == null ? "命名不规范" : entitySummary[2].Trim();
                                entityField = entityStrArray[1];
                                entityInfo.EntityName = entityField.Trim();
                                entityList.Add(entityInfo);
                                var last = strArray[1].Replace("\r\n", "");
                                string[] strArray1 = last.Split(new char[1] { '{' });
                                for (int x = 0; x < strArray1.Length; x++)
                                {
                                    var st = strArray1[x].Replace("\r\n", "");
                                    if (strArray1[x].Contains("///"))
                                    {
                                        string[] strArray2 = Regex.Split(st, "///", RegexOptions.IgnoreCase);
                                        if (!strArray2[0].Contains("Entity"))
                                        {
                                            summary = strArray2[2];
                                            var st2 = strArray2[3].Replace("\r\n", "");
                                            string[] strArray3 = Regex.Split(st2, "public", RegexOptions.IgnoreCase);
                                            var st3 = strArray3[1].Replace("\r\n", "");
                                            if (strArray3[1].Contains(" ?"))
                                            {
                                                st3 = st3.Replace(" ?", "?");
                                            }
                                            else if (strArray3[1].Contains("  "))
                                            {
                                                st3 = st3.Replace("  ", " ");
                                            }
                                            string[] strArray4 = Regex.Split(st3, " ", RegexOptions.IgnoreCase);

                                            types = strArray4[1];
                                            tableField = strArray4[2];
                                            var detailDto = new DataDetailDictionaryDTO();
                                            detailDto.EntityPath = entityPath;
                                            detailDto.ProjectCode = projectCode;
                                            detailDto.EntityName = (entity.Length == 0) ? detailDto.EntityName = "命名不规范" : detailDto.EntityName = entity;
                                            detailDto.TableField = (tableField.Length == 0) ? detailDto.TableField = "命名不规范" : detailDto.TableField = tableField;
                                            detailDto.Type = (types.Length == 0) ? detailDto.Type = "命名不规范" : detailDto.Type = types;
                                            detailDto.Summary = (summary.Length == 0) ? detailDto.Summary = "命名不规范" : detailDto.Summary = summary;
                                            detailList.Add(detailDto);

                                        }
                                    }
                                    else if (st.Contains("public"))
                                    {

                                        string[] strArray3 = Regex.Split(st, "public", RegexOptions.IgnoreCase);
                                        var st3 = strArray3[1].Replace("\r\n", "");
                                        if (strArray3[1].Contains(" ?"))
                                        {
                                            st3 = st3.Replace(" ?", "?");
                                        }
                                        else if (strArray3[1].Contains("  "))
                                        {
                                            st3 = st3.Replace("  ", " ");
                                        }
                                        string[] strArray4 = Regex.Split(st3, " ", RegexOptions.IgnoreCase);
                                        types = strArray4[1];
                                        tableField = strArray4[2];
                                        var detailDto = new DataDetailDictionaryDTO();
                                        detailDto.EntityPath = entityPath;
                                        detailDto.ProjectCode = projectCode;
                                        detailDto.EntityName = (entity.Length == 0) ? detailDto.EntityName = "命名不规范" : detailDto.EntityName = entity;
                                        detailDto.TableField = (tableField.Length == 0) ? detailDto.TableField = "命名不规范" : detailDto.TableField = tableField;
                                        detailDto.Type = (types.Length == 0) ? detailDto.Type = "命名不规范" : detailDto.Type = types;
                                        detailDto.Summary = (summary.Length == 0) ? detailDto.Summary = "命名不规范" : detailDto.Summary = summary;
                                        detailList.Add(detailDto);
                                    }
                                }

                            }
                            #endregion

                        }
                        catch { }
                    }
                }
            }
        }
    }
}
