using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace CreateMinWordDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = "e:/test/test3.docx";
            // CreateWordDoc(filepath, "Hello World");
           readWordContent(filepath);
        }

        /// <summary>
        /// 创建一个word文档，插入文本
        /// 最终的结果
        //     <?xml version="1.0" encoding="utf-8"?>
        //<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
        //  <w:body>
        //    <w:p>
        //      <w:r>
        //        <w:t>Hello World</w:t>
        //      </w:r>
        //    </w:p>
        //  </w:body>
        //</w:document>


        /// <summary>
        /// 将内容加粗,
        /// </summary>
        public static void readWordContent(string filepath) 
        {
            var doc = WordprocessingDocument.Open(filepath,true);
            Document document = doc.MainDocumentPart.Document;

            Paragraph p = document.Descendants<Paragraph>().FirstOrDefault();
            Run run=p.Descendants<Run>().FirstOrDefault();
            //RunProperties runProperties =run.AppendChild(new RunProperties(new Bold()));
            //加粗节点在文字后面是没有效果的，*****
            Text te = run.Descendants<Text>().FirstOrDefault();//找到text在前面加入加粗节点有效
            RunProperties runProperties = te.InsertBeforeSelf<RunProperties>(new RunProperties(new Bold()));
            doc.Dispose();
        }

        ///按照层级结构，从父级到子级挨个appendChild
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="msg"></param>
        public static void CreateWordDoc(string filepath, string msg)
        {
            WordprocessingDocument doc = WordprocessingDocument.Create(filepath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

            // Add a main document part. 
            MainDocumentPart mainPart = doc.AddMainDocumentPart();

            // Create the document structure and add some text.
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());

            // String msg contains the text, "Hello, Word!"
            run.AppendChild(new Text(msg));
            doc.Dispose();

        }
    }
}
