using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AddressCreator {
    class Program {
        static void Main(string[] args) {

            var commandLine = new Argument(args);

            string fileName = commandLine["f"];

            string sAll = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\uavt_63.xlsx";

            //string s1 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple.xlsx";
            //string s2 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple2.xlsx";
            //string s3 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple3.xlsx";
            //string s4 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple4.xlsx";
            //string s5 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple5.xlsx";
            //string s6 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple6.xlsx";
            //string s7 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple7.xlsx";
            //string s8 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple8.xlsx";
            //string s9 = @"C:\Users\N04155-1013\Desktop\Projects\RDLab-FieldAdress\uavt_2014\simple9.xlsx";

            ReadExcelFileDOM(sAll);

            var task = Task.Factory.StartNew(() => {
                //var sr = new FileStream(sAll,FileMode.Open,FileAccess.Read);
                //using(var workbook = new XLWorkbook(sr,XLEventTracking.Disabled)) {
                //    var workSheet = workbook.Worksheets.FirstOrDefault();

                //    var createdFileName = String.Format("launch.txt");

                //    using(var txtFile = File.Create(createdFileName)) {

                //    }

                //    var tw = new StreamWriter(createdFileName,true);

                //    for(int i = 1; i < workSheet.Rows().Count(); i++) {
                //        var sb = new StringBuilder();
                //        sb.AppendFormat("insert into ADDRESS_ITEM (city_code,county_code,county_name," +
                //                        "district_code,district_name,village_code,village_name,street_code," +
                //                        "street_name,csbm_code,csbm_name,building_code,door_number,site_name,block_name," +
                //                        "uavt_address_code,indoor_number) values (",i);
                //        foreach(var xlCell in workSheet.Row(i).Cells()) {
                //            string val = xlCell.GetValue<string>().Trim();
                //            if(String.IsNullOrEmpty(val)) {
                //                val = "-";
                //            }
                //            sb.AppendFormat("\"{0}\",",val);
                //        }
                //        if(workSheet.Row(i).Cells().Count() < 17) {
                //            sb.AppendLine("\"1\"");
                //        } else {
                //            sb.Remove(sb.Length - 1,1);
                //        }
                //        sb.AppendLine(");");
                //        tw.Write(sb.ToString());
                //    }
                //    Console.WriteLine("Task finished"); 
                //}
            });



            Console.ReadLine();

        }

        static void ReadExcelFileDOM(string filename) {
            using(SpreadsheetDocument myDoc = SpreadsheetDocument.Open(filename,true)) {
                WorkbookPart workbookPart = myDoc.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();

                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
                string text;
                while(reader.Read()) {
                    if(reader.ElementType == typeof(CellValue)) {
                        text = reader.GetText();
                    }
                }
            }
        }
    }
}
