using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEntitiesFormsApp.Dto
{
    public class DataHeadDictionaryDTO
    {
        /// <summary>
        /// 实体类路径
        /// </summary>
        public string EntityPath { get; set; }
        /// <summary>
        /// Mapping路径
        /// </summary>
        public string TablePath { get; set; }
        /// <summary>
        /// 项目代码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 实体名
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// 实体名称（中文）
        /// </summary>
        public string EntityNameChina { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string Type { get; set; }
    }
}
