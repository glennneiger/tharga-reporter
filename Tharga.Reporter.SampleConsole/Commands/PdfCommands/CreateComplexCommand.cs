﻿using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Tharga.Reporter.Engine.Entity;
using Tharga.Reporter.Engine.Entity.Element;
using Tharga.Toolkit.Console.Command.Base;
using Font = Tharga.Reporter.Engine.Entity.Font;
using Rectangle = Tharga.Reporter.Engine.Entity.Element.Rectangle;

namespace Tharga.Reporter.ConsoleSample.Commands.PdfCommands
{
    public class CreateComplexCommand : ActionCommandBase
    {
        public CreateComplexCommand()
            : base("complex", "create a complex document")
        {
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var documentProperties = CreateDocumentProperties(paramList, ref index);
            var template = CreateTemplate();

            var debug = QueryParam("Debug", GetParam(paramList, index++), new List<KeyValuePair<bool, string>> { new KeyValuePair<bool, string>(true, "Yes"), new KeyValuePair<bool, string>(false, "No") });

            await PdfCommand.RenderPdfAsync(template, documentProperties, null, null, debug);

            return true;
        }

        private DocumentProperties CreateDocumentProperties(string paramList, ref int index)
        {
            var title = QueryParam<string>("Title", GetParam(paramList, index++));
            var author = QueryParam<string>("Author", GetParam(paramList, index++));
            var creator = QueryParam<string>("Creator", GetParam(paramList, index++));
            var subject = QueryParam<string>("Subject", GetParam(paramList, index++));

            var documentProperties = new DocumentProperties
            {
                Title = title,
                Author = author,
                Creator = creator,
                Subject = subject,
            };
            return documentProperties;
        }

        private Template CreateTemplate()
        {
            var firstPageSection = new Section { Name = "First Page" };
            firstPageSection.Pane.ElementList.Add(new Text { Top = "50%", Left = "1cm", Value = "Firs page of complex sample document", Font = new Font { Size = 22 }, TextAlignment = TextBase.Alignment.Center });
            var template = new Template(firstPageSection);

            var indexSection = new Section { Name = "Index", Margin = new UnitRectangle { Left = "2cm", Right = "1cm", Top = "3cm", Bottom = "3cm" } };
            indexSection.Footer.ElementList.Add(new Text { Value = "Page {PageNumber} of {TotalPages}" });
            template.SectionList.Add(indexSection);

            var mainSection = new Section { Name = "Main", Margin = new UnitRectangle { Left = "2cm", Right = "1cm", Top = "3cm", Bottom = "3cm" } };
            //mainSection.Header.ElementList.Add(new Line { Width = "100%", Height = "100%" });
            mainSection.Header.ElementList.Add(new Text { Value = "{Title}", Left = "2cm" });
            mainSection.Header.ElementList.Add(new Text { Value = "{Author}" });
            mainSection.Header.ElementList.Add(new Text { Value = "{Creator}" });
            mainSection.Header.ElementList.Add(new Text { Value = "{Subject}" });

            //mainSection.Pane.ElementList.Add(new Line { Width = "100%", Height = "100%" });
            var referencePoint = new ReferencePoint { Top = "1cm", Left = "1cm", Stack = ReferencePoint.StackMethod.Vertical };
            referencePoint.ElementList.Add(new Text { Value = "{Title}" });
            referencePoint.ElementList.Add(new Text { Value = "{Author}" });
            referencePoint.ElementList.Add(new Text { Value = "{Creator}" });
            referencePoint.ElementList.Add(new Text { Value = "{Subject}" });
            mainSection.Pane.ElementList.Add(referencePoint);

            mainSection.Footer.ElementList.Add(new Rectangle { Width = "100%", Height = "100%", BackgroundColor = Color.Red });
            mainSection.Footer.ElementList.Add(new Rectangle { Left = "0", Top = "0", Width = "2cm", Height = "2cm", BackgroundColor = Color.Yellow });
            mainSection.Footer.ElementList.Add(new Line { Width = "3cm", Height = "3cm" });
            mainSection.Footer.ElementList.Add(new Text { Value = "Page {PageNumber} of {TotalPages}", TextAlignment = TextBase.Alignment.Left });
            mainSection.Footer.ElementList.Add(new Text { Value = "Page {PageNumber} of {TotalPages}", TextAlignment = TextBase.Alignment.Center });
            mainSection.Footer.ElementList.Add(new Text { Value = "Page {PageNumber} of {TotalPages}", TextAlignment = TextBase.Alignment.Right });
            template.SectionList.Add(mainSection);
            return template;
        }
    }
}