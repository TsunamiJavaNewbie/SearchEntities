using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEntitiesFormsApp.Dto
{
    [Serializable]
     public class LogDatasDto
    {
        private string path;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get { return path; } set { path = value; } }
        private string projectName;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get { return projectName; } set { projectName = value; } }

    }
}
