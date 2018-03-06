using System;
using System.Linq;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Plugin.Media;
using System.Diagnostics;
using System.IO;
using Acr.UserDialogs;
using IsDatSteve.Interfaces;
using Plugin.Media.Abstractions;
using PropertyChanged;
using System.Windows.Input;

namespace IsDatSteve.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel : ViewModelBase
    {
        Plugin.Media.Abstractions.MediaFile file;
        public bool SpinnerOn { get; set; }
        public bool imageShowing { get; set; } = false;
        public ImageSource setImage { get; set; }
        public Stream selectedImage { get; set; }
        public DelegateCommand TakePhotoCommand { get; }
        public DelegateCommand OpenPhotoCommand { get; }
        public DelegateCommand HowYaWantPiczCommand { get; }

        //WouldBruleLikeItCommand
        private IUserDialogs _userDialogs { get; }
        public string BrulesFinalWord { get; set; }

        const string newPhoto = "Take New Photo";
        const string selectPhoto = "Select from Camera Roll";

       
        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
                                 IDeviceService deviceService, IUserDialogs userDialogs)
            : base(navigationService, pageDialogService, deviceService)
        {
            Title = "Brule Approved";
            _userDialogs = userDialogs;
            TakePhotoCommand = new DelegateCommand(OnTakePhotoCommandExecuted);
            OpenPhotoCommand = new DelegateCommand(OnOpenPhotoCommandExecuted);
            HowYaWantPiczCommand = new DelegateCommand(OnHowYaWantPiczCommandExecuted);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            // TODO: Implement your initialization logic
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            // TODO: Handle any final tasks before you navigate away
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            switch (parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    // TODO: Handle any tasks that should occur only when navigated back to
                    break;
                case NavigationMode.New:
                    // TODO: Handle any tasks that should occur only when navigated to for the first time
                    break;
            }

            // TODO: Handle any tasks that should be done every time OnNavigatedTo is triggered
        }

        public override void Destroy()
        {
            // TODO: Dispose of any objects you need to for memory management
        }

        private async void OnHowYaWantPiczCommandExecuted()
        {
            try
            {
                string[] btnz = { newPhoto, selectPhoto };
                var choice = await _userDialogs.ActionSheetAsync("Got any broat pics?", "nah", null, null, btnz);
                if (choice == newPhoto)
                {
                    TakePhotoCommand.Execute();
                }
                else if (choice == selectPhoto)
                {
                    OpenPhotoCommand.Execute();
                } 
            } catch (Exception ex)
            {
                Debug.Write(ex.Message + "  " + ex.InnerException);
            }
        }

        public async void OnTakePhotoCommandExecuted()
        {
            var result = await CheckOnCameraPermissions();
            if (result)
            {
                await OpenPhotoTaker();
            } else
            {
                Debug.WriteLine("Apparently Access was Denied");
            }
        }

        public async void OnOpenPhotoCommandExecuted()
        {
            var result = await CheckOnPhotoRollPermissions();
            if (result)
            {
                await OpenPhotoSelecter();
            } else 
            {
                Debug.WriteLine("Apparently Access was Denied");
            }
        }

        public async Task<bool> CheckOnCameraPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != PermissionStatus.Granted)                                   // || storageStatus != PermissionStatus.Granted
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera});   //, Permission.Storage 
                cameraStatus = results[Permission.Camera];
            }

            if (cameraStatus == PermissionStatus.Granted)
            {
                return true;
            }
            else
            { return false; }
        }

        public async Task<bool> CheckOnPhotoRollPermissions()
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)                                   // || storageStatus != PermissionStatus.Granted
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] {Permission.Storage });   //, Permission.Storage 
                storageStatus = results[Permission.Storage];
            }

            if (storageStatus == PermissionStatus.Granted)
            {
                return true;
            }
            else
            { return false; }
        }

        public async Task OpenPhotoTaker()
        {
            try
            {
                ActivateSpinner();

                var rnd = new Random();
                var rndEnding = rnd.Next(100, 9999);
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await _userDialogs.AlertAsync("It appears that no camera is available", "No Camera", "OK");
                    return;
                }
                var myfile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,                  //resizes the photo to 50% of the original
                    CompressionQuality = 92,                                                // Int from 0 to 100 to determine image compression level
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,           // determine which camera to default to
                    Directory = "CC Directors",
                    Name = $"mybrulepic{rndEnding}",
                    SaveToAlbum = true,                                                     // this saves the photo to the camera roll 
                                                                                            //if we need the public album path --> var aPpath = file.AlbumPath;
                                                                                           //if we need a private path --> var path = file.Path;
                    AllowCropping = false,
                });
                file = myfile;
                if (file == null)
                {
                    DeactivateSpinner();
                    return;
                }

                Debug.WriteLine("File Location: " + file.Path + "     <--- here");
                setImage = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    var path = file.Path;
                    var pathprivate = file.AlbumPath;
                    Debug.WriteLine(path.ToString());
                    Debug.WriteLine(pathprivate.ToString());
                    selectedImage = file.GetStream();

                    return stream;
                });
                imageShowing = true;
                RaisePropertyChanged();

                WouldBruleLikeIt(file);
                DeactivateSpinner();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DeactivateSpinner();
            }
        }




        public async Task OpenPhotoSelecter()
        {
            try
            {
                ActivateSpinner();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await _userDialogs.AlertAsync("Permission was not granted to access camera roll or picking photos is not available on this device.", "Cannot Pick Photo", "OK");
                    return;
                }
                var myfile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 92,
                });
                file = myfile;
                if (file == null)
                {
                    DeactivateSpinner();
                    return;
                }
                string filePath = file.Path;
                string fileType = filePath.Substring(filePath.Length - 4);
                Debug.WriteLine($"****    {fileType}   *****");
                if (fileType == ".jpg" || fileType == ".png" || fileType == ".JPG" || fileType == ".PNG")
                {
                    setImage = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        var path = file.Path;
                        var pathprivate = file.AlbumPath;
                        Debug.WriteLine(path);
                        Debug.WriteLine(pathprivate);
                        selectedImage = file.GetStream();

                        //file.Dispose();
                        return stream;
                    });
                    imageShowing = true;
                    RaisePropertyChanged();

                    WouldBruleLikeIt(file);
                    DeactivateSpinner();

                }
                else
                {
                    await _userDialogs.AlertAsync("Unsupported file type.", "Error", "OK");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DeactivateSpinner();
            }
        }



        public void WouldBruleLikeIt(MediaFile file)
        {
            var wouldHeLike = Xamarin.Forms.DependencyService.Get<IBruleDetector>();
            var hmm = wouldHeLike.GetBrulesThoughts(file);
            var bestTag = hmm.Item1;
            var confidenceLevel = hmm.Item2;

            Debug.WriteLine(hmm.Item1);
            Debug.WriteLine(hmm.Item2);
            if (hmm.Item2.StartsWith("0", StringComparison.CurrentCulture))
            {
                bestTag = "i mean, i have no idea what this one is. i didn't ask for this";
                BrulesFinalWord = $"looks alike uhh {bestTag}.\n maybe i'm abouta {confidenceLevel} sure.";

            } else 
            {
                BrulesFinalWord = $"looks alike uhh {bestTag} to me.\n maybe i'm abouta {confidenceLevel} sure.";

            }


        }

        public ICommand TestPinCodeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigationService.NavigateAsync("PinLoginPage", null, false, true);
                });
            }
        }



        public void ActivateSpinner()
        {
            SpinnerOn = true;
            RaisePropertyChanged();
        }

        public void DeactivateSpinner()
        {
            SpinnerOn = false;
            RaisePropertyChanged();
        }

    }
}
