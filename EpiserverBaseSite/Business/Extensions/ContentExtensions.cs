using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web.Routing;
using EpiserverBaseSite.Business.Models;
using EpiserverBaseSite.Business.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Extensions
{
  
    public static class ContentExtensions
    {
        private static readonly Injected<IContentLoader> _contentLoader;
        private static readonly Injected<UrlResolver> _urlResolver;
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentLink"></param>
        /// <returns></returns>
        public static T TryGet<T>(this ContentReference contentLink) where T : IContentData
        {
            T result;
            _contentLoader.Service.TryGet(contentLink, out result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetContentUrl(this IContent content)
        {
            return (content == null) ? string.Empty : 
                _urlResolver.Service.GetUrl(content.ContentLink);


        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static IList<T> GetPagePath<T>(this T currentPage) where T : PageData
        {
            var contentList = _contentLoader.Service.GetAncestors(currentPage.ContentLink).Reverse()
                .SkipWhile(c => ContentReference.IsNullOrEmpty(c.ParentLink))
                .OfType<T>();

            return contentList.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static T GetTopPage<T>(this T currentPage) where T : PageData
        {
            var rootPage = _contentLoader.Service.GetAncestors(currentPage.ContentLink)
                .Where(p => p.ContentLink != ContentReference.StartPage && p.ContentLink != ContentReference.RootPage)
                .Reverse()
                .OfType<T>()
                .FirstOrDefault();

            return rootPage ?? currentPage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootPage"></param>
        /// <param name="filtered"></param>
        /// <returns></returns>
        public static IList<T> GetChildren<T>(PageData rootPage, bool filtered = true) where T : PageData
        {
            if (rootPage == null)
            {
                return new List<T>();
            }

            var children = _contentLoader.Service.GetChildren<T>(rootPage.ContentLink);

            if (!filtered)
                return children.ToList();

            var filteredChildren = EPiServer.Filters.FilterForVisitor.Filter(children);

            return filteredChildren.Cast<T>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public static LinkView GetPageLink(this PageData pageData)
        {
            if (pageData == null)
                return null;

            return new LinkView()
            {
                Title = pageData.GetTitle(),
                Url = pageData.GetContentUrl(),
                Target = pageData.GetPageLinkTarget(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static string GetPageLinkTarget(this PageData currentPage)
        {
            var targetFrame = currentPage.Property["PageTargetFrame"] as PropertyFrame;

            return (targetFrame != null) ? targetFrame.FrameName : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentData"></param>
        /// <returns></returns>
        public static string GetTitle(this PageData contentData)
        {
            string pageTitle = "";

            if (contentData is ISitePage)
            {
                pageTitle = ((ISitePage)contentData).PageTitle;
            }

            return (!pageTitle.IsNullOrEmpty()) ? pageTitle : contentData.Name;
        }


    }
}