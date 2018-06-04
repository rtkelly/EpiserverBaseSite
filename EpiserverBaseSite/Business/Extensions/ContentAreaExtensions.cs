using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Extensions
{
    public static class ContentAreaExtensions
    {
        private static readonly Injected<IContentLoader> _contentLoader;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentArea"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this ContentArea contentArea)
        {
            return contentArea == null || !contentArea.Items.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentArea"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetItems<T>(this ContentArea contentArea) where T : IContentData
        {
            return contentArea.IsNullOrEmpty() ? Enumerable.Empty<T>() : contentArea.Items.GetItems<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentArea"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetFilteredItems<T>(this ContentArea contentArea) where T : IContentData
        {
            return contentArea.IsNullOrEmpty() ? Enumerable.Empty<T>() : contentArea.FilteredItems.GetItems<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetItems<T>(this IEnumerable<ContentAreaItem> items) where T : IContentData
        {
            if (items == null)
            {
                yield break;
            }

            foreach (var item in items)
            {
                if (_contentLoader.Service.TryGet(item.ContentLink, out T model))
                {
                    yield return model;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T Get<T>(this ContentAreaItem item) where T : IContentData
        {
            if (item == null)
            {
                return default(T);
            }

            T result;

            _contentLoader.Service.TryGet<T>(item.ContentLink, out result);

            return result;
        }
    }
}