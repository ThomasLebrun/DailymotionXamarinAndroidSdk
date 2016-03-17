using Android.App;
using Android.Content;
using Android.Media;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace DailymotionXamarinAndroidSdk
{
    public class DailyMotionWebChromeClient : WebChromeClient, MediaPlayer.IOnCompletionListener
    {
        private readonly DailyMotionWebVideoView _dailyMotionWebVideoView;
        private readonly Context _context;

        public DailyMotionWebChromeClient(DailyMotionWebVideoView dailyMotionWebVideoView, Context context)
        {
            _dailyMotionWebVideoView = dailyMotionWebVideoView;
            _context = context;
        }

        public override View VideoLoadingProgressView
        {
            get
            {
                var pb = new ProgressBar(_context);
                pb.Indeterminate = true;

                return pb;
            }
        }

        public override void OnShowCustomView(View view, ICustomViewCallback callback)
        {
            base.OnShowCustomView(view, callback);

            ((Activity)_context).VolumeControlStream = Stream.Music;

            _dailyMotionWebVideoView.IsFullScreen = true;
            _dailyMotionWebVideoView.ViewCallback = callback;

            var layout = view as FrameLayout;
            if (layout != null)
            {
                var frame = layout;
                var child = frame.FocusedChild as VideoView;
                if (child != null)
                {
                    //We are in 2.3
                    var video = child;
                    frame.RemoveView(video);

                    _dailyMotionWebVideoView.SetupVideoLayout(video);

                    _dailyMotionWebVideoView.CustomVideoView = video;
                    _dailyMotionWebVideoView.CustomVideoView.SetOnCompletionListener(this);
                }
                else
                {
                    //Handle 4.x
                    _dailyMotionWebVideoView.SetupVideoLayout(view);
                }
            }
        }

        public override void OnHideCustomView()
        {
            base.OnHideCustomView();

            _dailyMotionWebVideoView.HideVideoView();
        }

        public void OnCompletion(MediaPlayer mp)
        {
            _dailyMotionWebVideoView.HideVideoView();
        }
    }
}