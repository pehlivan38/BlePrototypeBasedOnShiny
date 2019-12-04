using BLEPrototype.ViewModels;
using PotentialX.Data.Infrastructure;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLEPrototype.SqliteExercise
{
    public class DataViewModel : BLEViewModel
    {
        private DataService _repo { get; set; }
        public DataViewModel(DataService repo)
        {
            _repo = repo;
        }

        public override void OnNavigatedTo(INavigationParameters parameters) 
        {

            _ = _repo.GetEEGData(1);

        }
    }
}
