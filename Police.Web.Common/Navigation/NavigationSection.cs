namespace Police.Web.Common.Navigation {

    public class NavigationSection {

        public string Title { get; }
        public NavigationItem[] NavigationItems { get; }

        public NavigationSection(string title, params NavigationItem[] navigationItems) {
            Title = title;
            NavigationItems = navigationItems;
        }

    }

}