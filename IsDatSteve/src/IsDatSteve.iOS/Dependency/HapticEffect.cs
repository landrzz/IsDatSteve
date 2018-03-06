using System;
using Foundation;
using UIKit;
using IsDatSteve.iOS;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using IsDatSteve;
using IsDatSteve.iOS.Dependency;
using IsDatSteve.Interfaces;

[assembly: Dependency(typeof(HapticEffect))]
namespace IsDatSteve.iOS.Dependency
{
    public class HapticEffect : IHapticEffect
    {
        UINotificationFeedbackGenerator hapticNotify = new UINotificationFeedbackGenerator();
        UISelectionFeedbackGenerator hapticSelection = new UISelectionFeedbackGenerator();
        UIImpactFeedbackGenerator hapticImpact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Medium);

        public HapticEffect()
        {
            hapticNotify.Prepare();
            hapticSelection.Prepare();
            hapticImpact.Prepare();
        }

        public void HapticSuccess()
        {
            hapticNotify.NotificationOccurred(UINotificationFeedbackType.Success);
        }

        public void HapticError()
        {
            hapticNotify.NotificationOccurred(UINotificationFeedbackType.Error);
        }

        public void HapticWarning()
        {
            hapticNotify.NotificationOccurred(UINotificationFeedbackType.Warning);
        }

        public void HapticThud()
        {
            hapticImpact.ImpactOccurred();
        }

        public void HapticSelection()
        {
            hapticSelection.SelectionChanged();
        }
    }
}
