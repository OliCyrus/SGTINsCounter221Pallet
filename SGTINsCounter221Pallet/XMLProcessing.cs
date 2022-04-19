using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SGTINsCounter221Pallet
{
    internal class XMLProcessing
    {
        string[] _paths { get; set; }    

        public XMLProcessing(string[] filepaths)
        {
            _paths = filepaths;
        }

        public XMLProcessing()
        {
            string path;
            Console.Write(@"Введите путь к директории в виде X:\DirectoryName: ");
            path = Console.ReadLine();
            try
            {
                _paths = Directory.GetFiles(path, "*xml");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void Parse221Xml()
        {
            try
            {
                string input = null;
                if (_paths.Length == 0)
                {
                    throw new IOException("В директории отсутствую файлы подходящего типа.");
                }
                foreach (string path in _paths)
                {
                    XDocument xml = XDocument.Load(path);
                    var pallet_items = from xm in xml.Element("documents").Elements("hierarchy_info").Elements("sscc_down").
                                    Elements("sscc_info").Elements("childs").Elements("sscc_info").Elements("childs").Elements("sgtin_info")
                                           //where xm.Element("status").Value == "released_foreign"
                                       select new PalletInfo221
                                       {
                                           pallet_sscc = xm.Parent.Parent.Parent.Parent.Element("sscc").Value,
                                           case_sscc = xm.Element("sscc").Value,
                                           sgtin = xm.Element("sgtin").Value,
                                           lot = xm.Element("series_number").Value

                                       };
                    if (pallet_items.FirstOrDefault() != null)
                    {
                        Console.WriteLine("Палета:{0} Серия:{1} Количество коробов:{2} Количество упаковок:{3}", pallet_items.Select(x => x.pallet_sscc).FirstOrDefault(),
                          pallet_items.Select(x => x.lot).Distinct().ToArray().Aggregate((x, y) => x + ", " + y), pallet_items.Select(x => x.case_sscc).Distinct().Count(), pallet_items.Count());
                        Console.Write("Вывести содержимое палета?(y/n)");
                        input = Console.ReadLine();
                        PrintPalletItems(pallet_items, input);

                    }
                    else
                    {
                        var case_items = from xm in xml.Element("documents").Elements("hierarchy_info").Elements("sscc_down").
                                   Elements("sscc_info").Elements("childs").Elements("sgtin_info")
                                         select new CaseInfo221
                                         {
                                             upper_pallet_sscc = xm.Parent.Parent.Parent.Parent.Element("sscc_up").Element("sscc_info").Element("sscc").Value,
                                             case_sscc = xm.Element("sscc").Value,
                                             sgtin = xm.Element("sgtin").Value,
                                             lot = xm.Element("series_number").Value

                                         };
                        if (case_items.Select(p => p.upper_pallet_sscc).FirstOrDefault() == case_items.Select(s => s.case_sscc).FirstOrDefault())
                        {
                            Console.WriteLine("Короб:{0} Вложен в палету:{1} Серия:{2} Количество упаковок:{3}", case_items.Select(x => x.case_sscc).FirstOrDefault(),
                          "-", case_items.Select(x => x.lot).Distinct().ToArray().Aggregate((x, y) => x + ", " + y), case_items.Count());
                        }
                        else
                        {
                            Console.WriteLine("Короб:{0} Вложен в палету:{1} Серия:{2} Количество упаковок:{3}", case_items.Select(x => x.case_sscc).FirstOrDefault(),
                          case_items.Select(x => x.upper_pallet_sscc).FirstOrDefault(), case_items.Select(x => x.lot).Distinct().ToArray().Aggregate((x, y) => x + ", " + y), case_items.Count());

                        }

                        Console.Write("Вывести содержимое короба?(y/n)");
                        input = Console.ReadLine();
                        PrintCaseItems(case_items, input);



                    }


                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine (ex.Message);
            }
            
        }

        public void Split915Xml()
        {
            try
            {
                if (_paths.Length == 0)
                {
                    throw new IOException("В директории отсутствую файлы подходящего типа.");
                }
                foreach (string path in _paths)
                {
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Length > 1024000)
                    {
                        IEnumerable<XNode> nodes = Parse915Xml(path);
                        IEnumerator<XNode> enumerator = nodes.GetEnumerator();
                        int i = 1;
                        while (enumerator.MoveNext())
                        {
                            List<XNode> nodestoadd = new List<XNode>();
                            int size = 0;
                            while (size < 900000)
                            {
                                if (enumerator.Current == null)
                                    break;
                                nodestoadd.Add(enumerator.Current);
                                size += enumerator.Current.ToString().Length;
                                if (size > 900000)
                                    break;
                                enumerator.MoveNext();
                            }
                            Create915Xml(path, nodestoadd, i);
                            i++;
                        }
                    }
                    else
                        continue;
                    
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
            
            
        }



        #region Static Methods

        static void Create915Xml(string filepath, List<XNode> nodestoadd, int splitnumber)
        {
            XDocument document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                      new XElement("documents",
                      new XAttribute("version", "1.35"),
                      new XAttribute("original_id", "153c530e-e4f1-4730-a320-a1f314a7d5d2"),
                      new XAttribute("session_ui", "09b3da39-f672-4127-825d-fbc7fe66e870"),
                         new XElement("multi_pack", new XAttribute("action_id", "915"),
                         new XElement("subject_id", "00000000202356"),
                         new XElement("operation_date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z"),
                         new XElement("by_sgtin"))));
            document.Element("documents").Element("multi_pack").Element("by_sgtin").Add(nodestoadd);
            FileInfo fileInfo = new FileInfo(filepath);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(fileInfo.FullName.Replace(".", $"_{splitnumber}."), settings))
            {
                document.Save(writer);
            }
        }

        static IEnumerable<XNode> Parse915Xml(string path)
        {
            XDocument xml = XDocument.Load(path);
            IEnumerable<XNode> nodes = from nd in xml.Element("documents").Element("multi_pack").Element("by_sgtin").Nodes()
                                       select nd;
            return nodes;
        }

        static void PrintPalletItems(IEnumerable<PalletInfo221> collection, string input)
        {
            while (input != null)
            {
                if (input == "y" || input == "Y")
                {
                    foreach (var str in collection)
                    {
                        Console.WriteLine(str.sgtin);
                    }
                    var distinctSSCC = collection.Select(x => x.case_sscc).Distinct().ToArray();
                    foreach (var sscc in distinctSSCC)
                    {
                        Console.WriteLine(sscc);
                    }
                    break;
                }
                else if (input == "n" || input == "N")
                {
                    break;
                }
                else
                {

                    Console.WriteLine("Неправильный ввод");
                    Console.Write("Вывести содержимое палета?(y/n)");
                    input = Console.ReadLine();
                    continue;
                }
            }
        }

        static void PrintCaseItems(IEnumerable<CaseInfo221> collection, string input)
        {
            while (input != null)
            {
                if (input == "y" || input == "Y")
                {
                    foreach (var str in collection)
                    {
                        Console.WriteLine(str.sgtin);
                    }
                    break;
                }
                else if (input == "n" || input == "N")
                {
                    break;
                }
                else
                {

                    Console.WriteLine("Неправильный ввод");
                    Console.Write("Вывести содержимое короба?(y/n)");
                    input = Console.ReadLine();
                    continue;
                }
            }
        }

        #endregion


    }
}
