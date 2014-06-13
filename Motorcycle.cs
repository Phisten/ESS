using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESS
{

    public enum Weights : int
    {
        絕對重要 = 1000,
        非常重要 = 875,
        很重要 = 625,
        重要 = 500,
        普通重要 = 375,
        略微重要 = 125,
        不重要 = 0
    }
    



    partial class Motorcycle
    {

        public static double GetWeight(Weights weightName) { return ((int)weightName) / 1000d; }

        public static List<string> ScoreCalculationName = new List<string>() { "不計算", "值高較優", "值低較優" };
        /// <summary>相對得分計算式, 0為錯誤 1為值高較優(比較值/樣本值) 2為值低較優(樣本值/比較值)</summary>
        public static List<Func<double, double, double>> ScoreCalculation = new List<Func<double, double, double>>() 
            { 
                null, //無效計算式
                (curNum,sampleNum)=>curNum/sampleNum, //值高較優(比較值/樣本值): 樣本值為參與機車最大值
                (curNum,sampleNum)=>sampleNum/curNum  //值低較優(樣本值/比較值): 樣本值為參與機車最小值
            };

        public Motorcycle()
        {


        }

        /// <summary>
        /// 讀取參數計算式,排除非計算項
        /// </summary>
        /// <param name="ParamTypePath"></param>
        /// <returns></returns>
        internal static List<int> GetParamTypeList(string ParamTypePath)
        {
            DataSet dsSchema = new DataSet();
            dsSchema.ReadXml(ParamTypePath);


            List<int> ParamTypeList = new List<int>();

            //排除非計算項
            for (int i = 0; i < dsSchema.Tables[0].Rows.Count; i++)
            {
                if ((string)dsSchema.Tables[0].Rows[i][1] == "值高較優")
                {
                    ParamTypeList.Add(1);
                }
                else if ((string)dsSchema.Tables[0].Rows[i][1] == "值低較優")
                {
                    ParamTypeList.Add(2);
                }
                else
                {
                    
                }
            }
            return ParamTypeList;

        }

        /// <summary>依照設定的權重計算機車的相對分數,回傳評分結果List</summary>
        /// <param name="motorcycleParamList">所有機車的參數資訊</param>
        /// <param name="paramTypeList">各參數相對分數的計算模式索引</param>
        /// <param name="weightList">各參數的權重，未賦予時權重將平均</param>
        /// <returns>回傳所有機車的相對分數</returns>
        public static List<List<double>> MotorcycleParamScore(List<List<double>> motorcycleParamList, List<int> paramTypeList, List<Weights> weightList = null)
        {
            int mtcCount = motorcycleParamList.Count; //機車總數
            int paramCount = paramTypeList.Count; //可計算的參數總數

            List<List<double>> ScoreList = new List<List<double>>(mtcCount); //回傳值

            List<double> ParamSampleList = new List<double>(paramCount); //各機車參數的樣本值

            //確認樣本值 存於ParamSumList用於之後計算相對分數
            for (int i = 0; i < paramCount; i++)
            {
                double sampleTemp = 0;
                switch (paramTypeList[i])
                {
                    case 1:
                        for (int j = 0; j < mtcCount; j++)
                        {
                            if (sampleTemp < motorcycleParamList[j][i])
                            {
                                sampleTemp = motorcycleParamList[j][i];
                            }
                        }
                        break;
                    case 2:
                        sampleTemp = double.MaxValue;
                        for (int j = 0; j < mtcCount; j++)
                        {
                            if (sampleTemp > motorcycleParamList[j][i])
                            {
                                sampleTemp = motorcycleParamList[j][i];
                            }
                        }
                        break;
                    default:
                        throw new ApplicationException("未定義的相對分數計算式或不應被計算的參數(如機種名稱)");
                }
                ParamSampleList.Add(sampleTemp);
            }

            //計算相對分數
            for (int i = 0; i < mtcCount; i++)
            {
                ScoreList.Add( new List<double>(paramCount)); 
                for (int j = 0; j < paramCount; j++)
                {
                    //根據參數計算式 計算其相對分數
                    // ScoreCalculation為計算式List 透過paramTypeList[j]索引取得計算式
                    // 然後送入兩參數motorcycleParamList[i][j], ParamSampleList[j]
                    ScoreList[i].Add(
                        ScoreCalculation[paramTypeList[j]](motorcycleParamList[i][j], ParamSampleList[j]) * (weightList == null ? 1d / (double)paramCount : (int)weightList[j] / 1000d)
                    );
                }
            }


            return ScoreList;

        }
    }
}
