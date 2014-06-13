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
    public partial class FormCompare : ComplexForm
    {
        public FormCompare(MainForm mainForm)
            : base(mainForm)
        {
            InitializeComponent();
        }

        DataSet dsTempData, dsData;// = new DataSet();
        DataSet dsSchema;//
        private void FormCompare_Load(object sender, EventArgs e)
        {



        }
        internal override void Init()
        {
            dsTempData = new DataSet();
            dsData = new DataSet();
            dsSchema = new DataSet();
            dsTempData.ReadXml(mainForm.essDataPath);
            dsData.ReadXml(mainForm.essDataPath);
            dsSchema.ReadXml(mainForm.essSchemaPath);

            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dsTempData;
            dataGridView1.DataMember = MainForm.essDataTableName;

            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = dsData;
            dataGridView2.DataMember = MainForm.essDataTableName;


            mainForm.DataGridViewReSize(ref dataGridView1);
            mainForm.DataGridViweOnlyShowData(ref dataGridView1);
            mainForm.DataGridViewReSize(ref dataGridView2);
            mainForm.DataGridViweOnlyShowData(ref dataGridView2);

        }

        /// <summary>
        /// 權重正規化
        /// </summary>
        /// <param name="srcDgv"></param>
        internal List<Weights> CalculateWeights(DataGridView srcDgv)
        {
            //排除非計算項
            for (int i = dsTempData.Tables[MainForm.essDataTableName].Columns.Count - 1; i >= 0; i--)
            {
                string curCalType = (string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1];
                if (curCalType == "值高較優" || curCalType == "值低較優")
                {
                    dataGridView1.Columns.Insert(i, new DataGridViewTextBoxColumn());
                    dataGridView1.Columns[i].HeaderText = dataGridView1.Columns[i + 1].HeaderText;
                    dataGridView1.Columns.RemoveAt(i + 1);
                }
                else
                {
                    dsTempData.Tables[MainForm.essDataTableName].Columns.RemoveAt(i);
                }
            }

            dataGridView1.DataSource = null;


            //補上評價者名稱
            DataGridViewTextBoxColumn dgvColTemp = new DataGridViewTextBoxColumn();
            dgvColTemp.HeaderText = "評價者";
            dataGridView1.Columns.Insert(0, dgvColTemp);

            dsTempData.Clear();

            //複製評價者名稱
            for (int i = 0 ,length = srcDgv.Rows.Count - 1; i < length; i++)
            {
                dataGridView1.Rows.Insert(i, new DataGridViewRow());
                for (int j = 0, colLen = srcDgv.ColumnCount; j < colLen; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = srcDgv.Rows[i].Cells[j].Value;
                }
            }

            //輸出平均權重
            DataGridViewRow avgRow = new DataGridViewRow();
            dataGridView1.Rows.Add(avgRow);
            int dataCount = srcDgv.RowCount - 1;
            dataGridView1.Rows[dataCount].Cells[0].Value = "平均權重";
            for (int j = 1, colLen = srcDgv.ColumnCount; j < colLen; j++)
            {
                double curVal = 0d;
                for (int i = 0; i < dataCount; i++)
                {
                    curVal += (double)dataGridView1.Rows[i].Cells[j].Value;
                }
                dataGridView1.Rows[dataCount].Cells[j].Value = curVal / (double)dataCount;
            }


            //輸出正規化權重
            List<double> NormalWeights = new List<double>();

            DataGridViewRow norRow = new DataGridViewRow();
            dataGridView1.Rows.Add(norRow);
            dataCount = srcDgv.RowCount;
            int valCount = srcDgv.ColumnCount - 1;
            dataGridView1.Rows[dataCount].Cells[0].Value = "正規化權重";

            double sumVal = 0d;
            for (int j = 1, colLen = srcDgv.ColumnCount; j < colLen; j++)
            {
                sumVal += (double)dataGridView1.Rows[dataCount - 1].Cells[j].Value;
            }
            for (int j = 1, colLen = srcDgv.ColumnCount; j < colLen; j++)
            {
                double curVal = Math.Round((double)dataGridView1.Rows[dataCount - 1].Cells[j].Value / sumVal, 3);
                NormalWeights.Add(curVal);
                dataGridView1.Rows[dataCount].Cells[j].Value = curVal;
            }

            return NormalWeights.Select<double, Weights>(x => (Weights)((int)(x * 1000))).ToList<Weights>();
        }

        /// <summary>
        /// 取出可計算參數,輸出為二維double List
        /// </summary>
        /// <param name="srcDgv"></param>
        /// <returns></returns>
        internal List<List<double>> GetCalculableParam(DataGridView srcDgv)
        {
            List<List<double>> CalculableParamList = new List<List<double>>();

            dsTempData = new DataSet();
            dsTempData.ReadXml(mainForm.essDataPath);
            DataTable dt = dsTempData.Tables[MainForm.essDataTableName];

            //排除非計算項
            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if ((string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1] == "值高較優")
                {

                }
                else if ((string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1] == "值低較優")
                {
                }
                else
                {
                    dt.Columns.RemoveAt(i);
                }
            }

            //output List
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CalculableParamList.Add(dt.Rows[i].ItemArray.ToList<object>().Select<object, double>(x => double.Parse((string)x)).ToList<double>());
            }

            return CalculableParamList;
        }

        internal void SetCalculableParam(DataGridView srcDgv, List<List<double>> Param)
        {
            DataSet dsSchema = new DataSet();
            dsSchema.ReadXml(mainForm.essSchemaPath);

            //判斷該參數索引是否可計算
            Func<int, bool> Calculable = i => ((string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1] == "值高較優" || (string)dsSchema.Tables[MainForm.essSchemaTableName].Rows[i][1] == "值低較優");

            DataSet curDs = srcDgv.DataSource as DataSet;
            DataTable curTb = curDs.Tables[MainForm.essDataTableName];
            int paramIndex = 0;

            //各列機車參數的分數總合
            List<double> ScoreSumList = new List<double>(new double[curTb.Columns.Count]);

            for (int i = 0; i < curTb.Columns.Count; i++)
            {
                if (Calculable(i))
                {
                    for (int j = 0; j < curTb.Rows.Count; j++)
                    {
                        double curScore = Math.Round(Param[j][paramIndex],3);
                        ScoreSumList[j] += curScore;
                        curTb.Rows[j][i] = curScore;
                    }
                    paramIndex++;
                }
            }

            //追加機車評分總分欄位
            DataColumn dsDc = new DataColumn();
            dsDc.ColumnName = "機車評分總分";
            curTb.Columns.Add(dsDc);
            for (int i = 0; i < srcDgv.Rows.Count; i++)
            {
                if (ScoreSumList[i] != -1d)
                {
                    curTb.Rows[i][srcDgv.ColumnCount - 1] = ScoreSumList[i];
                }
            }

            srcDgv.Sort(srcDgv.Columns[srcDgv.ColumnCount-1], ListSortDirection.Descending);
        }


        internal void CalculateScoreAndShow(List<Weights> weightsList, ref DataGridView tarDgv)
        {
            dsSchema = new DataSet();
            dsSchema.ReadXml(mainForm.essSchemaPath);
            List<List<double>> Param = GetCalculableParam(tarDgv);
            List<int> ParamTypeList = Motorcycle.GetParamTypeList(mainForm.essSchemaPath);

            Param = Motorcycle.MotorcycleParamScore(Param, ParamTypeList, weightsList);

            SetCalculableParam(tarDgv, Param);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeForm(mainForm);
        }


    }
}
