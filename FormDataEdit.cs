using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ESS
{
    public partial class FormDataEdit : ComplexForm
    {
        public FormDataEdit(MainForm mainForm)
            : base(mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormDataEdit_Load(object sender, EventArgs e)
        {
            
        }


        DataSet ds1;
        internal override void Init()
        {
            //load dgv1
            button2_Click(null, null);

            button2.Visible = false;


            //load dgv2
            DataSet ds2 = new DataSet();
            ds2.ReadXml(mainForm.essDataPath);

            dataGridView2.DataSource = ds2;
            dataGridView2.DataMember = "Motorcycle";
            mainForm.DataGridViewReSize(ref dataGridView2);
            mainForm.DataGridViweOnlyShowData(ref dataGridView2);
            mainForm.fCompare.CalculateScoreAndShow(null, ref dataGridView2);
        }

        private void essDataBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        //Load dgv1
        private void button2_Click(object sender, EventArgs e)
        {

        //    mainForm.essData.Reset();
        //    mainForm.essData.ReadXmlSchema(@"d:\essDataOpt.xml");
        //    mainForm.essData.ReadXml(@"d:\essDataOpt.xml", XmlReadMode.Auto);

            ds1 = new DataSet();
            ds1.ReadXml(mainForm.essDataPath);

            dataGridView1.DataSource = ds1;
            dataGridView1.DataMember = "Motorcycle";

            mainForm.DataGridViewReSize(ref dataGridView1);
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (mainForm.NewEssDataPath())
            {
                hasEdited = false;
                ds1.WriteXml(mainForm.essDataPath);
                this.Init();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeForm(mainForm);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            hasEdited = true;
        }

    }

}
