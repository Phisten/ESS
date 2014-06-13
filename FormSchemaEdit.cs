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

            
        }
        internal override void Init()
        {
            ds = new DataSet();
            ds.ReadXml(mainForm.essSchemaPath);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = MainForm.essSchemaTableName;

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
            mainForm.DataGridViweOnlyShowData(ref dataGridView1);
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

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.DataGridViweOnlyShowData(ref dataGridView1);

            //確認檔名
            if (mainForm.NewEssDataPath() == false)
                return;

            //儲存變更
            this.hasEdited = false;

            //目前是使用相同檔名
            if (mainForm.essSchemaPath == mainForm.essDataPath)
            {
                //欄位更名 essSchemaPath與essDataPath檔案相同時
                for (int i = 0, length = ds.Tables[MainForm.essDataTableName].Columns.Count; i < length; i++)
                {
                    string newColumnName = ds.Tables[MainForm.essSchemaTableName].Rows[i][0] as string;
                    ds.Tables[MainForm.essDataTableName].Columns[i].ColumnName = newColumnName;
                }
                ds.WriteXml(mainForm.essSchemaPath);
            }
            else
            {
                //欄位更名 essSchemaPath與essDataPath檔案相異時
                DataSet dsData = new DataSet();
                dsData.ReadXml(mainForm.essDataPath);
                for (int i = 0, length = ds.Tables[MainForm.essDataTableName].Rows.Count; i < length; i++)
                {
                    string newColumnName = ds.Tables[MainForm.essSchemaTableName].Rows[i][0] as string;
                    dsData.Tables[MainForm.essDataTableName].Columns[i].ColumnName = newColumnName;
                }
                ds.WriteXml(mainForm.essSchemaPath);
                dsData.WriteXml(mainForm.essDataPath);
            }


        }

        /// <summary>
        /// 回首頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ChangeForm(mainForm);
        }

        /// <summary>
        /// 新增參數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.hasEdited = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AllowUserToAddRows = true;


            //反應至機車資料


        }
        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTable curTb = ds.Tables[MainForm.essDataTableName];
            curTb.Columns.Add(dataGridView1.Rows[dataGridView1.RowCount-1].Cells[0].Value as string);

            for (int i = 0,length = curTb.Rows.Count; i < length; i++)
            {
                curTb.Rows[i][curTb.Columns.Count - 1] = 1;
            }

            
        }
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        /// <summary>
        /// 刪除參數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.hasEdited = true;
            List<DataGridViewRow> waitDeleteDgvRow = new List<DataGridViewRow>();
            List<DataColumn> waitDeleteDataRow = new List<DataColumn>();
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                waitDeleteDgvRow.Add(item.OwningRow);
                waitDeleteDataRow.Add(ds.Tables[MainForm.essDataTableName].Columns[item.OwningRow.Cells[0].Value as string]);
            }

            //反應至機車資料
            //foreach (DataColumn item in ds.Tables[MainForm.essDataTableName].Columns)
            //{
            //    for (int i = 0, length = waitDeleteDataRow.Count; i < length; i++)
            //    {
            //        if (item.ColumnName == waitDeleteDataRow[i].Cells[0].Value as string)
            //        {
            //            waitDeleteDgvRow.Add(item);
            //        }
            //    }
            //}


            //並移除參數
            foreach (DataGridViewRow item in waitDeleteDgvRow)
            {
                dataGridView1.Rows.Remove(item);
            }
            foreach (DataColumn item in waitDeleteDataRow)
            {
                ds.Tables[MainForm.essDataTableName].Columns.Remove(item);
            }

        }


        /// <summary>
        /// 編輯模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.hasEdited = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }


    }
}
