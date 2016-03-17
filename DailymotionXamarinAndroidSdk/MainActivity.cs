using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DailymotionXamarinAndroidSdk
{
    [Activity(Label = "DailymotionXamarinAndroidSdk", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private DailyMotionWebVideoView _dmView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _dmView = this.FindViewById<DailyMotionWebVideoView>(Resource.Id.dmWebVideoView);
            _dmView.SetVideoId("x10iisk");
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            _dmView.HandleBackPress(this);
        }
    }
}

