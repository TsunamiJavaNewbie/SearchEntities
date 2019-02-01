using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using SearchEntitiesFormsApp.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEntitiesFormsApp.Tool
{
    public class ExcelTool
    {
        public ExcelTool(List<DataDetailDictionaryDTO> list, string filePath) {
            SaveDetailToExcelFile(list,filePath);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public void SaveDetailToExcelFile(List<DataDetailDictionaryDTO> list, string filePath)
        {

            try
            {
                
                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                HSSFSheet sheet = tfExcel.CreateSheet(hssfworkbook, "数据字典");
                sheet.CreateFreezePane(0, 1); //冻结列头行
                tfExcel.SetColumnWidth(sheet, new List<int>() { 15, 50, 40, 25, 55 });
                HSSFCellStyle style = tfExcel.GetStyle1(hssfworkbook);
                tfExcel.CreateRow(sheet, 0, style, new List<string>() {
                    "项目代码","实体名字","实体字段","字段类型","注释"
                });
                int rowIndex = 1;
                var groupList = list.GroupBy(a => a.EntityName).ToList();
                groupList.ForEach(a =>
                {
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + a.Count() - 1, 1, 1));
                    a.ToList().ForEach(b =>
                    {
                        tfExcel.CreateRow(sheet, rowIndex, style,
                       new List<string>()
                       {
                           b.ProjectCode,b.EntityName,b.TableField,b.Type,b.Summary
                       });
                        rowIndex++;
                    });
                });
                using (FileStream file = new FileStream(filePath, FileMode.Create))
                {
                    hssfworkbook.Write(file);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public TfExcel tfExcel { get { return new TfExcel(); } }
    }
}
