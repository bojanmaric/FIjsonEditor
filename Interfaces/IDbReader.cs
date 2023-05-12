using EditJsonFInspection.DBContext;
using System.Collections.Generic;
namespace EditJsonFInspection.Interfaces
{
    public interface IDbReader
    {
        (string, string) GetNameAndTypeOfScale(string hierarchyId);
        string GetNameOfWorkPlace(int workPlaceIdent);
        List<T1t001EpworkplaceTypName> GetAllNameOfWorkPlace();

    }
}
