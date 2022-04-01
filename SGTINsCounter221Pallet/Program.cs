using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace SGTINsCounter221Pallet
{
    class Program
    {
        static void Main(string[] args)
        {

            XMLProcessing xMLProcessing = new XMLProcessing(Directory.GetFiles(Directory.GetCurrentDirectory()+@"\221","*.xml"));
            xMLProcessing.Parse221XML();

        }

        

        

        static void getsgtinsfrom915(string[] filepath)
        {
            List<string> ssccs = selectsscc(Directory.GetFiles(@"D:\415\", "*.xml"));
            List<string> sgtins = new List<string>();
            int counter = 0;
            for (int i = 0; i < filepath.Length; i++)
            {
                
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filepath[i]);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    foreach (XmlNode bysgtin in xnode.ChildNodes)
                    {
                        if (bysgtin.Name == "by_sgtin")
                        {
                            foreach (XmlNode detail in bysgtin)
                            {
                                if (detail.Name == "detail")
                                {
                                    foreach (XmlNode sscc in detail.ChildNodes)
                                    {
                                        foreach(string str in ssccs)
                                        {
                                            if (sscc.Name == "sscc" && sscc.InnerText == str)
                                            {
                                                foreach (XmlNode content in detail.ChildNodes)
                                                {
                                                    if (content.Name == "content")
                                                    {
                                                        foreach (XmlNode sgtin in content)
                                                        {
                                                            //Console.WriteLine(String.Concat("<sgtin>", sgtin.InnerText, "</sgtin>"));
                                                            sgtins.Add(String.Concat("<sgtin>", sgtin.InnerText, "</sgtin>"));
                                                            counter++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        

                                    }
                                }
                            }
                        }

                    }
                }
            }
            File.WriteAllLines(@"D:\415\output.txt", sgtins);
            Console.WriteLine(counter);
        }

        static void getsgtinsfrom9151(string[] filepath)
        {
            List<string> ssccs = selectsscc(Directory.GetFiles(@"D:\415\", "*.xml"));
            List<string> sgtins = new List<string>();
            int counter = 0;
            for (int i = 0; i < filepath.Length; i++)
            {

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filepath[i]);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    foreach (XmlNode bysgtin in xnode.ChildNodes)
                    {
                        if (bysgtin.Name == "by_sgtin")
                        {
                            foreach (XmlNode detail in bysgtin)
                            {
                                if (detail.Name == "detail")
                                {
                                    foreach (XmlNode sscc in detail.ChildNodes)
                                    {
                                        foreach (string str in ssccs)
                                        {
                                            if (sscc.Name == "sscc" && sscc.InnerText == str)
                                            {
                                                sgtins.Add("<detail>");
                                                sgtins.Add(string.Concat("<sscc>",sscc.InnerText,"</sscc>"));
                                                foreach (XmlNode content in detail.ChildNodes)
                                                {
                                                    if (content.Name == "content")
                                                    {
                                                        sgtins.Add("<content>");
                                                        foreach (XmlNode sgtin in content)
                                                        {
                                                            //Console.WriteLine(String.Concat("<sgtin>", sgtin.InnerText, "</sgtin>"));
                                                            sgtins.Add(String.Concat("<sgtin>", sgtin.InnerText, "</sgtin>"));
                                                            counter++;
                                                        }
                                                    }
                                                }
                                                sgtins.Add("</content>");
                                                sgtins.Add("</detail>");
                                            }
                                        }


                                    }
                                }
                            }
                        }

                    }
                }
            }
            File.WriteAllLines(@"D:\415\output.txt", sgtins);
            Console.WriteLine(counter);
        }

        static List<string> selectsscc(string[] filepath)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < filepath.Length; i++)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filepath[i]);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    foreach (XmlNode orderdetails in xnode.ChildNodes)
                    {
                        if(orderdetails.Name == "order_details")
                        {
                            foreach(XmlNode union in orderdetails)
                            {
                                if(union.Name == "union")
                                {
                                    foreach(XmlNode ssccdetail in union)
                                    {
                                        if(ssccdetail.Name == "sscc_detail")
                                        {
                                            Console.WriteLine(string.Concat("<sscc>",ssccdetail.ChildNodes[0].InnerText,"</sscc>"));
                                            output.Add(ssccdetail.ChildNodes[0].InnerText);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            return output;
        }

        static void selecterroritems(string[] filepath)
        {
            int count = 0;
            for (int i = 0; i < filepath.Length; i++)
            {

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filepath[i]);
                // получим корневой элемент
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    //Console.WriteLine(xnode.Name.ToString());
                    foreach (XmlNode errors in xnode.ChildNodes)
                    {
                        if(errors.Name == "errors")
                        {
                            Console.WriteLine(String.Concat("<sscc>",errors.ChildNodes[2].InnerText,"</sscc>"));
                            count++;
                        }
                    }
                }
             }
            Console.WriteLine(count);
        }


    }
}
