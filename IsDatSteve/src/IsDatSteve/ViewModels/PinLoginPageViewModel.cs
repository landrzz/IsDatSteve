using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FormsToolkit;
using IsDatSteve.Views.ContentViews;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using PropertyChanged;
using Xamarin.Forms;

namespace IsDatSteve.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class PinLoginPageViewModel : ViewModelBase
    {
        private IUserDialogs _userDialogs { get; }

        public string PinNum1 { get; set; } = "fa-circle-o";
        public string PinNum2 { get; set; } = "fa-circle-o";
        public string PinNum3 { get; set; } = "fa-circle-o";
        public string PinNum4 { get; set; } = "fa-circle-o";
        public string CancelDeleteText { get; set; } = "Cancel";
        public string MostRecentlyFocusedNumber { get; set; } = string.Empty;
        public string CodeBuilder { get; set; }
        public Color PinColor { get; set; } = new Color();

        public PinLoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
                                 IDeviceService deviceService, IUserDialogs userDialogs)
            : base(navigationService, pageDialogService, deviceService)
        {
            _userDialogs = userDialogs;
            PinColor = Color.FromHex("#1D294A");   //dark blue
        }


        public int ClickCounter = 0;
        public ICommand PinButtonTappedCommand
        {
            get
            {
                return new Command(async (num) =>
                {
                    try
                    {
                        if (num != null)
                        {
                            var strNum = num as string;
                            var intNum = int.Parse(strNum);
                            MostRecentlyFocusedNumber = strNum;

                            ClickCounter++;
                            if (ClickCounter == 1)
                            {
                                PinColor = Color.FromHex("#4b636e");
                                PinNum1 = "fa-circle";
                                CancelDeleteText = "Delete";
                                CodeBuilder = MostRecentlyFocusedNumber;
                            }
                            else if (ClickCounter == 2)
                            {
                                PinNum1 = "fa-circle";
                                PinNum2 = "fa-circle";
                                CancelDeleteText = "Delete";
                                CodeBuilder = $"{CodeBuilder}{num}";
                            }
                            else if (ClickCounter == 3)
                            {
                                PinNum3 = "fa-circle";
                                CancelDeleteText = "Delete";
                                CodeBuilder = $"{CodeBuilder}{num}";
                            }
                            else if (ClickCounter == 4)
                            {
                                PinNum4 = "fa-circle";
                                CancelDeleteText = "Delete";
                                CodeBuilder = $"{CodeBuilder}{num}";
                                await Task.Delay(150);
                                await CheckForCorrectCode();
                            }
                            else if (ClickCounter > 4)
                            {
                                Debug.WriteLine("Something's wrong. How did I get here?");
                            }

                            Debug.WriteLine($"Current Code Combo: {CodeBuilder}");
                        }
                    } catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                });
            }
        }

        public ICommand CancelDeleteButtonTappedCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        if (CancelDeleteText == "Cancel")
                        {
                            _navigationService.GoBackAsync(null, false, true);
                            return;
                        }

                        if (ClickCounter == 0)
                        {
                            CancelDeleteText = "Cancel";   
                        }
                        else if (ClickCounter == 1)
                        {
                            PinNum1 = "fa-circle-o";
                            CancelDeleteText = "Cancel";

                            CodeBuilder = string.Empty;
                        }
                        else if (ClickCounter == 2)
                        {
                            PinNum2 = "fa-circle-o";
                            CancelDeleteText = "Delete";
                            CodeBuilder = new string(CodeBuilder.Take(1).ToArray());
                        }
                        else if (ClickCounter == 3)
                        {
                            PinNum3 = "fa-circle-o";
                            CancelDeleteText = "Delete";
                            CodeBuilder = new string(CodeBuilder.Take(2).ToArray());
                        }
                        else if (ClickCounter == 4)
                        {
                            PinNum4 = "fa-circle-o";
                            CancelDeleteText = "Delete";
                        }
                        else if (ClickCounter > 4)
                        {
                            Debug.WriteLine("How did I get here?");
                        }
                        ClickCounter--;

                        Debug.WriteLine($"Current Code Combo: {CodeBuilder}");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                });
            }
        }

        public async Task CheckForCorrectCode()
        {
            Debug.WriteLine($"Current Code Combo: {CodeBuilder}");

            var correctCode = "1234";
            if (CodeBuilder == correctCode)
            {
                Helpers.HapticsHelper.VibrateFail();
                _userDialogs.Alert("Code Was Correct. Yay.");
                //Navigate Here

            } else 
            {
                Helpers.HapticsHelper.VibrateFail();

                PinColor = Color.FromHex("#ae392e");   //red
                CancelDeleteText = "Cancel";
                MessagingService.Current.SendMessage("CodeInvalid");

                ClickCounter = 0;
                CodeBuilder = string.Empty;
                await Task.Delay(30);
                PinNum4 = "fa-circle-o";
                await Task.Delay(30);
                PinNum3 = "fa-circle-o";
                await Task.Delay(30);
                PinNum2 = "fa-circle-o";
                await Task.Delay(30);
                PinNum1 = "fa-circle-o";
            }
        }


        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            // TODO: Implement your initialization logic
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            // TODO: Handle any final tasks before you navigate away
        }

    }
}
