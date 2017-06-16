using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
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
            ViewModel.LoadArticle(articleIndex);

            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("Image");
            //if (animation != null)
            //{
            //    animation.TryStart(Image);
            //    Image.Opacity = 1;
            //}

            Image.Opacity = 1;
            animation.TryStart(Image);


            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("Image", Image);

            // Add a fade out effect
            Transitions = new TransitionCollection {new ContentThemeTransition()};
        }

    }
}