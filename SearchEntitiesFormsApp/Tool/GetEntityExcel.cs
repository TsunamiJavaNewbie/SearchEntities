using NPOI.HSSF.UserModel;
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
    /// <summary>
    /// 获取实体的Excel信息
    /// </summary>
    public class GetEntityExcel
    {
        public GetEntityExcel(List<EntityInfoDTO> list, string filePath)
        {
            SaveDetailToExcelFile(list, filePath);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public void SaveDetailToExcelFile(List<EntityInfoDTO> list, string filePath)
        {

            try
            {

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                HSSFSheet sheet = tfExcel.CreateSheet(hssfworkbook, "数据字典");
                sheet.CreateFreezePane(0, 1); //冻结列头行
                tfExcel.SetColumnWidth(sheet, new List<int>() { 15, 50, 40, 40 });
                HSSFCellStyle style = tfExcel.GetStyle1(hssfworkbook);
                tfExcel.CreateRow(sheet, 0, style, new List<string>() {
                    "项目代码","文件名字","实体名称","实体注释"
                });
                int rowIndex = 1;
                var groupList = list.GroupBy(a => a.FileNames).ToList();
                groupList.ForEach(a =>
                {
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + a.Count() - 1, 1, 1));
                    a.ToList().ForEach(b =>
                    {
                       tfExcel.CreateRow(sheet, rowIndex, style,
                       new List<string>()
                       {
                           b.ProjectCode,b.FileNames,b.EntityName,b.Summary
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
