using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CreatorsNews
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static int? _navigatedIndex;
        public MainPage()
        {
            InitializeComponent();

            // Extend the app into the titlebar so that we can apply acrylic
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void ArticleClicked(object sender, ItemClickEventArgs e)
        {
            var articleIndex = Array.IndexOf(ViewModel.Articles, e.ClickedItem);

            var container = ArticlesGridView.ContainerFromItem(e.ClickedItem) as GridViewItem;
            if (container != null)
            {
                var root = (FrameworkElement)container.ContentTemplateRoot;
                var image = (UIElement)root.FindName("Image");

                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", image);
            }

            // Add a fade out effect
            Transitions = new TransitionCollection();
            Transitions.Add(new ContentThemeTransition());

            Frame.Navigate(typeof(ArticlePage), _navigatedIndex = articleIndex);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Don't use vertical entrance animation with connected animation
            if (e.NavigationMode == NavigationMode.Back)
            {
                EntranceTransition.FromVerticalOffset = 0;
            }

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void ArticlesGridView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_navigatedIndex != null)
            {
                // May be able to perform backwards Connected Animation
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
                if (animation != null)
                {
                    var item = ViewModel.Articles[_navigatedIndex.Value];

                    ArticlesGridView.ScrollIntoView(item, ScrollIntoViewAlignment.Default);
                    ArticlesGridView.UpdateLayout();

                    var container = ArticlesGridView.ContainerFromItem(item) as GridViewItem;
                    if (container != null)
                    {
                        var root = (FrameworkElement)container.ContentTemplateRoot;
                        var image = (Image)root.FindName("Image");

                        image.Opacity = 0;

                        // this never fires
                        //image.ImageOpened += (sender_, e_) =>
                        //{
                        //    image.Opacity = 1;
                        //    animation.TryStart(image);
                        //};

                        // Have to run this directly
                        image.Opacity = 1;
                        animation.TryStart(image);
                    }
                    else
                    {
                        animation.Cancel();
                    }
                }

                _navigatedIndex = null;
            }
        }
    }
}