using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CreatorsNews
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticlePage : Page
    {
        public ArticlePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var articleIndex = (int) e.Parameter;
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
            if (animation != null)
            {
                Image.Opacity = 0;
                // Wait for image opened. In future Insider Preview releases, this won't be necessary.
                Image.ImageOpened += (sender_, e_) =>
                {
                    Image.Opacity = 1;
                    animation.TryStart(Image);
                };
            }

            ViewModel.LoadArticle(articleIndex);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", Image);

            // Add a fade out effect
            Transitions = new TransitionCollection();
            Transitions.Add(new ContentThemeTransition());
        }

    }
}