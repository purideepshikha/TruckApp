using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckApp.Views
{
    public partial class AboutPage : ContentPage
    {
        TruckDetails truckDetails = new TruckDetails();
        private bool TruckAvailabiltySwitch = true;

        public AboutPage()
        {
            InitializeComponent();
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                truckDetails.MakerName = TruckMaker.SelectedItem.ToString();
                truckDetails.TruckId = lblTruckId.Text.ToString();
                truckDetails.IsAvailable = TruckAvailabiltySwitch;
                truckDetails.PurchaseDate = TruckPurchaseDatePicker.Date.ToString();
                truckDetails.TruckImage = PhotoPath;
                TruckDetailsDatabase database = await TruckDetailsDatabase.Instance;
                await database.SaveItemAsync(truckDetails);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void TruckAvailabilty_Toggled(object sender, ToggledEventArgs e)
        {
            if (TruckAvailabiltySwitch)
            {
                TruckAvailabiltySwitch = false;
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
    }
}