﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
namespace WriteDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            TestPPT.testfunc();
            //WriteToWordDoc(@"e:/test/WriteDoc.docx", "刘泾铭");
        }

        /// <summary>
        ///追加一个加粗的文字 ，
        ///需要按照openxml的xml约定追加内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="txt"></param>
        public static void WriteToWordDoc(string filePath, string txt)
        {
            using (WordprocessingDocument wordProcessingDocument = WordprocessingDocument.Open(filePath, true))
            {
                Body body = wordProcessingDocument.MainDocumentPart.Document.Body;

                //Add new text
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());

                //Apply bold formatting to the run
                RunProperties runProperties = run.AppendChild(new RunProperties(new Bold()));
                run.AppendChild(new Text(txt));

               //Paragraph para2 = body.AppendChild(new Paragraph());
               //Run run2 = para.AppendChild(new Run());
               ////apply formatting to the run
               //RunProperties runProperties2 = run2.AppendChild(new RunProperties(new Bold()));
            }
        }
    }
}
