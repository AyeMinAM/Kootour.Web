using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;

namespace MVCSite.Web.ViewModels
{
    public enum AccountSelectSection
    { 
        Boards=1,
        Activities,
        Notifications,
        Settings,
        Favorites,
        Books
    };
    public class AccountNavigateModel
    {
        public string UserInitial
        {
            get;
            set;
        }
        public string UserFullName
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public bool UseAvatar
        {
            get;
            set;
        }
        public string AvatarUrl
        {
            get;
            set;
        }
        public int UseId
        {
            get;
            set;
        }
        public string BoardsCSS { get; set; }
        public string ActivitiesCSS { get; set; }
        public string NotificationsCSS { get; set; }
        public string SettingsCSS { get; set; }
        public string FavoritesCSS { get; set; }
        public string BooksCSS { get; set; } 

    }
}