using System;
namespace Sample.Views
{
    public class MyNavigationPage : NavigationPage
    {
        public MyNavigationPage()
        {
            BarBackgroundColor = Colors.White;
        }

        public MyNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = Colors.White;
        }
    }
}

