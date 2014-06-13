using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Phisten
{
    class Phisten_
    {
        




    }


    public class XmlConvertTo
    {
        /// <summary>
        /// 将Xml字符串转换成DataTable对象
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <param name="tableIndex">Table表索引</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ConvertToDataTableByXmlStringIndex(string xmlStr, int Index)
        {
            return ConvertToDateSetByXmlString(xmlStr).Tables[Index];
        }

        /// <summary>
        /// 将Xml字符串转换成DataTable对象 第一个Table
        /// </summary>
        /// <param name="xmlStr">Xml字符串</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ConvertToDataTableByXmlString(string xmlStr)
        {
            return ConvertToDateSetByXmlString(xmlStr).Tables[0];
        }

        /// <summary>
        /// 将XML字符串转换成DATASET
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static DataSet ConvertToDateSetByXmlString(string xmlStr)
        {
            if (xmlStr.Length > 0)
            {
                StringReader StrStream = null;
                XmlTextReader Xmlrdr = null;
                try
                {
                    DataSet ds = new DataSet();
                    //读取字符串中的信息
                    StrStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    Xmlrdr = new XmlTextReader(StrStream);
                    //ds获取Xmlrdr中的数据  
                    //ds.ReadXmlSchema(Xmlrdr);
                    ds.ReadXml(Xmlrdr);
                    return ds;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    //释放资源
                    if (Xmlrdr != null)
                    {
                        Xmlrdr.Close();
                        StrStream.Close();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 将DATASET转换成XML字符串
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string ConvertToXmlStringByDataSet(DataSet ds)
        {
            return ConvertToXmlStringByDataSetIndex(ds,-1); 
        }
         //// <summary>
        /// 将DataSet对象中指定的Table转换成XML字符串 当为-1时为第一个
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableIndex">DataSet对象中的Table索引</param>
        /// <returns>XML字符串</returns>
        public static string ConvertToXmlStringByDataSetIndex(DataSet ds, int Index)
        {
            if (Index != -1)
            {
                return ConvertToXmlStringByDataTable(ds.Tables[Index]);
            }
            else
            {
                return ConvertToXmlStringByDataTable(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 将DATATABLE转换成XML字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertToXmlStringByDataTable(DataTable dt)
        {
            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    UnicodeEncoding ucode = new UnicodeEncoding();
                    string returnValue = ucode.GetString(temp).Trim();
                    return returnValue;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return "";
            }
        }


    }


}