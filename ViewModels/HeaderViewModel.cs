using EditJsonFInspection.Events;
using EditJsonFInspection.Interfaces;
using EditJsonFInspection.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace EditJsonFInspection.ViewModels
{
    public class HeaderViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IHelper _helper;

        public HeaderViewModel(IEventAggregator eventAggregator, IHelper helper, IRegionManager regionManager)
        {

            _regionManager = regionManager;

            _eventAggregator = eventAggregator;
            _helper = helper;

            AddCommand = new DelegateCommand(() =>
            {
                _regionManager.RequestNavigate("FieldsPart", nameof(AddNewItem));
            });

            NavigateToMainPage = new DelegateCommand(() =>
            {
                _regionManager.RequestNavigate("FieldsPart", nameof(MainFieldsParts));
            });

            EditCommand = new DelegateCommand(() =>
            {
                _eventAggregator.GetEvent<EditEvent>().Publish(true);
            });

            CleanFieldsCommand = new DelegateCommand(() => { _eventAggregator.GetEvent<CleanMainFields>().Publish(); });

            ConvertOldData = new DelegateCommand(() =>
            {
                if (_helper.ConvertOldDataToNew())
                {

                    MessageBox.Show("Successfuly converted data \nRestart program to see new list!!!");
                }
                else
                {
                    MessageBox.Show("Error occured convered data");

                }
            });

            DeleteCommand = new DelegateCommand(() => { _eventAggregator.GetEvent<DeleteEvent>().Publish(); });
        }



        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); Search(); }
        }
    
        public void Search()
        {
            _eventAggregator.GetEvent<SearchEvent>().Publish(_searchText.ToUpper());
        }

       
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand ConvertOldData { get; set; }
        public DelegateCommand NavigateToMainPage { get; set; }
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand CleanFieldsCommand { get; set; }

    }
}
