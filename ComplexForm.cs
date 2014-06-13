using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESS
{
    public class ComplexForm : Form
    {
        public MainForm mainForm;
        public ComplexForm lastForm;
        public ComplexForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.FormClosing += ComplexForm_FormClosing;
        }
        public ComplexForm()
        {
            this.FormClosing += ComplexForm_FormClosing;
        }

        private void ComplexForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lastForm == null || lastForm == this)
                Application.Exit();
                //警告: 這會跳過存檔詢問
            else
            {
                e.Cancel = true;
                ChangeForm(lastForm);
            }
        }

        /// <summary>
        /// 視窗內的資料受改動的情況下視窗被關閉的場合,給予確認是否要存檔的機會. 此值不會自動更新.需再編集資料時手動標記
        /// 注意: 程式直接被關閉時不會觸發存檔詢問
        /// </summary>
        internal bool hasEdited = false;

        /// <summary>
        /// 切換視窗,繼承原位置與大小資訊,然後替目標視窗設定原視窗以及呼叫初始化方法Init()
        /// </summary>
        /// <param name="retFrom"></param>
        internal void ChangeForm(ComplexForm retFrom)
        {
            if (hasEdited)
            {
                DialogResult mboxReturn = MessageBox.Show("您的變更尚未儲存,確定要離開嗎?","離開此視窗",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                if (mboxReturn == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    //放棄修改
                    this.hasEdited = false;
                }
            }

            retFrom.FormBorderStyle = this.FormBorderStyle;
            retFrom.Location = this.Location;
            retFrom.Size = this.Size;
            retFrom.WindowState = this.WindowState;
            if (retFrom.lastForm == null)
            {
                retFrom.lastForm = this;
            }
            retFrom.Init();
            retFrom.Show();
            this.Visible = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ComplexForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ComplexForm";
            this.ResumeLayout(false);

        }

        internal virtual void Init()
        {


        }



    }
}
