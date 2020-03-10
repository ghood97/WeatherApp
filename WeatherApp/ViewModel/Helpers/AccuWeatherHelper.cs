﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.ViewModel.Helpers
{
    public class AccuWeatherHelper
    {
        public const string BASE_URL = "http://dataservice.accuweather.com";
        public const string AUTOCOMPLETE_ENDPOINT = "/locations/v1/cities/autocomplete?apikey={0}&q={1}";
        public const string CURRENT_CONDITIONS_ENDPOINT = "/currentconditions/v1/{0}?apikey={1}";
        public const string API_KEY = "e0fGQ8irE3Wl3iMgNIcpxDnHNQi1c19y";

        public static async Task<List<Location>> GetLocations(string query)
        {
            List<Location> locations = new List<Location>();

            string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY, query);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                locations = JsonConvert.DeserializeObject<List<Location>>(json);
            }

            return locations;
        }

        public static async Task<CurrentCondition> GetCurrentCondition(string locationKey)
        {
            CurrentCondition currentCondition = new CurrentCondition();

            string url = BASE_URL + string.Format(CURRENT_CONDITIONS_ENDPOINT, locationKey, API_KEY);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                currentCondition = (JsonConvert.DeserializeObject<List<CurrentCondition>>(json)).FirstOrDefault();
            }

            return currentCondition;
        }
    }
}
