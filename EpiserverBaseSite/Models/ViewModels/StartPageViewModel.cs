using EpiserverBaseSite.Models.Abstract;
using EpiserverBaseSite.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Models.ViewModels
{
    public class StartPageViewModel : PageViewModel<StartPage>
    {

        public StartPageViewModel(StartPage currentPage, ISiteSettings settings) 
            : base(currentPage, settings)
        {

        }


    }
}