using EditJsonFInspection.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace EditJsonFInspection.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Edit Final Inspection Json File";
        private readonly IDbReader _dbReader;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
        }

    }
}
