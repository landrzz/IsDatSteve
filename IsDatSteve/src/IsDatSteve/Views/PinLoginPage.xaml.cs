using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormsToolkit;
using IsDatSteve.Helpers;
using IsDatSteve.ViewModels;
using Plugin.Iconize;
using Xamanimation;
using Xamarin.Forms;

namespace IsDatSteve.Views
{
    public partial class PinLoginPage : ContentPage
    {
        PinLoginPageViewModel binding => BindingContext as PinLoginPageViewModel;

        public PinLoginPage()
        {
            InitializeComponent();

            MessagingService.Current.Subscribe("CodeInvalid", (e) =>
            {
                NumberGrid.InputTransparent = true;
                NumberGrid.IsEnabled = false;
                var shake = new ShakeAnimation();
                shake.Duration = "200";
                shake.Easing = EasingType.SinInOut;
                CodeStack.Animate(shake);
                NumberGrid.InputTransparent = false;
                NumberGrid.IsEnabled = true;
            });
        }

        async void PinNum1(object sender, EventArgs e)
        {
            await ClickHappened("1", btn1, lbl1, lblLetters1);
        }

        async void PinNum2(object sender, EventArgs e)
        {
            await ClickHappened("2", btn2, lbl2, lblLetters2);
        }

        async void PinNum3(object sender, EventArgs e)
        {
            await ClickHappened("3", btn3, lbl3, lblLetters3);
        }

        async void PinNum4(object sender, EventArgs e)
        {
            await ClickHappened("4", btn4, lbl4, lblLetters4);
        }

        async void PinNum5(object sender, EventArgs e)
        {
            await ClickHappened("5", btn5, lbl5, lblLetters5);
        }

        async void PinNum6(object sender, EventArgs e)
        {
            await ClickHappened("6", btn6, lbl6, lblLetters6);
        }

        async void PinNum7(object sender, EventArgs e)
        {
            await ClickHappened("7", btn7, lbl7, lblLetters7);
        }

        async void PinNum8(object sender, EventArgs e)
        {
            await ClickHappened("8", btn8, lbl8, lblLetters8);
        }

        async void PinNum9(object sender, EventArgs e)
        {
            await ClickHappened("9", btn9, lbl9, lblLetters9);
        }

        async void PinNum0(object sender, EventArgs e)
        {
            await ClickHappened("0", btn0, lbl0, lblLetters0);
        }

        public async Task ClickHappened(string btnNum, Button btn, Label lblNum, Label lblLetters)
        {
            binding.PinButtonTappedCommand.Execute(btnNum);
            btn.BackgroundColor = Color.WhiteSmoke;
            btn.BorderColor = StyleKit.RoyalBlueColor;
            lblNum.TextColor = StyleKit.RoyalBlueColor;
            lblLetters.TextColor = StyleKit.RoyalBlueColor;
            await Task.Delay(130);
            btn.BackgroundColor = Color.Transparent;
            btn.BorderColor = StyleKit.DarkBlueColor;
            lblNum.TextColor = StyleKit.DarkBlueColor;
            lblLetters.TextColor = StyleKit.DarkBlueColor;
        }
    }
}
