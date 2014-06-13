using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ESS
{


    partial class Motorcycle
    {

        /// <summary>品牌 / 製造廠商</summary>
        public string Brand { get; set; }
        /// <summary>機種名稱 / 產品名稱</summary>
        public string Name { get; set; }
        /// <summary>產品型號</summary>
        //public string Model  { get; set; }
        /// <summary>標準售價</summary>
        public int Price { get; set; }
        /// <summary>最高速度</summary>
        public int MaxSpeed { get; set; }
        /// <summary>加速時間(由0至最高速度所需秒數)</summary>
        public int AccelerationTime { get; set; }
        /// <summary>實際排氣量</summary>
        public int Displacement{ get; set; }
        /// <summary>排氣量類別</summary>
        public eDisplacementClass DisplacementClass { get; set; }

        //品牌	機種名稱	價格	乾燥重量	置物箱	尾速	油箱容量	馬力	扭力	耗油率	據點	特點



        /// <summary>排氣量分類</summary>
        public enum eDisplacementClass
        {
            Class125cc = 125, Class150cc = 150
        }

        public Motorcycle(XElement srcXElement)
        {
            Name = srcXElement.Element(eXmlTitle.名稱.ToString()).ToString();
            Brand = srcXElement.Element(eXmlTitle.廠牌.ToString()).ToString();
            //Model = srcXElement.Element(eXmlTitle.型號.ToString()).ToString();
            Price  = XElementToInt(srcXElement.Element(eXmlTitle.售價.ToString()));
            MaxSpeed = XElementToInt(srcXElement.Element(eXmlTitle.極速.ToString()));
            AccelerationTime  = XElementToInt(srcXElement.Element(eXmlTitle.加速時間.ToString()));
            Displacement   = XElementToInt(srcXElement.Element(eXmlTitle.實際排氣量.ToString()));
            DisplacementClass = (eDisplacementClass)XElementToInt(srcXElement.Element(eXmlTitle.排氣量分類.ToString()));
        }


        public XElement toXml()
        {
            XElement xmlMotorcycle =
                new XElement("機車",
                    new XElement(eXmlTitle.名稱.ToString(), Name),
                    new XElement(eXmlTitle.廠牌.ToString(), Brand),
                    //new XElement(eXmlTitle.型號.ToString(), Model),
                    new XElement(eXmlTitle.售價.ToString(), Price),
                    new XElement(eXmlTitle.極速.ToString(), MaxSpeed),
                    new XElement(eXmlTitle.加速時間.ToString(), AccelerationTime),
                    new XElement(eXmlTitle.實際排氣量.ToString(), Displacement),
                    new XElement(eXmlTitle.排氣量分類.ToString(), DisplacementClass)
                );
            //System.Windows.Forms.MessageBox.Show(xmlMotorcycle.ToString());
            return xmlMotorcycle;
        }

        

        private float XElementToFloat(XElement inputXElement)
        {
            float outFloat = 0f;
            outFloat = inputXElement == null ? 0 : float.Parse(inputXElement.ToString());
            return outFloat;
        }
        private int XElementToInt(XElement inputXElement)
        {
            int outInt = 0;
            outInt = inputXElement == null ? 0 : int.Parse(inputXElement.ToString());
            return outInt;
        }

        enum eXmlTitle
        {
            名稱, 廠牌, 型號, 售價, 極速, 加速時間, 實際排氣量, 排氣量分類
        }

        enum eBrand
        {
            宏佳騰=0,偉士牌=1,光陽=2,比雅久=3,三陽=4,山葉=5
        }
    }


}
