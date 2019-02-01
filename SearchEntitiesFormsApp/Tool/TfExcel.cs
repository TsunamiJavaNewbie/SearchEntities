using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace SearchEntitiesFormsApp.Tool
{
    public class TfExcel
    {
        /// <summary>
        /// 自定义单元格样式
        /// </summary>
        /// <returns></returns>
        public HSSFCellStyle CreateStyle(HSSFWorkbook hssfworkbook, short fontHeight = 13 * 14,
                BorderStyle borderTop = BorderStyle.None, BorderStyle borderBottom = BorderStyle.None, BorderStyle borderLeft = BorderStyle.None, BorderStyle borderRight = BorderStyle.None,
                HorizontalAlignment alignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center,
                bool wrapText = false, string fontName = "新宋体", FontBoldWeight boldweight = FontBoldWeight.Normal
                )
        {
            HSSFCellStyle style = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
            HSSFFont font = (HSSFFont)hssfworkbook.CreateFont();
            font.FontName = fontName;
            font.FontHeight = fontHeight;//设置字体大小
            font.Boldweight = (short)boldweight;
            style.SetFont(font);//样式中加载字体样式
            style.Alignment = alignment;//水平居中
            style.VerticalAlignment = verticalAlignment;//居中
            style.BorderBottom = borderBottom;//设置单元格边框
            style.BorderRight = borderRight;
            style.BorderLeft = borderLeft;
            style.BorderTop = borderTop;
            style.WrapText = wrapText;
            return style;
        }

        /// <summary>
        /// 建立一个SHEET A4纸 竖向
        /// </summary>
        /// <returns></returns>
        public HSSFSheet CreateSheet(HSSFWorkbook hssfworkbook, string name)
        {
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.CreateSheet(name);
            sheet.PrintSetup.PaperSize = 9;//A4纸
            sheet.PrintSetup.Landscape = false;//竖向
            sheet.SetMargin(MarginType.RightMargin, (double)0.2);
            sheet.SetMargin(MarginType.TopMargin, (double)0.2);
            sheet.SetMargin(MarginType.LeftMargin, (double)0.2);
            sheet.SetMargin(MarginType.BottomMargin, (double)0.2);
            return sheet;
        }

        /// <summary>
        /// 建立一个SHEET A4纸 横向
        /// </summary>
        /// <returns></returns>
        public HSSFSheet CreateSheetCross(HSSFWorkbook hssfworkbook, string name)
        {
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.CreateSheet(name);
            sheet.PrintSetup.PaperSize = 9;//A4纸
            sheet.PrintSetup.Landscape = true;//横向
            sheet.SetMargin(MarginType.RightMargin, (double)0.2);
            sheet.SetMargin(MarginType.TopMargin, (double)0.2);
            sheet.SetMargin(MarginType.LeftMargin, (double)0.2);
            sheet.SetMargin(MarginType.BottomMargin, (double)0.2);
            return sheet;
        }

        /// <summary>
        /// 设置行宽
        /// </summary>
        public void SetColumnWidth(HSSFSheet sheet, List<int> widths)
        {
            for (var i = 0; i < widths.Count(); i++)
            {
                sheet.SetColumnWidth(i, widths[i] * 256);
            }
        }

        /// <summary>
        /// 第一个样式,抬头
        /// </summary>
        /// <returns></returns>
        public HSSFCellStyle GetStyle(HSSFWorkbook hssfworkbook)
        {
            return CreateStyle(hssfworkbook, 18 * 20);
        }

        /// <summary>
        /// 样式,抬头
        /// </summary>
        /// <returns></returns>
        public HSSFCellStyle GetStyleHead(HSSFWorkbook hssfworkbook)
        {
            return CreateStyle(hssfworkbook);
        }

        /// <summary>
        /// 第二个样式,显示表头及表单
        /// </summary>
        /// <returns></returns>
        public HSSFCellStyle GetStyle1(HSSFWorkbook hssfworkbook)
        {
            return CreateStyle(hssfworkbook, 13 * 14, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin,
                HorizontalAlignment.Center, VerticalAlignment.Center, true);
        }

        /// <summary>
        /// 第三个样式,显示项名加粗
        /// </summary>
        /// <returns></returns>
        public HSSFCellStyle GetStyle2(HSSFWorkbook hssfworkbook)
        {
            return CreateStyle(hssfworkbook, 13 * 14, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin,
                HorizontalAlignment.Center, VerticalAlignment.Center, true, boldweight: FontBoldWeight.Bold);
        }
        /// <summary>
        /// 第四个样式,靠左
        /// </summary>
        /// <returns></returns>
        public HSSFCellStyle GetStyle4(HSSFWorkbook hssfworkbook)
        {
            return CreateStyle(hssfworkbook, 13 * 14, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin, BorderStyle.Thin,
                HorizontalAlignment.Left, VerticalAlignment.Center, true);
        }

        /// <summary>
        /// 创建单元格
        /// </summary>
        public void CreateCell(HSSFCell cell, HSSFRow row, int index, HSSFCellStyle style, string value)
        {
            cell = (HSSFCell)row.CreateCell(index);
            cell.CellStyle = style;
            cell.SetCellValue(string.IsNullOrEmpty(value) ? "" : value);
        }

        /// <summary>
        /// 创建标题行 合并列
        /// </summary>
        public void CreateTitleRow(HSSFSheet sheet, string title, HSSFCellStyle style, int colspan, int rowIndex = 0, int rowHeight = 0)
        {
            HSSFRow row = (HSSFRow)sheet.CreateRow(rowIndex);//建立一行，NPOI要选择立行才参建立CELL
            if (rowHeight != 0)
                row.Height = (short)(rowHeight * 20);
            HSSFCell cell = (HSSFCell)row.CreateCell(rowIndex);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, colspan - 1));//合并单完格
            cell.CellStyle = style;
            cell.SetCellValue(title);
        }

        /// <summary>
        /// 创建一行
        /// </summary>
        public void CreateRow(HSSFSheet sheet, int rowIndex, HSSFCellStyle style, List<string> value, int rowHeight = 0)
        {
            var row = (HSSFRow)sheet.CreateRow(rowIndex);
            if (rowHeight != 0)
                row.Height = (short)(rowHeight * 20);
            HSSFCell cell = (HSSFCell)row.CreateCell(0);
            for (var i = 0; i < value.Count(); i++)
            {
                CreateCell(cell, row, i, style, value[i]);
            }
        }

        /// <summary>
        /// 返回ExcelFileByte
        /// </summary>
        /// <returns></returns>
        public byte[] GetExcelFileData(HSSFWorkbook hssfworkbook)
        {
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);
            //写出报表
            byte[] bt = ms.ToArray();
            hssfworkbook = null;
            ms.Close();
            ms.Dispose();
            return bt;
        }
    }
}
