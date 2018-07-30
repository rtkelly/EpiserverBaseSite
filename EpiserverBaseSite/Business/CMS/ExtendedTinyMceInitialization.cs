using EPiServer.Cms.TinyMce.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.CMS
{

    [ModuleDependency(typeof(TinyMceInitialization))]
    public class ExtendedTinyMceInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.Configure<TinyMceConfiguration>(config =>
            {
                config.Default()
                    .ContentCss("/static/css/editor.css")
                    .Menubar("file edit insert view format table tools help")
                    .Toolbar(
                        "epi-link | epi-image-editor | epi-personalized-content | cut copy paste | fullscreen",
                        "styleselect  formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat",
                        "table | toc | codesample")
                    .BlockFormats("Paragraph=p;Header 1=h1;Header 2=h2;Header 3=h3");
                    
            });
        }

        public void Initialize(InitializationEngine context)
        {

        }

        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}