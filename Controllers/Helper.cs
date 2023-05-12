using EditJsonFInspection.DBContext;
using EditJsonFInspection.Interfaces;
using EditJsonFInspection.Models;
using ImTools;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace EditJsonFInspection.Controllers
{
    public class Helper : IHelper
    {
        // Old path of json
       // public static string convertJsonPath = "./data/InfoTextCommandFile.json";


        // New path with new structure which we currently use in program
        public static string dataPath = "./data/InfoTextCommandFile.json";


        public readonly IDbReader _dbreader;
        public List<T1t001EpworkplaceTypName> WorkPlaceList;
        ObservableCollection<InfoTextModel> Mainlist;
        public List<string> Language;
        public List<MessageBoxResult> messageBoxResultsList;

        public Helper(IDbReader dbreader)
        {

            WorkPlaceList = new();
            _dbreader = dbreader;
            Language = new List<string>
            {
                "en",
                "de"
            };
            messageBoxResultsList = new();
        }
        public async Task<bool> AddNewEntryIntoFile(InfoTextModel infoEntry)
        {

            foreach (InfoTextModel item in Mainlist)
            {
                if (IsElementsEquals(item, infoEntry))
                {
                    return  false;
                }
            }
            Mainlist.Add(infoEntry);

            return await SaveFile();
        }

        public ObservableCollection<InfoTextModel> GetAll()
        {

            Mainlist = JsonConvert.DeserializeObject<ObservableCollection<InfoTextModel>>(File.ReadAllText(dataPath));

            WorkPlaceList = _dbreader.GetAllNameOfWorkPlace();

            foreach (InfoTextModel item in Mainlist)
            {
                item.WorkPlaceName = WorkPlaceList.FindFirst(x => x.Ident == item.WorkPlaceType).WorkplaceTypName;

            }
            return Mainlist;
        }

        public async Task<bool> SaveFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Mainlist);
                await File.WriteAllTextAsync(dataPath, json);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error happens saving data\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }

        public async Task<bool> UpdateEntryIntoFile(InfoTextModel oldEntry, InfoTextModel newEntry)
        {
            int index=Mainlist.IndexOf(oldEntry);


            Mainlist[index] = newEntry;


            return await SaveFile();
        }
        public async Task<bool> DeleteItem(InfoTextModel item)
        {
            Mainlist.Remove(item);

            return await SaveFile();
            

        }
        public bool ConvertOldDataToNew()
        {

            List<InfoTextModel> InfoTextlist = new();

            OpenFileDialog dialog = new();
            dialog.Filter= "Json files (*.json)|*.json";
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.FileName))
            {
                MessageBox.Show("You have to choose json file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            try
            {
                //<InfoCurrentModel>
                dynamic file = JsonConvert.DeserializeObject(File.ReadAllText(dialog.FileName));


                foreach (var item in file["ShowInfosForOperator"])
                {
                    // Reading old prod hierarhy and take from db name and type

                    List<ProdHierarchiesModel> prodHierarchyList = new();

                    foreach (string subItem in item["ProdHierarchies"])
                    {
                        ProdHierarchiesModel prodHie = new ProdHierarchiesModel();

                        (prodHie.ScaleName, prodHie.ScaleType) = _dbreader.GetNameAndTypeOfScale(subItem);

                        prodHie.GroupName = subItem;

                        prodHierarchyList.Add(prodHie);
                    }
                    // Take infoMessages object

                    var infoMessageModel = item["InfoMessages"];


                    InfoMessageModel infoMsg = new InfoMessageModel()
                    {
                        Language = infoMessageModel["Language"],
                        Text = infoMessageModel["Text"]
                    };

                    int workPlace = item["WorkPlaceTyp"];

                    //  string workPlaceName = _dbreader.GetNameOfWorkPlace(workPlace);

                    List<ButtonConfigurationModel> buttons = new();

                    foreach (var buttonItem in item["ButtonsConfigruation"])
                    {
                        buttons.Add(new ButtonConfigurationModel()
                        {
                            WhichButton = buttonItem["WhichButton"],
                            ButtonText = buttonItem["ButtonText"],
                            Visuable = buttonItem["Visuable"],
                            MyDialogResult = buttonItem["MyDialogResult"]

                        });

                    }

                    InfoTextlist.Add(new InfoTextModel()
                    {
                        ProdHierarchies = prodHierarchyList,
                        InfoMessage = infoMsg,
                        KeyShowMessagePosition = item["KeyShowMessagePosition"],
                        OrderInfoMessage = item["OrderInfoMessage"],
                        WorkPlaceType = item["WorkPlaceTyp"],

                        PictureFile = item["PictureFile"],
                        ButtonsConfiguration = buttons

                    });

                }

                if (InfoTextlist.Count > 0)
                {
                    File.WriteAllText(dataPath, JsonConvert.SerializeObject(InfoTextlist));

                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n {ex.Source}", "Exection occured DB reading", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;

        }

        public bool IsElementsEquals(InfoTextModel oldState, InfoTextModel newState)
        {

            bool isEquals = false;

            if (oldState != null && newState != null)
            {
               
                string oldInfo = JsonConvert.SerializeObject(oldState);
                string newInfo = JsonConvert.SerializeObject(newState);

                if (string.Equals(oldInfo, newInfo))
                {
                    isEquals = true;
                }
                else
                {
                    isEquals = false;
                }

            }

            return isEquals;
        }


        public List<string> GetLanguageList()
        {
            return Language;
        }

        public List<T1t001EpworkplaceTypName> GeWorkPlaceList()
        {
            return WorkPlaceList;
        }
    }
}
