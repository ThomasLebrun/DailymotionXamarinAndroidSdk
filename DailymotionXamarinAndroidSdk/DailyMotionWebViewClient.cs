using Android.Webkit;

namespace DailymotionXamarinAndroidSdk
{
    public class DailyMotionWebViewClient : WebViewClient
    {
        private readonly DailyMotionWebVideoView _dailyMotionWebVideoView;

        public DailyMotionWebViewClient(DailyMotionWebVideoView dailyMotionWebVideoView)
        {
            _dailyMotionWebVideoView = dailyMotionWebVideoView;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            var uri = Android.Net.Uri.Parse(url);
            if (uri.Scheme.Equals("dmevent"))
            {
                var @event = uri.GetQueryParameter("event");
                if (@event.Equals("apiready"))
                {
                    if (_dailyMotionWebVideoView.AutoPlay)
                    {
                        _dailyMotionWebVideoView.CallPlayerMethod("play");
                    }
                }
                return true;
            }

            return base.ShouldOverrideUrlLoading(view, url);
        }
    }
}