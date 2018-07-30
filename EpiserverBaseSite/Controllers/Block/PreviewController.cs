using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;


namespace EpiserverBaseSite.Controllers.Block
{

    [TemplateDescriptor(Inherited = true, TemplateTypeCategory = TemplateTypeCategories.MvcController,
     Tags = new[] { RenderingTags.Preview }, AvailableWithoutTag = false)]
    public class PreviewController : Controller, IRenderTemplate<BlockData>
    {
        public ActionResult Index(IContentData currentContent)
        {
            return View("BlockPreview", currentContent);
        }
    }
}