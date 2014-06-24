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
        public string essSchemaPath = @"essData.xml";
        public string essWeightsPath = @"essWeights.xml";
        //public static string StaticEssDataPath = @"essData.xml";
        //public static string StaticEssSchemaPath = @"essSchema.xml";
        //public string essDataTableName = "Motorcycle";
        //public string essSchemaTableName = "MotorcycleSchema";
        internal static string essDataTableName = "Motorcycle";
        internal static string essSchemaTableName = "MotorcycleSchema";


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(FormWidth, FormHeight);
            this.WindowState = FormWindowState.Maximized;
            this.lastForm = this;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

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
            //fDataEdit.Init();
            ChangeForm(fDataEdit);
        }

        //機車參數編輯
        private void button2_Click(object sender, EventArgs e)
        {
            //fSchemaEdit.Init();
            ChangeForm(fSchemaEdit);
        }

        //模糊化權重比較
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeForm(fWeights);
        }


        /// <summary>
        /// 更改機車資料之路徑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            changeEssDataPath();
        }

        /// <summary>
        /// 修改機車資料之路徑,若成功回傳true,失敗則回傳FALSE
        /// </summary>
        /// <returns></returns>
        internal bool changeEssDataPath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.InitialDirectory = essDataPath;
            ofd.FileName = essDataPath;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet dsTest;
                try
                {
                    dsTest = new DataSet();
                    dsTest.ReadXml(ofd.FileName);

                    //機車資料驗證

                    //成功修改檔案
                    textBox1.Text = ofd.FileName;
                    essDataPath = ofd.FileName;
                    essSchemaPath = ofd.FileName;
                }
                catch (Exception)
                {
                    MessageBox.Show("不是機車資料檔或檔案損毀", "讀取失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw;
                    return false;
                }
                finally
                {

                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新建機車資料之路徑,若成功回傳true,失敗則回傳FALSE
        /// </summary>
        /// <returns></returns>
        internal bool NewEssDataPath()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xml";
            sfd.InitialDirectory = essDataPath;
            sfd.FileName = essDataPath;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //成功修改檔案
                    textBox1.Text = sfd.FileName;
                    essDataPath = sfd.FileName;
                    essSchemaPath = sfd.FileName;
                }
                catch (Exception)
                {
                    MessageBox.Show("檔案建立失敗", "另存新檔失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw;
                    return false;
                }
                finally
                {

                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新建評分者資料之路徑,若成功回傳true,失敗則回傳FALSE
        /// </summary>
        /// <returns></returns>
        internal bool NewEssWeightsPath()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".xml";
            sfd.InitialDirectory = essWeightsPath;
            sfd.FileName = essWeightsPath;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    //成功修改檔案
                    essWeightsPath = sfd.FileName;
                }
                catch (Exception)
                {
                    MessageBox.Show("檔案建立失敗", "另存新檔失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw;
                    return false;
                }
                finally
                {

                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改評價者資料之路徑,若成功回傳true,失敗則回傳FALSE
        /// </summary>
        /// <returns></returns>
        internal bool changeEssWeightsPath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.InitialDirectory = essWeightsPath;
            ofd.FileName = essWeightsPath;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataSet dsTest;
                try
                {
                    dsTest = new DataSet();
                    dsTest.ReadXml(ofd.FileName);

                    //TODO: 評價者資料驗證

                    //成功修改檔案
                    essWeightsPath = ofd.FileName;
                }
                catch (Exception)
                {
                    MessageBox.Show("不是機車資料檔或檔案損毀", "讀取失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw;
                    return false;
                }
                finally
                {

                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
