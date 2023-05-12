using EditJsonFInspection.Controllers;
using EditJsonFInspection.Interfaces;
using EditJsonFInspection.ViewModels;
using EditJsonFInspection.Views;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace EditJsonFInspection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();

            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("HeaderRegion", nameof(Header));
            regionManager.RegisterViewWithRegion("FieldsPart", nameof(MainFieldsParts));
            regionManager.RegisterViewWithRegion("ContentRegion", nameof(ElementGridList));

            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Header, HeaderViewModel>();
            containerRegistry.RegisterForNavigation<ElementGridList, ElementGridListViewModel>();
            containerRegistry.RegisterForNavigation<MainFieldsParts, MainFieldsPartsViewModel>();
            containerRegistry.RegisterForNavigation<AddNewItem, AddNewItemViewModel>();

            // Setup Dependecy Injection
            containerRegistry.RegisterSingleton<IDbReader, DbReader>();
            containerRegistry.RegisterSingleton<IHelper, Helper>();

        }
    }
}
