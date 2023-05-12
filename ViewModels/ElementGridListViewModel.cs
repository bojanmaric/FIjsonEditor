
using EditJsonFInspection.Events;
using EditJsonFInspection.Interfaces;
using EditJsonFInspection.Models;
using Microsoft.IdentityModel.Tokens;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace EditJsonFInspection.ViewModels
{
    public class ElementGridListViewModel : BindableBase
    {
        public readonly IDbReader _dbReader;
        public readonly IHelper _helper;
        private readonly IEventAggregator _eventAggregator;

        private ICollectionView _dataList;

        private string _filter;

        public ElementGridListViewModel(IDbReader dbreader, IHelper helper, IEventAggregator eventAggregator)
        {

            _dbReader = dbreader;
            _helper = helper;
            _eventAggregator = eventAggregator;
            InitView();


        }
        public void InitView()
        {
           
            GetAllElements();
            _eventAggregator.GetEvent<RefresListEvent>().Subscribe(() => _dataList.Refresh());

            _eventAggregator.GetEvent<SearchEvent>().Subscribe(FilterData);
           
        }

        private bool FilterList(object obj)
        {
            var infoModel = obj as InfoTextModel;
            if (_filter.IsNullOrEmpty()) return true;


            return infoModel.WorkPlaceName.ToUpper().Contains(_filter) || infoModel.ProdHierarchies.Where(x => x.ScaleType.ToUpper().Contains(_filter)).Count() > 0;
        }

        public void FilterData(string filter)
        {
            _filter = filter;
            _dataList.Refresh();
        }

        private void GetAllElements()
        {
            InfoTextList = _helper.GetAll();

            _dataList = CollectionViewSource.GetDefaultView(InfoTextList);
            _dataList.Filter = FilterList;

           
        }
        private InfoTextModel _selectedInfoText;
        public InfoTextModel SelectedInfoText
        {
            get { return _selectedInfoText; }
            set { SetProperty(ref _selectedInfoText, value); SelectEvent(); }
        }

        public void SelectEvent()
        {
            _eventAggregator.GetEvent<SelectItem>().Publish(SelectedInfoText);
        }

        private ObservableCollection<InfoTextModel> _infoTextList;
        public ObservableCollection<InfoTextModel> InfoTextList
        {
            get { return _infoTextList; }
            set { SetProperty(ref _infoTextList, value); }
        }
        
    }
}
