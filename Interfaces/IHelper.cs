using EditJsonFInspection.DBContext;
using EditJsonFInspection.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EditJsonFInspection.Interfaces
{
    public interface IHelper
    {
        Task<bool> AddNewEntryIntoFile(InfoTextModel infoEntry);

        Task<bool> SaveFile();

        Task<bool> UpdateEntryIntoFile(InfoTextModel oldEntry, InfoTextModel newEntry);

        Task<bool> DeleteItem(InfoTextModel item);
        ObservableCollection<InfoTextModel> GetAll();
        bool ConvertOldDataToNew();

        List<string> GetLanguageList();
        List<T1t001EpworkplaceTypName> GeWorkPlaceList();

        bool IsElementsEquals(InfoTextModel oldState, InfoTextModel newState);




    }
}
