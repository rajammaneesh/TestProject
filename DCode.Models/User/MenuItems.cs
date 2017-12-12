namespace DCode.Models.User
{
    public class MenuItem
    {
        public string MenuItemName { get; set; }
        public string NavigationUrl { get; set; }
        public string ImageUrlActive { get; set; }
        public string ImageUrlInactive { get; set; }
        public string CssClass { get; set; }
        //required for angularjs
        public string State { get; set; }
        public string TabName { get; set; }
    }
}
