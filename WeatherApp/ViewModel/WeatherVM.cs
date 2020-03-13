using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Location selectedLocation;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedLocation = new Location
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
            Locations = new ObservableCollection<Location>();
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

        public Location SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;
                if (selectedLocation != null)
                {
                    OnPropertyChanged("SelectedLocation");
                    GetCurrentCondition();
                }
            }
        }

        public ObservableCollection<Location> Locations { get; set; }

        private async void GetCurrentCondition()
        {
            Query = string.Empty;
            CurrentCondition = await AccuWeatherHelper.GetCurrentCondition(SelectedLocation.Key);
            Locations.Clear();
        }

        public async void MakeQuery()
        {
            // get list of locations from search criteria
            var locations = await AccuWeatherHelper.GetLocations(Query);
            // clear current locations list because setting it to a new ObservableCollection
            // will break the binding
            Locations.Clear();
            foreach(Location location in locations)
            {
                // add each location from query to Locations list
                Locations.Add(location);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
