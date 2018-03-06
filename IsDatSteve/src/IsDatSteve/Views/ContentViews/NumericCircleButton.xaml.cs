using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace IsDatSteve.Views.ContentViews
{
    public partial class NumericCircleButton : ContentView
    {
        public NumericCircleButton()
        {
            InitializeComponent();
        }


        public static Xamarin.Forms.Color defaultColor = Color.FromHex("#000000");
        public static string defaultLetters = string.Empty;
        public static string defaultNumber = string.Empty;

        public static readonly BindableProperty DigitTextProperty = BindableProperty.Create(nameof(DigitText), typeof(string), typeof(NumericPinButton), defaultNumber);
        public string DigitText
        {
            get
            {
                return (string)GetValue(DigitTextProperty);
            }
            set
            {
                SetValue(DigitTextProperty, value);
            }
        }

        public static readonly BindableProperty DigitLettersProperty = BindableProperty.Create(nameof(DigitLetters), typeof(string), typeof(NumericPinButton), defaultLetters);
        public string DigitLetters
        {
            get
            {
                return (string)GetValue(DigitLettersProperty);
            }
            set
            {
                SetValue(DigitLettersProperty, value);
            }
        }

        public static readonly BindableProperty DigitColorProperty = BindableProperty.Create(nameof(DigitColor), typeof(Color), typeof(NumericCircleButton), defaultColor);
        public Color DigitColor
        {
            get
            {
                return (Color)GetValue(DigitColorProperty);
            }
            set
            {
                SetValue(DigitColorProperty, value);
            }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(NumericCircleButton), defaultColor);
        public Color BorderColor
        {
            get
            {
                return (Color)GetValue(BorderColorProperty);
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        //public static readonly BindableProperty MyCommandProperty = BindableProperty.Create(nameof(MyCommand), typeof(ICommand), typeof(NumericCircleButton));
        //public ICommand MyCommand
        //{
        //    get
        //    {
        //        return (ICommand)GetValue(MyCommandProperty);
        //    }
        //    set
        //    {
        //        SetValue(MyCommandProperty, value);
        //    }
        //}


    }
}
