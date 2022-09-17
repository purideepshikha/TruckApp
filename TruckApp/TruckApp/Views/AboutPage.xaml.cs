using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckApp.Views
{
    public partial class AboutPage : ContentPage
    {
        TruckDetails truckDetails = new TruckDetails();
        private bool TruckAvailabiltySwitch = false;

        public AboutPage()
        {
            InitializeComponent();
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lblTruckId.Text))
                {
                    await DisplayAlert("Error", "Please enter Valid Truck Number. 3 characters and 3 Numbers", "OK");
                    return;
                }
                if(lblTruckId.Text.Length<6)
                {
                    await DisplayAlert("Error", "Please enter Valid Truck Number. 3 characters and 3 Numbers", "OK");
                    return;
                }
                Regex regexObj = new Regex(@"^[a-zA-Z{3}0-9{3}]*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                
                bool foundMatch = regexObj.IsMatch(lblTruckId.Text);
                if (foundMatch == false)
                {
                    await DisplayAlert("Error", "Please enter Valid Truck Number 3 characters and 3 Numbers", "OK");
                    return;
                }
                if(TruckMaker.SelectedItem==null)
                {
                    await DisplayAlert("Error", "Please Select Make", "OK");
                    return;
                }
                if (TruckPurchaseDatePicker.Date.Date.Year <2000)
                {
                    await DisplayAlert("Error", "Purchase Date should not be year 2000", "OK");
                    return;
                }
                truckDetails.MakerName = TruckMaker.SelectedItem.ToString();
                truckDetails.TruckId = lblTruckId.Text.ToString();
                truckDetails.IsAvailable = TruckAvailabiltySwitch;
                truckDetails.PurchaseDate = TruckPurchaseDatePicker.Date.ToString();
                truckDetails.TruckImage = PhotoPath;
                truckDetails.EditorDelete = "Edit/Delete";
                TruckDetailsDatabase database = await TruckDetailsDatabase.Instance;
                await database.SaveItemAsync(truckDetails);
                await DisplayAlert("Sucess", "Successfully Inserted", "OK");

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                await DisplayAlert("Failure", "Sorry Something Wrong", "OK");
            }
        }

        private void TruckAvailabilty_Toggled(object sender, ToggledEventArgs e)
        {
            if (TruckAvailabiltySwitch)
            {
                TruckAvailabiltySwitch = false;
            }
            else
            {
                TruckAvailabiltySwitch = true;
            }
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (Stream stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            {
                await stream.CopyToAsync(newStream);

            }
            PhotoPath = newFile;
        }
        string PhotoPath = null;
        async Task PickerPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                imgSelected.Source = photo.FullPath;
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        private async void btnImage_Clicked(object sender, EventArgs e)
        {
            await PickerPhotoAsync();

        }
        int restrictCount = 6;

        private void lblTruckId_TextChanged(object sender, TextChangedEventArgs e)
        {

            Entry entry = sender as Entry;
            String val = entry.Text; //Get Current Text

            if (val.Length > restrictCount)//If it is more than your character restriction
            {
                val = val.Remove(val.Length - 1);// Remove Last character 
                entry.Text = val; //Set the Old value
            }

        }
    }
}