using EPiServer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Extensions
{
    public static class ExceptionExtensions
    {
        public static void LogError(this Exception ex)
        {
            var logger = LogManager.GetLogger();

            if (logger != null)
                logger.Error(ex.Message);
        }
    }
}