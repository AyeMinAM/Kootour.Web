using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using MVCSite.DAC.Entities;
using MVCSite.ViewResource;
using MVCSite.Common;

namespace MVCSite.Web.ViewModels
{
    public class Layout
    {
        public Layout()
        {
            IsSignedIn = false;
            IsIndex = false;
            SelectedPage = LayoutSelectedPage.Home;
            ViewType = LayoutViewType.Normal;
            PageTitle = "Welcome! | Kootour";  
            MetaName = "Explore our world like never before";
            MetaDescription = "Kootour is a place for both tourists and local guides to explore together, so you just need to confirm a few and you will have the best things of our world to explore.";
            MetaKeywords = "kootour,Kootour";
            UserID = 0;
            Role = (int)UserRole.Tourist;
        }
        
        public bool IsSignedIn { get; set; }
        /// <summary>
        /// Is user ever signed in from this computer
        /// </summary>
        public bool IsMember { get; set; }
        /// <summary>
        /// Is this used in a public web page(withoug javascipt controls)
        /// </summary>
        public bool IsIndex { get; set; }

        public bool RenderSignInSignUp { get; set; }
        public int MsgCount { get; set; }
        public List<DashBoardMsg> MsgList { get; set; }
        public LayoutSelectedPage SelectedPage { get; set; }
        public LayoutViewType ViewType { get; set; }
        public User CurrentUser { get; set; }
        public int UserID { get; set; }
        public string RequestUrl { get; set; }
        public string BrowserCss { get; set; }

        public string PageTitle { get; set; }
        public string MetaName { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int Role { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class MobileLayout
    {
        public MobileLayoutSelectedTab Tab;
        public bool IsDebuging { get; set; }
        public int UserId { get; set; }
        public string ReturnWebViewPath { get; set; }
        public string PageTitle { get; set; }

        public MobileLayout()
        {
            PageTitle = ViewStrings.SelectGameContents;
        }
    }
    public class CommunityLayout
    {
        public CommunityLayout()
        {
            IsDebuging = true;
        }
        public bool IsDebuging { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string PathLogBrowserMessage { get; set; }
        public string PathQuitBrowser { get; set; }
    }
    public class SimpleMsg
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public enum LayoutSelectedPage
    {
        Home,
        Account,
        AccountAfterLogOn,
        Company,
        Notifications,
        Card,
        Board,
        BoardPopup,
    }

    public enum LayoutViewType
    {
        Normal,
        Invitee,
        Hint4Login,

    }
    public enum MobileLayoutSelectedTab
    {
        PictureDictionary,
        Book,
        GetConsolePageData,
    }
}