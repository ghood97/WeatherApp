using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;
        private CurrentCondition currentCondition;
        private Location selectedCity;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new Location
                {
                    LocalizedName = "Boston"
                };
                CurrentCondition = new CurrentCondition
                {
                    WeatherText = "Sunny",
                    Temperature = new Temperature
                    {
                        Imperial = new TempSystem
                        {
                            Value = 65
                        }
                    }
                };
            }

            SearchCommand = new SearchCommand(this);
        }

        public SearchCommand SearchCommand { get; set; }

        public CurrentCondition CurrentCondition
        {
            get { return currentCondition; }
            set
            {
                currentCondition = value;
                OnPropertyChanged("CurrentCondition");
            }
        }

        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged("Query");
            }
        }

        public Location SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }

        public async void MakeQuery()
        {
            var locations = await AccuWeatherHelper.GetLocations(Query);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
