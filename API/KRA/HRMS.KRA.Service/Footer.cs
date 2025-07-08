using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HRMS.KRA.Service
{
    public partial class Footer : PdfPageEventHelper
    {
        private readonly string m_webRootPath;
        public Footer(string webRootPath)
        {
            m_webRootPath = webRootPath;
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            PdfPTable table = null;
            PdfPCell cell = null;
            table = new PdfPTable(2);

            table.HorizontalAlignment = 0;
            table.TotalWidth = 470f;
            table.LockedWidth = true;

            //   string imagePath = ConfigurationManager.AppSettings["SenecaLogo"].ToString();
            string imagePath = Path.Combine(m_webRootPath, @"Images/SenecaLogo.png") ; 
            Image image = Image.GetInstance(imagePath);
            image.ScaleToFit(150f, 150f);
            image.Alignment = Element.ALIGN_RIGHT;

            // imagePath = ConfigurationManager.AppSettings["MidSizeWorkspace"].ToString();
            imagePath = Path.Combine(m_webRootPath, @"Images/KRA.png");  
            Image WorkSpaceLogo = Image.GetInstance(imagePath);
            WorkSpaceLogo.Alignment = Element.ALIGN_LEFT;

            cell = new PdfPCell(WorkSpaceLogo);
            cell.Border = 0;
            cell.FixedHeight = 40;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            WorkSpaceLogo.ScaleAbsolute(145, 35);
           // cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;            
            table.AddCell(cell);

            cell = new PdfPCell(image);
            cell.Border = 0;           
            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            table.AddCell(cell);
            table.SpacingAfter = 20;
            document.Add(table);

        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
             base.OnEndPage(writer, doc);            
            PdfPTable footerTbl = new PdfPTable(2);
            PdfPCell cell = null;
            footerTbl.TotalWidth = 500;
            footerTbl.HorizontalAlignment = 0;            
            footerTbl.LockedWidth = true;            

            string data = @$"SENECAGLOBAL IT SERVICES PRIVATE LIMITED, 
3RD FLOOR, AUROBINDO GALAXY, TSIIC RAIDURG			
HYDERABAD - 500081, TELANGANA, INDIA		
WWW.SENECAGLOBAL.COM   |   COMPANY IDENTITY NUMBER - U72200TG2007PTC055624
";
            Paragraph footer = new Paragraph(data, FontFactory.GetFont("Arial", 7, new BaseColor(186, 184, 108)));
            footer.Alignment = Element.ALIGN_LEFT;
            cell = new PdfPCell(footer);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.PaddingLeft = 0;
            footerTbl.AddCell(cell);           
            string imagePath = Path.Combine(m_webRootPath, @"Images/footer.jpg");
            Image image = Image.GetInstance(imagePath);
            image.ScaleToFit(150f, 150f);
            image.Alignment = Element.ALIGN_RIGHT;
            cell = new PdfPCell(image);
            cell.Border = 0;
            cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            footerTbl.AddCell(cell);
            footerTbl.WriteSelectedRows(0, -1, 50, (doc.BottomMargin - 5), writer.DirectContent);
        }
    }
}
