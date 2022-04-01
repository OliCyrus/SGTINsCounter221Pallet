using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SGTINsCounter221Pallet
{
    class Archive
    {
        public static void palletandcasecounter(string[] filepath)
        {
            try
            {

                for (int i = 0; i < filepath.Length; i++)
                {
                    int counter = 0;
                    string pallet = "";
                    string lot = "";

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(filepath[i]);
                    // получим корневой элемент
                    XmlElement xRoot = xDoc.DocumentElement;
                    // обход всех узлов в корневом элементе
                    Console.WriteLine("\nПалета, Серия, Количество:");
                    foreach (XmlNode xnode in xRoot)
                    {
                        //Console.WriteLine(xnode.Name.ToString());
                        foreach (XmlNode sscc_down in xnode.ChildNodes)
                        {
                            // если узел - company
                            if (sscc_down.Name == "sscc_down")
                            {
                                //Console.WriteLine($"sscc: {childnode.InnerText}");
                                // Console.WriteLine($"sscc: {childnode.ChildNodes.}");
                                foreach (XmlNode ssccinfo_pallet in sscc_down)
                                {
                                    if (ssccinfo_pallet.Name == "sscc_info")
                                    {
                                        pallet = ssccinfo_pallet.ChildNodes[0].InnerText;
                                        foreach (XmlNode childs in ssccinfo_pallet)
                                        {
                                            if (childs.Name == "childs")
                                            {
                                                foreach (XmlNode ssccinfo_case in childs)
                                                {
                                                    if (ssccinfo_case.Name == "sscc_info")
                                                    {
                                                        //Console.WriteLine("Case SSCC:{0}", ssccinfo_case.ChildNodes[0].InnerText);
                                                        foreach (XmlNode sscc_childs in ssccinfo_case)
                                                        {
                                                            if (sscc_childs.Name == "childs")
                                                            {
                                                                foreach (XmlNode sgtin_info in sscc_childs)
                                                                {
                                                                    if (sgtin_info.Name == "sgtin_info")
                                                                    {
                                                                        //Console.WriteLine(sgtin_info.ChildNodes[0].InnerText);
                                                                        counter++;
                                                                        if (lot == "")
                                                                        {
                                                                            lot = sgtin_info.ChildNodes[4].InnerText;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (ssccinfo_case.Name == "sgtin_info")
                                                    {
                                                        Console.WriteLine(ssccinfo_case.ChildNodes[0].InnerText);      //uncomment for case sgtins output
                                                        counter++;
                                                        if (lot == "")
                                                        {
                                                            lot = ssccinfo_case.ChildNodes[4].InnerText;
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
                    if (counter != 0)
                    {
                        Console.WriteLine(pallet + " " + lot + " " + counter.ToString());
                    }

                }
            }
            catch
            {

            }
        }
    }
}
