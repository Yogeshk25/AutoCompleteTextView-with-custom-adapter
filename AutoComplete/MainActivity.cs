using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace AutoComplete
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            List<Person> items = new List<Person>
            {
                new Person { Name = "Yogesh", Email = "yogesh@test.com" },
                new Person { Name = "Test1", Email = "test1@test.com" },
                new Person { Name = "Test2", Email = "test2@test.com" },
                new Person { Name = "Yogendra", Email = "yogesndra@test.com" }
            };

            AutoCompleteTextView textView = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteTextView);
            textView.Threshold = 1;
            textView.Adapter = new PersonAdapter(this, items);
            textView.ItemSelected += (sender, e) =>
            {
                Toast.MakeText(this, $"{e.Position} selected", ToastLength.Long).Show();
            };
        }
    }
}

