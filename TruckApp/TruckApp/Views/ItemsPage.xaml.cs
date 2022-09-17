using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckApp.Models;
using TruckApp.ViewModels;
using TruckApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckApp.Views
{
    public partial class ItemsPage : ContentPage
    {

        public ItemsPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            TruckDetailsDatabase database = await TruckDetailsDatabase.Instance;
            lstTruckData.ItemsSource = await database.GetItemsAsync();
        }
      
    }
}