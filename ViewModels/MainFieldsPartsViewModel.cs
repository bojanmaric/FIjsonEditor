using EditJsonFInspection.DBContext;
using EditJsonFInspection.Events;
using EditJsonFInspection.Interfaces;
using EditJsonFInspection.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EditJsonFInspection.ViewModels
{
    public class MainFieldsPartsViewModel : BindableBase, INavigationAware
    {
        private readonly IHelper _helper;
        private readonly IEventAggregator _eventAggregator;
        public MainFieldsPartsViewModel(IEventAggregator eventAggregator, IHelper helper)
        {
            _helper = helper;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SelectItem>().Subscribe(x => { InfoOldState = x; InfoObject = JsonConvert.DeserializeObject<InfoTextModel>(JsonConvert.SerializeObject(x)); });

            _eventAggregator.GetEvent<CleanMainFields>().Subscribe(() => { InfoObject = null; });
            _eventAggregator.GetEvent<EditEvent>().Subscribe(x => { if (!IsEnabledEdit) { IsEnabledEdit = x; } else { IsEnabledEdit = false; } });
            _eventAggregator.GetEvent<DeleteEvent>().Subscribe(DeleteInfoItem);

            EditItemCommand = new DelegateCommand(Edit);


            LanguageList = _helper.GetLanguageList();
            WorkPlaceList = _helper.GeWorkPlaceList();

            DeleteProdHierElement = new DelegateCommand<object>(CleanFromProdHier);
            IsEnabledEdit = false;
        }

        private void CleanFromProdHier(object parameter)
        {
            var clickedElement = parameter as ProdHierarchiesModel; 
            if (InfoObject.ProdHierarchies.Contains(clickedElement) && clickedElement.ScaleType != null)
            {
                InfoObject.ProdHierarchies.Remove(clickedElement);

                var info = JsonConvert.DeserializeObject<InfoTextModel>(JsonConvert.SerializeObject(InfoObject));

                InfoObject = info;
            
            }
        }

        private async void DeleteInfoItem() 
        {
            if (InfoOldState != null)
            {

                var result = MessageBox.Show($"Do you want to delete item? \n  WorkPlace: {InfoOldState.WorkPlaceName}", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (await _helper.DeleteItem(InfoOldState))
                    {
                        MessageBox.Show($"Successfuly deleted item", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
        }

        public async void Edit()
        {                        
            if (InfoOldState != null && InfoObject != null)
            {
                InfoObject.WorkPlaceName = WorkPlace.WorkplaceTypName;
                InfoObject.WorkPlaceType = WorkPlace.Ident;
                if (!_helper.IsElementsEquals(_infoOldState, _infoObject))
                {

                    if (await _helper.UpdateEntryIntoFile(InfoOldState, InfoObject))
                    {
                        _eventAggregator.GetEvent<RefresListEvent>().Publish();
                        IsEnabledEdit = false;
                        MessageBox.Show($"Successfuly updated element", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error happens occured updating element in file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Selected element isn't changed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"You have to select element", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private InfoTextModel _infoObject;
        public InfoTextModel InfoObject
        {
            get { return _infoObject; }
            set
            {
                SetProperty(ref _infoObject, value); if (InfoObject != null)
                {
                    WorkPlace = WorkPlaceList.Single(x => x.Ident == InfoObject.WorkPlaceType);
                }
            }
        }
       /* private bool _enableDelete;
        public bool EnableDelete
        {
            get { return _enableDelete; }
            set { SetProperty(ref _enableDelete, value); }
        }*/

        /*private ProdHierarchiesModel _prodSelectedHierarchies;
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
*/
        private InfoTextModel _infoOldState;
        public InfoTextModel InfoOldState
        {
            get { return _infoOldState; }
            set { SetProperty(ref _infoOldState, value); }
        }

        private bool _isEnabledEdit;
        public bool IsEnabledEdit
        {
            get { return _isEnabledEdit; }
            set
            { SetProperty(ref _isEnabledEdit, value); } 
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
        private List<T1t001EpworkplaceTypName> _workPlaceList;
        public List<T1t001EpworkplaceTypName> WorkPlaceList
        {
            get { return _workPlaceList; }
            set { SetProperty(ref _workPlaceList, value); }
        }

        public DelegateCommand EditItemCommand { get; set; }
        public DelegateCommand<object> DeleteProdHierElement { get; set; }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            InfoObject = null;
            InfoOldState = null;
            IsEnabledEdit = false;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

            _eventAggregator.GetEvent<SelectItem>().Unsubscribe(x => { InfoOldState = x; InfoObject = JsonConvert.DeserializeObject<InfoTextModel>(JsonConvert.SerializeObject(x)); });
            _eventAggregator.GetEvent<DeleteEvent>().Subscribe(DeleteInfoItem);

            InfoObject = null;
            InfoOldState = null;
            IsEnabledEdit = false;
        }

    }
}
