using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
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

        public void Parse221Xml()
        {
            string input = null;
            foreach(string path in _paths)
            {
                XDocument xml = XDocument.Load(path);
                var pallet_items = from xm in xml.Element("documents").Elements("hierarchy_info").Elements("sscc_down").
                                Elements("sscc_info").Elements("childs").Elements("sscc_info").Elements("childs").Elements("sgtin_info")
                                       //where xm.Element("status").Value == "released_foreign"
                                   select new PalletInfo
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
                                     select new CaseInfo
                                     {
                                         upper_pallet_sscc = xm.Parent.Parent.Parent.Parent.Element("sscc_up").Element("sscc_info").Element("sscc").Value,
                                         case_sscc = xm.Element("sscc").Value,
                                         sgtin = xm.Element("sgtin").Value,
                                         lot = xm.Element("series_number").Value

                                     };
                    if(case_items.Select(p => p.upper_pallet_sscc).FirstOrDefault() == case_items.Select(s => s.case_sscc).FirstOrDefault())
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
            Console.ReadKey();
        }

        static void PrintPalletItems(IEnumerable<PalletInfo> collection, string input)
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

        static void PrintCaseItems(IEnumerable<CaseInfo> collection, string input)
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

                
        
    }
}
