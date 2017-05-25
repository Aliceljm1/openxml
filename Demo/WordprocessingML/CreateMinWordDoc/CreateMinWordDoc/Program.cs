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
            CreateWordDoc("e:/test/test2.docx", "Hello World");
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

        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="msg"></param>
        public static void CreateWordDoc(string filepath, string msg)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(filepath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());

                // String msg contains the text, "Hello, Word!"
                run.AppendChild(new Text(msg));
            }
        }
    }
}
