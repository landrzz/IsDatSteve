using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IsDatSteve.TriggerActions
{
    public class PinClickedTriggerAction : TriggerAction<Button> 
    {
        private Color PrimaryDark = Color.FromHex("#4b636e");

        protected override async void Invoke(Button sender)
        {
            sender.BackgroundColor = Color.LightGray;
            await Task.Delay(150);
            sender.BackgroundColor = Color.Transparent;
        }
    }
}
