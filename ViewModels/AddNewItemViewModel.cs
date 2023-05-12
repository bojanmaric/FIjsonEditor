using EditJsonFInspection.DBContext;
using EditJsonFInspection.Events;
using EditJsonFInspection.Interfaces;
using EditJsonFInspection.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EditJsonFInspection.ViewModels
{
    public class AddNewItemViewModel : BindableBase,INavigationAware
    {
        public readonly IHelper _helper;
        IEventAggregator _eventAggregator;

        public AddNewItemViewModel(IEventAggregator eventAggregator, IHelper helper)

        {
            _helper = helper;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<SelectItem>().Subscribe(x=>InfoTextItem= JsonConvert.DeserializeObject<InfoTextModel>(JsonConvert.SerializeObject(x)));

            _eventAggregator.GetEvent<CleanMainFields>().Subscribe(() => { InfoTextItem = null; });
            DeleteProdHierElement = new DelegateCommand<object>(CleanFromProdHier);

            AddNewItemCommand = new DelegateCommand(AddNew);
            InitValues();
        }

        private void InitValues()
        {
            LanguageList = _helper.GetLanguageList();
            WorkPlaceList = _helper.GeWorkPlaceList();

            
        }
        private void CleanFromProdHier(object parameter)
        {
            var clickedElement = parameter as ProdHierarchiesModel;
            if (InfoTextItem.ProdHierarchies.Contains(clickedElement) && clickedElement.ScaleType != null)
            {
                InfoTextItem.ProdHierarchies.Remove(clickedElement);

                var info = JsonConvert.DeserializeObject<InfoTextModel>(JsonConvert.SerializeObject(InfoTextItem));

                InfoTextItem = info;

            }
            
        }
        public async void AddNew()
        {
            InfoTextItem.WorkPlaceType = WorkPlace.Ident;
            InfoTextItem.WorkPlaceName = WorkPlace.WorkplaceTypName;

            if (await _helper.AddNewEntryIntoFile(InfoTextItem))
            {
                _eventAggregator.GetEvent<RefresListEvent>().Publish();

                MessageBox.Show($"Successfully added new item into file", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"You cannot add same item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            InfoTextItem = null;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            InfoTextItem = null;

            _eventAggregator.GetEvent<SelectItem>().Unsubscribe(x => InfoTextItem = JsonConvert.DeserializeObject<InfoTextModel>(JsonConvert.SerializeObject(x)));

        }
        private bool _enableDelete;
        public bool EnableDelete
        {
            get { return _enableDelete; }
            set { SetProperty(ref _enableDelete, value); }
        }

        private ProdHierarchiesModel _prodSelectedHierarchies;
        public ProdHierarchiesModel ProdSelectedHierarchies
        {
            get { return _prodSelectedHierarchies; }
            set
            {
                SetProperty(ref _prodSelectedHierarchies, value != null ? value : null); if (value != null)
                {
                    EnableDelete = true;
                }
                else
                {
                    EnableDelete = false;
                }
            }
        }

        private List<string> _languageList;
        public List<string> LanguageList
        {
            get { return _languageList; }
            set { SetProperty(ref _languageList, value); }
        }

        private T1t001EpworkplaceTypName _workPlace;
        public T1t001EpworkplaceTypName WorkPlace
        {
            get { return _workPlace; }
            set { SetProperty(ref _workPlace, value); }
        }
        private InfoTextModel _infoTextItem;
        public InfoTextModel InfoTextItem
        {
            get { return _infoTextItem; }
            set { SetProperty(ref _infoTextItem, value); if (_infoTextItem != null) { WorkPlace = WorkPlaceList.Single(x => x.Ident == InfoTextItem.WorkPlaceType); }; }
        }

        private List<T1t001EpworkplaceTypName> _workPlaceList;
        public List<T1t001EpworkplaceTypName> WorkPlaceList
        {
            get { return _workPlaceList; }
            set { SetProperty(ref _workPlaceList, value); }
        }
        private MessageBoxResult _selectedResult;
        public MessageBoxResult SelectedResult
        {
            get => _selectedResult;
            set => SetProperty(ref _selectedResult, value);
        }
        //Enum.GetNames(typeof(MessageBoxReslut))

        public string[] DialResult = Enum.GetNames(typeof(MessageBoxResult));
       
        public DelegateCommand AddNewItemCommand { get; set; }
        public DelegateCommand<object> DeleteProdHierElement { get; set; } 

    }
}
