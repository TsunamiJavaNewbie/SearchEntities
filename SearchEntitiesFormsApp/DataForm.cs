using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SearchEntitiesFormsApp.Dto;
using SearchEntitiesFormsApp.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEntitiesFormsApp
{
    public partial class DataForm : Form
    {
        List<DataDetailDictionaryDTO> detailList = new List<DataDetailDictionaryDTO>();
        LogDatasDto logDto = new LogDatasDto();
        Dictionary<string, LogDatasDto> remeberUrls = new Dictionary<string, LogDatasDto>();
        public string urls = @"C:\DDExcel";
        public string EntityStr = "";
        public string ProjectStr = "";
        public List<string> urlLists;
        public string path = "";
        public string listName = "";
        public string name = "";

        
        public void UpdateUrl() {
            EntityStr = path == null ? textBoxEntityUrl.Text : path;
            ProjectStr = name == null ? cbbProjectName.Text : name;
            getText(EntityStr, ProjectStr);
            if (EntityStr.Count() > 0)
            {
                urlLists = getListName(EntityStr, listName);
            }
        }

        public DataForm()
        {
            InitializeComponent();
            //居中启动
            this.StartPosition = FormStartPosition.CenterScreen;
            EntityStr = textBoxEntityUrl.Text;
            ProjectStr = cbbProjectName.Text;
            DataForm_Load(null, null);
        }

        /// <summary>
        /// 生成excel文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTablesDetail_Click(object sender, EventArgs e)
        {
            //获取窗体的内容
            
            if (string.IsNullOrEmpty(EntityStr))
            {
                MessageBox.Show("请输入文件路径");
                return;
            }
            if (string.IsNullOrEmpty(ProjectStr))
            {
                MessageBox.Show("请输入项目名称");
                return;
            }

            if (urlLists!=null) {
                getDatas(urlLists, ProjectStr);
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="entityStr"></param>
        /// <param name="projectStr"></param>
        public void getDatas(List<string> list,string projectStr)
        {
            //记录text框的数据
            foreach (string entityStrs in list)
            {
                //获取数据
                new GetDatas(entityStrs, projectStr, urls);
            }
            DialogResult dr = MessageBox.Show("文件生成成功!路径:" + urls, "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //自动打开Excel文件
                string url = new CreateFolder().DefaultPath(urls);
                System.Diagnostics.Process.Start(url);
            }
        }

        /// <summary>
        /// 排除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileChoose_Click(object sender, EventArgs e)
        {
            #region 记录文件路径

            LogDatasDto remeberUrl = new LogDatasDto();
            try {
                // 登录时 如果没有Data.bin文件就创建、有就打开
                FileStream fs = new FileStream("dataUrl.bin", FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                //保存密码选中状态
                remeberUrl.ProjectName = cbbProjectName.Text.Trim();
                if (cbRemeber.Checked)
                    remeberUrl.Path = textBoxEntityUrl.Text.Trim();
                else
                    remeberUrl.Path = "";
                if (remeberUrls.ContainsKey(remeberUrl.ProjectName))
                {
                    //如果有清掉
                    remeberUrls.Remove(remeberUrl.ProjectName);
                }
                //添加用户信息到集合
                remeberUrls.Add(remeberUrl.ProjectName, remeberUrl);
                //写入文件
                bf.Serialize(fs, remeberUrls);
                //关闭
                fs.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
          
            #endregion

            ProjectStr = cbbProjectName.Text;
            EntityStr = textBoxEntityUrl.Text;
            if (remeberUrl.Path != EntityStr) {
                EntityStr = textBoxEntityUrl.Text;
            }
            var fileChoose = new ChooseFlieList(EntityStr,ProjectStr,this);
            fileChoose.ShowDialog(this);
        }

        /// <summary>
        /// 获取历史记录的数据
        /// </summary>
        private void getText(string entityUrl, string projectName)
        {
            textBoxEntityUrl.Text = entityUrl;
            cbbProjectName.Text = projectName;
            tbUrl.Text = urls;
        }

        /// <summary>
        /// 获取文件夹list名称
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        private List<string> getListName(string entityName,string listName) {
            //截取字符串的‘，’和去除空值操作
            string[] name = listName.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> urlList = new List<string>();
            showCheckedList.Items.Clear();
            foreach (string names in name)
            {
                //显示文件夹名字
                showCheckedList.Items.Add(names,true);
                urlList.Add(entityName + @"\" + names);
            }
            return urlList;
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            try {
                //  读取配置文件寻找记录
                FileStream fs = new FileStream("dataUrl.bin", FileMode.OpenOrCreate);
                if (fs.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    remeberUrls = bf.Deserialize(fs) as Dictionary<string, LogDatasDto>;
                    foreach (LogDatasDto remeberUrl in remeberUrls.Values)
                    {
                        this.cbbProjectName.Items.Add(remeberUrl.ProjectName);
                    }

                    for (int i = 0; i < remeberUrls.Count; i++)
                    {
                        if (this.cbbProjectName.Text != "")
                        {
                            if (remeberUrls.ContainsKey(this.cbbProjectName.Text))
                            {
                                this.textBoxEntityUrl.Text = remeberUrls[this.cbbProjectName.Text].Path;
                                this.cbRemeber.Checked = true;
                            }
                        }
                    }
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
            //  用户名默认选中第一个
            if (this.cbbProjectName.Items.Count > 0)
            {
                this.cbbProjectName.SelectedIndex = this.cbbProjectName.Items.Count - 1;
            }
            getText(this.textBoxEntityUrl.Text, this.cbbProjectName.Text);
        }

        private void cbbProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                //  首先读取记录的配置文件
                FileStream fs = new FileStream("dataUrl.bin", FileMode.OpenOrCreate);

                if (fs.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    remeberUrls = bf.Deserialize(fs) as Dictionary<string, LogDatasDto>;

                    for (int i = 0; i < remeberUrls.Count; i++)
                    {
                        if (this.cbbProjectName.Text != "")
                        {
                            if (remeberUrls.ContainsKey(cbbProjectName.Text) && remeberUrls[cbbProjectName.Text].Path != "")
                            {
                                this.textBoxEntityUrl.Text = remeberUrls[cbbProjectName.Text].Path;
                                this.cbRemeber.Checked = true;
                            }
                            else
                            {
                                this.textBoxEntityUrl.Text = "";
                                this.cbRemeber.Checked = false;
                            }
                        }
                    }
                }

                fs.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void btnUrl_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.tbUrl.Text = path.SelectedPath;
            urls = path.SelectedPath;
        }
    }
}
