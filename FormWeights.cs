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
    public partial class FormWeights : ComplexForm
    {
        public FormWeights(MainForm mainForm)
            : base(mainForm)
        {
            InitializeComponent();
        }

        DataSet dsSchema = new DataSet();
        DataSet dsData = new DataSet();
        DataSet dsWeights = new DataSet();


        internal override void Init()
        {
            dsSchema = new DataSet();
            dsData = new DataSet();
            dsSchema.ReadXml(mainForm.essSchemaPath);
            dsData.ReadXmlSchema(mainForm.essDataPath);

            DataTable dt = new DataTable();
            dt = dsData.Tables[MainForm.essDataTableName];
            dataGridView1.Columns.Clear();

            DataTable scoreCalculation = new DataTable();
            scoreCalculation.Columns.Add("weightsName", Type.GetType("System.String"));
            scoreCalculation.Columns.Add("weightsValue", Type.GetType("System.Double"));
            scoreCalculation.Rows.Add("絕對重要", 1d);
            scoreCalculation.Rows.Add("非常重要", 0.875d);
            scoreCalculation.Rows.Add("很重要", 0.625d);
            scoreCalculation.Rows.Add("重要", 0.5d);
            scoreCalculation.Rows.Add("普通重要", 0.375d);
            scoreCalculation.Rows.Add("略微重要", 0.125d);
            scoreCalculation.Rows.Add("不重要", 0d);

            DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
            string headerName = "aa";
            comboboxColumn = CreateComboBoxColumn(headerName);
            comboboxColumn.DataSource = scoreCalculation;
            comboboxColumn.DisplayMember = "weightsName";
            comboboxColumn.ValueMember = "weightsValue";
            comboboxColumn.HeaderText = headerName;

            //dataGridView1.DataSource = dsData;

            dataGridView1.DataSource = dsData;
            dataGridView1.DataMember = MainForm.essDataTableName;
            

            //排除非計算項
            for (int i = dsData.Tables[MainForm.essDataTableName].Columns.Count - 1; i >= 1; i--)
            {
                if ((string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1] == "值高較優")
                {
                    DataGridViewComboBoxColumn curComboCol = comboboxColumn.Clone() as DataGridViewComboBoxColumn;
                    //curComboCol.HeaderText = dataGridView1.Columns[i].HeaderText;
                    curComboCol.HeaderText = dsData.Tables[MainForm.essDataTableName].Columns[i].ColumnName;
                    dataGridView1.Columns.Insert(i, curComboCol);
                    dataGridView1.Columns.RemoveAt(i + 1);
                    
                }
                else if ((string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1] == "值低較優")
                {
                    DataGridViewComboBoxColumn curComboCol = comboboxColumn.Clone() as DataGridViewComboBoxColumn;
                    //curComboCol.HeaderText = dataGridView1.Columns[i].HeaderText;
                    curComboCol.HeaderText = dsData.Tables[MainForm.essDataTableName].Columns[i].ColumnName;
                    dataGridView1.Columns.Insert(i, curComboCol);
                    dataGridView1.Columns.RemoveAt(i + 1);
                }
                else
                {
                    dsData.Tables[MainForm.essDataTableName].Columns.RemoveAt(i);
                }
            }


            dataGridView1.DataSource = null;

            if (dataGridView1.Columns.Count > 0 && dataGridView1.Columns[0].HeaderText != "評價者")
            {
                //補上評價者名稱
                DataGridViewTextBoxColumn dgvColTemp = new DataGridViewTextBoxColumn();
                dgvColTemp.HeaderText = "評價者";
                dataGridView1.Columns.Insert(0, dgvColTemp);
            }

            //新增預設資料
            string[] dataName = new string[] { "戴子芸", "程威誠", "林哲瑋", "林玠霈", "陳雅婷" };
            double[][] dataVal = new double[][] {
                new double[]{ 0.875d, 0.875d, 0.625d, 0.375d, 0.375d, 0.375d, 0.375d, 0.5d, 0.125d, 0.5d },
                new double[]{ 0.875d, 0.375d, 0.5d, 0.5d, 0.875d, 0.5d, 0.375d, 0.875d, 0.125d, 0.5d },
                new double[]{ 0.875d, 0.375d, 0.625d, 0.625d, 0.375d, 0.625d, 0.5d, 0.5d, 0.375d, 0.375d },
                new double[]{ 1d, 0.5d, 0.875d, 0.375d, 0.375d, 0.125d, 0.125d, 0.875d, 0d, 0d },
                new double[] { 1d, 0.375d, 0.875d, 0.375d, 0d, 0.5d, 0.375d, 0.875d, 0d, 0.5d}};
            int DefDataCount = 5;

            for (int j = 0, length = DefDataCount; j < length; j++)
            {
                dt.Rows.Add((object)dataName[j]);
                for (int i = 1, Columnslength = dt.Columns.Count; i < Columnslength; i++)
                {
                    if (dataVal[j].Length + 1 > i)
                        dt.Rows[j][i] = dataVal[j][i - 1];
                    else
                        dt.Rows[j][i] = 0.375d;
                }

                dataGridView1.Rows.Insert(j, new DataGridViewRow());
                dataGridView1.Rows[j].Cells[0].Value = dataName[j];
                for (int i = 1, Columnslength = dataGridView1.Columns.Count; i < Columnslength; i++)
                {
                    if (dataVal[j].Length + 1 > i)
                        dataGridView1.Rows[j].Cells[i].Value = dataVal[j][i - 1];
                    else
                        dataGridView1.Rows[j].Cells[i].Value = 0.375d;
                }
            }

            //新建DATASET
            //dt.Clear();
            dt.Columns[0].ColumnName = "評價者";
            



            mainForm.DataGridViewReSize(ref dataGridView1);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter; //快速編輯
        }

        private void FormWeights_Load(object sender, EventArgs e)
        {

        }

        private DataGridViewComboBoxColumn CreateComboBoxColumn(string HeaderText)
        {
            DataGridViewComboBoxColumn column =
                new DataGridViewComboBoxColumn();
            {
                column.DataPropertyName = HeaderText;
                column.HeaderText = HeaderText;
                column.MaxDropDownItems = 7;
                column.FlatStyle = FlatStyle.Flat;
            }
            return column;
        }

        /// <summary>
        /// 送出評價者清單,顯示機車評價
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeForm(mainForm.fCompare);
            List<Weights> weightsList = mainForm.fCompare.CalculateWeights(dataGridView1);
            mainForm.fCompare.CalculateScoreAndShow(weightsList, ref mainForm.fCompare.dataGridView2);
        }


        /// <summary>
        /// 刪除目前選擇的橫列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //this.hasEdited = true;
            List<DataGridViewRow> waitDeleteDgvRow = new List<DataGridViewRow>();
            List<DataColumn> waitDeleteDataRow = new List<DataColumn>();
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                waitDeleteDgvRow.Add(item.OwningRow);
                //waitDeleteDataRow.Add(dsData.Tables[MainForm.essDataTableName].Columns[item.OwningRow.Cells[0].Value as string]);
            }

            //移除參數
            foreach (DataGridViewRow item in waitDeleteDgvRow)
            {
                if (item.DataGridView == dataGridView1)
                {
                    dataGridView1.Rows.Remove(item);
                }
            }
            //foreach (DataColumn item in waitDeleteDataRow)
            //{
            //if (item.Table == dsData.Tables[MainForm.essDataTableName])
            //{
            //    dsData.Tables[MainForm.essDataTableName].Columns.Remove(item);
            //}
            //}



            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            //    {
            //        dataGridView1.Rows.Remove(item);
            //    }
            //}
        }

        /// <summary>
        /// 評分者資訊存檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //確認檔名
            if (mainForm.NewEssWeightsPath() == false)
                return;

            dsData.WriteXml(mainForm.essWeightsPath);
        }




        /// <summary>
        /// 載入評分者資訊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //確認檔名
            if (mainForm.changeEssWeightsPath() == false)
                return;

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dsData = new DataSet();
            dsData.ReadXml(mainForm.essWeightsPath);

            dataGridView1.DataSource = dsData;
            dataGridView1.DataMember = MainForm.essDataTableName;


            DataTable scoreCalculation = new DataTable();
            scoreCalculation.Columns.Add("weightsName", Type.GetType("System.String"));
            scoreCalculation.Columns.Add("weightsValue", Type.GetType("System.Double"));
            scoreCalculation.Rows.Add("絕對重要", 1d);
            scoreCalculation.Rows.Add("非常重要", 0.875d);
            scoreCalculation.Rows.Add("很重要", 0.625d);
            scoreCalculation.Rows.Add("重要", 0.5d);
            scoreCalculation.Rows.Add("普通重要", 0.375d);
            scoreCalculation.Rows.Add("略微重要", 0.125d);
            scoreCalculation.Rows.Add("不重要", 0d);

            DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
            string headerName = "aa";
            comboboxColumn = CreateComboBoxColumn(headerName);
            comboboxColumn.DataSource = scoreCalculation;
            comboboxColumn.DisplayMember = "weightsName";
            comboboxColumn.ValueMember = "weightsValue";
            comboboxColumn.HeaderText = headerName;

            //若無資料則直接跳出
            if (dsData.Tables[MainForm.essDataTableName].Rows.Count == 0)
            {
                Console.Write("essWeights 檔案內資料為空!");
                return;
            }


            //修改為COMBOBOX
            for (int i = dsData.Tables[MainForm.essDataTableName].Columns.Count - 1; i >= 1; i--)
            {
                DataGridViewComboBoxColumn curComboCol = comboboxColumn.Clone() as DataGridViewComboBoxColumn;
                curComboCol.HeaderText = dsData.Tables[MainForm.essDataTableName].Columns[i].ColumnName;
                dataGridView1.Columns.Insert(i, curComboCol);
                dataGridView1.Columns[i].ValueType = Type.GetType("double");
                for (int j = 0, length = dataGridView1.Rows.Count - 1; j < length; j++)
                {
                    var readTmp = dataGridView1.Rows[j].Cells[i + 1].Value;
                    if (readTmp == null || readTmp.ToString() == "")
                    {
                        readTmp = (double)ESS.Weights.普通重要 / 1000d;
                    }
                    else
                    {
                        readTmp = double.Parse(readTmp.ToString());
                    }
                    double dTemp = (double)readTmp;
                    dataGridView1.Rows[j].Cells[i].Value = dTemp;
                }
                dataGridView1.Columns.RemoveAt(i + 1);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {

        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView s = sender as DataGridView;
            DataTable dt = dsData.Tables[MainForm.essDataTableName];

            
            dt.Rows[e.RowIndex][e.ColumnIndex] = s.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            

        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTable dt = dsData.Tables[MainForm.essDataTableName];
            dt.Rows.RemoveAt(e.Row.Index);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = dsData.Tables[MainForm.essDataTableName];
            dt.Rows.Add();
            dataGridView1.Rows.Add();
        }



    }
}
