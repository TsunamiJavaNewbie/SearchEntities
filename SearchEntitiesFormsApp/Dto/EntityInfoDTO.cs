using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEntitiesFormsApp.Dto
{

     public class EntityInfoDTO
    {

        /// <summary>
        /// 项目代码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 实体类文件夹名
        /// </summary>
        public string FileNames { get; set; }

        /// <summary>
        /// 实体名
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 实体类注释
        /// </summary>
        public string Summary { get; set; }
        
    }
}
