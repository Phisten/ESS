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
    public partial class FormSchemaEdit : ComplexForm
    {
        public FormSchemaEdit(MainForm mainForm)
            : base(mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        private void FormSchemaEdit_Load(object sender, EventArgs e)
        {
            ds.ReadXml(mainForm.essSchemaPath);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = mainForm.essSchemaTableName;

            DataTable scoreCalculation = new DataTable();
            scoreCalculation.Columns.Add("scoreCalculation", Type.GetType("System.String"));
            scoreCalculation.Columns.Add("scoreCalculationName", Type.GetType("System.String"));
            scoreCalculation.Rows.Add("不計算", "不計算");
            scoreCalculation.Rows.Add("值高較優", "值高較優");
            scoreCalculation.Rows.Add("值低較優", "值低較優");

            DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
            comboboxColumn = CreateComboBoxColumn();
            comboboxColumn.DataSource = scoreCalculation;
            comboboxColumn.ValueMember = "scoreCalculation";
            comboboxColumn.DisplayMember = "scoreCalculationName";
            comboboxColumn.HeaderText = "參數計算式";

            dataGridView1.Columns.Add(comboboxColumn);
            dataGridView1.Columns.RemoveAt(1);
            mainForm.DataGridViewReSize(ref dataGridView1);

            
        }
        private DataGridViewComboBoxColumn CreateComboBoxColumn()
        {
            DataGridViewComboBoxColumn column =
                new DataGridViewComboBoxColumn();
            {
                column.DataPropertyName = "參數計算式";
                column.HeaderText = "參數計算式";
                column.MaxDropDownItems = 3;
                column.FlatStyle = FlatStyle.Flat;
            }
            return column;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //欄位更名
            DataSet dsData = new DataSet();
            dsData.ReadXml(mainForm.essDataPath);
            for (int i = 0, length = ds.Tables[0].Rows.Count - 1; i < length; i++)
            {
                dsData.Tables[0].Columns[i].ColumnName = ds.Tables[0].Rows[i][0] as string;
            }
            dsData.WriteXml(mainForm.essDataPath);

            ds.WriteXml(mainForm.essSchemaPath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeForm(mainForm);
        }
    }
}
