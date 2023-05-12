using EditJsonFInspection.DBContext;
using EditJsonFInspection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EditJsonFInspection.Controllers
{
    public class DbReader : IDbReader
    {


        public (string, string) GetNameAndTypeOfScale(string hierarchyId)
        {
            try
            {
                T1v001EperzeugnisPlanGruppe entry;
                using (T1d003produktionContext context = new T1d003produktionContext())
                {
                    entry = context.T1v001EperzeugnisPlanGruppes.Where(item => item.ErzeugnisGruppe == hierarchyId).FirstOrDefault();
                }
               
                return (entry.NameWaagenBasisKlasse, entry.NameWaagenTyp);
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n {ex.Source}", "Exection occured DB reading",MessageBoxButton.OK,MessageBoxImage.Error);
                return ("", "");

            }

        }


        public List<T1t001EpworkplaceTypName> GetAllNameOfWorkPlace()
        {
            try
            {
                dynamic entry;
                List<T1t001EpworkplaceTypName> result = new List<T1t001EpworkplaceTypName>();

                using (T1d003produktionContext context = new T1d003produktionContext())
                {
                    entry = context.T1t001EpworkplaceTypNames ;
                    foreach (var item in entry)
                    {
                        result.Add(item);
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n {ex.Source}", "Exection occured DB reading", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;

            }
        }

        public string GetNameOfWorkPlace(int workPlaceIdent)
        {
            try
            {
                T1t001EpworkplaceTypName entry;
                using (T1d003produktionContext context = new T1d003produktionContext())
                {
                    entry = context.T1t001EpworkplaceTypNames.SingleOrDefault(item => item.Ident == workPlaceIdent);
                }

                return entry.WorkplaceTypName;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n {ex.Source}", "Exection occured DB reading", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";

            }
        }

       
    }
}
