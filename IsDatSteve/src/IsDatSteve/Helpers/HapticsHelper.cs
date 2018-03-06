using System;
using IsDatSteve;
using IsDatSteve.Interfaces;
using Xamarin.Forms;

namespace IsDatSteve.Helpers
{
    public static class HapticsHelper
    {
        public static void VibrateSuccess()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IHapticEffect>().HapticSuccess();
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //if (CrossVibrate.Current.CanVibrate)
                    //CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(300));
            }
        }

        public static void VibrateFail()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IHapticEffect>().HapticError();
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //if (CrossVibrate.Current.CanVibrate)
                    //CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(500));
            }
        }

        public static void VibrateWarning()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IHapticEffect>().HapticWarning();
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                //if (CrossVibrate.Current.CanVibrate)
                    //CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(700));
            }
        }


        public static void VibrateHapticThud()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IHapticEffect>().HapticThud();
            }
        }

        public static void VibrateHaptic()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IHapticEffect>().HapticSelection();
            }
        }
    }
}
