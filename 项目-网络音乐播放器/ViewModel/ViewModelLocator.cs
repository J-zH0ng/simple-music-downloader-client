/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:项目_网络音乐播放器"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using 项目_网络音乐播放器.ViewModels;

namespace 项目_网络音乐播放器.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<LocalMusicViewModel>();
            SimpleIoc.Default.Register<PlayAreaViewModel>();
            SimpleIoc.Default.Register<PlayListsControlViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        public LocalMusicViewModel LocalMusicViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LocalMusicViewModel>();
            }
        }
        public PlayAreaViewModel PlayAreaViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayAreaViewModel>();
            }
        }
        public PlayListsControlViewModel PlayListsControlViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayListsControlViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}