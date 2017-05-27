using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using D = DocumentFormat.OpenXml.Drawing;

namespace iflytek.service.ppt2html5
{
    /// <summary>
    /// 提供openxml的操作方法，将ppt中非法动画触发器修复
    /// </summary>
    public class PPTOpenXml
    {

        /// <summary>
        /// childTnLst> par>cTn
        ///cTn 中的nodetype值，目标替换clickPar 为withEffect, 此属性决定触发时机
        /// </summary>
        public static void fixERROR1(string presentationFile)
        {

             presentationFile = @"F:\问题PPT\动画没有触发事件\0.0问题单页.pptx";//获取动画

            PresentationDocument presentationDocument = PresentationDocument.Open(presentationFile, true);
            changeErrorAttr(presentationDocument);
            presentationDocument.Dispose();

        }

        /// <summary>
        /// 经验积累
        /// </summary>
        /// <param name="presentationDocument"></param>
        public static void changeErrorAttr(PresentationDocument presentationDocument)
        {
            if (presentationDocument == null)
            {
                string error = "presentationDocument == null";
                Console.WriteLine(error);
            }
            PresentationPart presentationPart = presentationDocument.PresentationPart;

            if (presentationPart != null &&
                presentationPart.Presentation != null)
            {
                Presentation presentation = presentationPart.Presentation;

                if (presentation.SlideIdList != null)
                {
                    foreach (var slideId in presentation.SlideIdList.Elements<SlideId>())
                    {
                        SlidePart slidePart = presentationPart.GetPartById(slideId.RelationshipId) as SlidePart;
                        var ctn = slidePart.Slide.Descendants<CommonTimeNode>();
                        foreach (var item in ctn)
                        {
                            Console.WriteLine("id=" + item.Id + ",presetID=" + item.PresetId + ",presetClass=" + item.PresetClass + ",nodetype=" + item.NodeType);

                            if (!String.IsNullOrEmpty(item.Id) && !String.IsNullOrEmpty(item.PresetId)
                                && !String.IsNullOrEmpty(item.PresetClass) && !String.IsNullOrEmpty(item.NodeType))
                            {
                                if (item.NodeType == "clickPar")
                                {
                                    item.NodeType = TimeNodeValues.WithEffect;
                                }
                            }
                        }
                    }
                }
            }
        }


        // Get a list of the titles of all the slides in the presentation.
        public static IList<string> GetSlideTitles(PresentationDocument presentationDocument)
        {
            if (presentationDocument == null)
            {
                throw new ArgumentNullException("presentationDocument");
            }
            PresentationPart presentationPart = presentationDocument.PresentationPart;

            if (presentationPart != null &&
                presentationPart.Presentation != null)
            {
                Presentation presentation = presentationPart.Presentation;

                if (presentation.SlideIdList != null)
                {
                    List<string> titlesList = new List<string>();

                    foreach (var slideId in presentation.SlideIdList.Elements<SlideId>())
                    {
                        SlidePart slidePart = presentationPart.GetPartById(slideId.RelationshipId) as SlidePart;
                        var effects = slidePart.Slide.Descendants<AnimateEffect>();
                        foreach (var effect in effects)
                        {
                            Console.WriteLine("effect.filter=" + effect.Filter);
                        }

                        var anis = slidePart.Slide.Descendants<Animate>();
                        foreach (var a in anis)
                        {
                            Console.WriteLine("calcmode=" + a.CalculationMode.ToString());
                        }


                        string title = GetSlideTitle(slidePart);

                        titlesList.Add(title);
                    }

                    return titlesList;
                }

            }

            return null;
        }

        // Get the title string of the slide.
        public static string GetSlideTitle(SlidePart slidePart)
        {
            if (slidePart == null)
            {
                throw new ArgumentNullException("presentationDocument");
            }

            // Declare a paragraph separator.
            string paragraphSeparator = null;

            if (slidePart.Slide != null)
            {
                // Find all the title shapes.
                var shapes = from shape in slidePart.Slide.Descendants<Shape>()
                             where IsTitleShape(shape)
                             select shape;

                StringBuilder paragraphText = new StringBuilder();

                foreach (var shape in shapes)
                {
                    // Get the text in each paragraph in this shape.
                    foreach (var paragraph in shape.TextBody.Descendants<D.Paragraph>())
                    {
                        // Add a line break.
                        paragraphText.Append(paragraphSeparator);

                        foreach (var text in paragraph.Descendants<D.Text>())
                        {
                            paragraphText.Append(text.Text);
                        }

                        paragraphSeparator = "\n";
                    }
                }

                return paragraphText.ToString();
            }

            return string.Empty;
        }


        // Determines whether the shape is a title shape.
        private static bool IsTitleShape(Shape shape)
        {
            var placeholderShape = shape.NonVisualShapeProperties.ApplicationNonVisualDrawingProperties.GetFirstChild<PlaceholderShape>();
            if (placeholderShape != null && placeholderShape.Type != null && placeholderShape.Type.HasValue)
            {
                switch ((PlaceholderValues)placeholderShape.Type)
                {
                    // Any title shape.
                    case PlaceholderValues.Title:

                    // A centered title.
                    case PlaceholderValues.CenteredTitle:
                        return true;

                    default:
                        return false;
                }
            }
            return false;
        }

    }
}
