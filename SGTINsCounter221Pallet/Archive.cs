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

        //public void Split915Xml()
        //{
        //    foreach (string path in _paths)
        //    {
        //        IEnumerable<XNode> nodes = Parse915Xml(path);
        //        for (int i = 0; i < nodes.Count();)
        //        {
        //            int size = 0;
        //            IEnumerable<XNode> nodestoadd = null;
        //            while (size < 900000)
        //            {
        //                nodestoadd.Append(nodes.ElementAt(i));
        //                size += size += nodes.ElementAt(i).ToString().Length;
        //                i++;
        //            }
        //            Create915Xml(path, nodestoadd, i);
        //        }
        //XDocument document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
        //          new XElement("documents",
        //          new XAttribute("version", "1.35"),
        //          new XAttribute("original_id", "153c530e-e4f1-4730-a320-a1f314a7d5d2"),
        //          new XAttribute("session_ui", "09b3da39-f672-4127-825d-fbc7fe66e870"),
        //             new XElement("multi_pack", new XAttribute("action_id", "915"),
        //             new XElement("subject_id", "1e5bb040-46c2-4ee0-b8ca-6d68adbfeb33"),
        //             new XElement("operation_date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z"),
        //             new XElement("by_sgtin"))));
        //int n = 1;
        //foreach (XNode node in nodes)
        //{
        //    if (size < 900000)
        //    {
        //        document.Element("documents").Element("multi_pack").Element("by_sgtin").Add(node);
        //        size += node.ToString().Length;
        //    }
        //    else
        //    {
        //        FileInfo fileInfo = new FileInfo(path);
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Encoding = new UTF8Encoding(false);
        //        settings.Indent = true;
        //        using (XmlWriter writer = XmlWriter.Create(String.Concat(fileInfo.FullName, "_", n), settings))
        //        {
        //            document.Save(writer);
        //        }
        //        document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
        //          new XElement("documents",
        //          new XAttribute("version", "1.35"),
        //          new XAttribute("original_id", "153c530e-e4f1-4730-a320-a1f314a7d5d2"),
        //          new XAttribute("session_ui", "09b3da39-f672-4127-825d-fbc7fe66e870"),
        //             new XElement("multi_pack", new XAttribute("action_id", "915"),
        //             new XElement("subject_id", "1e5bb040-46c2-4ee0-b8ca-6d68adbfeb33"),
        //             new XElement("operation_date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z"),
        //             new XElement("by_sgtin"))));
        //        n++;
        //        size = 0;
        //    }

        //}

        //    }


        //}

        //static void Create915Xml(string filepath, IEnumerable<XNode> nodestoadd, int splitnumber)
        //{
        //    XDocument document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
        //              new XElement("documents",
        //              new XAttribute("version", "1.35"),
        //              new XAttribute("original_id", "153c530e-e4f1-4730-a320-a1f314a7d5d2"),
        //              new XAttribute("session_ui", "09b3da39-f672-4127-825d-fbc7fe66e870"),
        //                 new XElement("multi_pack", new XAttribute("action_id", "915"),
        //                 new XElement("subject_id", "1e5bb040-46c2-4ee0-b8ca-6d68adbfeb33"),
        //                 new XElement("operation_date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z"),
        //                 new XElement("by_sgtin"))));
        //    document.Element("documents").Element("multi_pack").Element("by_sgtin").Add(nodestoadd);
        //    FileInfo fileInfo = new FileInfo(filepath);
        //    XmlWriterSettings settings = new XmlWriterSettings();
        //    settings.Encoding = new UTF8Encoding(false);
        //    settings.Indent = true;
        //    using (XmlWriter writer = XmlWriter.Create(fileInfo.FullName.Replace(".", "_{splitnumber}."), settings))
        //    {
        //        document.Save(writer);
        //    }
        //}
        //static IEnumerable<XNode> Parse915Xml(string path)
        //{
        //    XDocument xml = XDocument.Load(path);
        //    IEnumerable<XNode> nodes = from nd in xml.Element("documents").Element("multi_pack").Element("by_sgtin").Nodes()
        //                               select nd;
        //    return nodes;
        //}
        //}
    }
}
