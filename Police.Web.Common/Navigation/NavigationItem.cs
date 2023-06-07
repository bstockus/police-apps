namespace Police.Web.Common.Navigation {

    public class NavigationItem {

        public string Title { get; }
        public string ShortTitle { get; }
        public string Area { get; }
        public string Page { get; }

        public NavigationItem(string title, string shortTitle, string area, string page) {
            Title = title;
            ShortTitle = shortTitle;
            Area = area;
            Page = page;
        }

    }

}