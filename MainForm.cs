using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ESS
{
    public partial class MainForm : ComplexForm
    {
        //static public int FormWidth = 640;
        //static public int FormHeight = 480;
        public FormWeights fWeights;
        public FormCompare fCompare;
        public FormDataEdit fDataEdit;
        public FormSchemaEdit fSchemaEdit;
        public string essDataPath = @"essData.xml";
        public string essSchemaPath = @"essSchema.xml";
        //public static string StaticEssDataPath = @"essData.xml";
        //public static string StaticEssSchemaPath = @"essSchema.xml";
        public string essDataTableName = @"Motorcycle";
        public string essSchemaTableName = "MotorcycleSchema";
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(FormWidth, FormHeight);
            this.WindowState = FormWindowState.Maximized;
            this.lastForm = this;

            //this.essData.Reset();
            //this.essData.ReadXmlSchema(@"d:\essDataOpt.xml");
            //this.essData.ReadXml(essDataPath, XmlReadMode.Auto);

            fWeights = new FormWeights(this);
            //fWeights.Show();
            fCompare = new FormCompare(this);
            //fCompare.Show();
            fDataEdit = new FormDataEdit(this);
            //fDataEdit.Show();
            fSchemaEdit = new FormSchemaEdit(this);
            //fSchemaEdit.Show();
            //XElement a = new Motorcycle().toXml();
            //a.Save(@"D:\asds.xml");

            //Application.Exit();

            //預設DATA路徑
            textBox1.Text = essDataPath;

        }

        /// <summary>
        /// 表單根據行首與資料縮放
        /// </summary>
        /// <param name="dgv"></param>
        internal void DataGridViewReSize(ref DataGridView dgv)
        {
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// 刪除列首,禁止編輯,不顯示新行
        /// </summary>
        /// <param name="dgv"></param>
        internal void DataGridViweOnlyShowData(ref DataGridView dgv)
        {
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
        }

        //機車資料編輯
        private void button3_Click(object sender, EventArgs e)
        {
            ChangeForm(fDataEdit);
            fDataEdit.Init();
        }

        //機車參數編輯
        private void button2_Click(object sender, EventArgs e)
        {
            ChangeForm(fSchemaEdit);
        }

        //模糊化權重比較
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeForm(fWeights);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.InitialDirectory = Application.StartupPath;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet dsTest;
                try
                {
                    dsTest = new DataSet();
                    dsTest.ReadXml(ofd.FileName);
                    textBox1.Text = ofd.FileName;
                }
                catch (Exception) 
                {
                    MessageBox.Show("不是機車資料檔或檔案損毀","讀取失敗",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    //throw;
                }
                finally
                {

                }
            }
            //textBox1.Text = dlResult;
        }


    }
}
