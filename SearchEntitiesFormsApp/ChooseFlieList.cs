using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchEntitiesFormsApp
{
    public partial class ChooseFlieList : Form
    {
        string path,name;
        DataForm form;
        public ChooseFlieList(string _path,string _name, DataForm forms)
        {
            path = _path;
            name = _name;
            form = forms;
            InitializeComponent();
            //居中启动
            this.StartPosition = FormStartPosition.CenterScreen;
            GetFiles(path);
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 获取并显示所有文件夹名字的
        /// </summary>
        /// <param name="path"></param>
        public void GetFiles(string path)
        {
            DirectoryInfo inputfolder = new DirectoryInfo(path);

            //获取所有文件夹
            var list = inputfolder.GetDirectories();
            foreach (DirectoryInfo names in list)
            {
                //显示文件夹名字
                checkedListBox.Items.Add(names);
            }

        }

        private void buttonComfirm_Click(object sender, EventArgs e)
        {
            string getName=null;
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (!checkedListBox.GetItemChecked(i))//先判断是否被选中
                {
                    getName += checkedListBox.GetItemText(checkedListBox.Items[i])+",";
                }
            }
            form.path = path;
            form.listName = getName;
            form.name = name;
            form.UpdateUrl();
            this.Close();           
        }
        /// <summary>
        /// 全选控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allCheckBox.Checked)
            {
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    checkedListBox.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    checkedListBox.SetItemChecked(i, false);
                }
            }
        }

    }
}
