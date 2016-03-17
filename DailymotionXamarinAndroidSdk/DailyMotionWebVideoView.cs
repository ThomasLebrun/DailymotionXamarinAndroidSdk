using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace DailymotionXamarinAndroidSdk
{
    public class DailyMotionWebVideoView : WebView
    {
        private WebSettings mWebSettings;
        private WebChromeClient mChromeClient;

        private String mEmbedUrl = "http://www.dailymotion.com/embed/video/{0}?html=1&fullscreen={1}&app={2}&api=location";
        private String mExtraUA = "; DailymotionEmbedSDK 1.0";
        private FrameLayout mVideoLayout;
        private FrameLayout mRootLayout;
        private Boolean mAllowAutomaticNativeFullscreen = false;

        public bool AutoPlay { get; set; }
        public bool IsFullScreen { get; set; }
        public WebChromeClient.ICustomViewCallback ViewCallback { get; set; }
        public VideoView CustomVideoView { get; set; }

        #region Constructors

        public DailyMotionWebVideoView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            this.Initialize();
        }

        public DailyMotionWebVideoView(Context context)
            : base(context)
        {
            this.Initialize();
        }

        public DailyMotionWebVideoView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.Initialize();
        }

        public DailyMotionWebVideoView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            this.Initialize();
        }

        public DailyMotionWebVideoView(Context context, IAttributeSet attrs, int defStyleAttr, bool privateBrowsing)
            : base(context, attrs, defStyleAttr, privateBrowsing)
        {
            this.Initialize();
        }

        public DailyMotionWebVideoView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.Initialize();
        }

        #endregion

        private void Initialize()
        {
            //The topmost layout of the window where the actual VideoView will be added to
            mRootLayout = (FrameLayout)((Activity)this.Context).Window.DecorView;

            mWebSettings = this.Settings;
            mWebSettings.JavaScriptEnabled = true;
            mWebSettings.SetPluginState(WebSettings.PluginState.On);
            mWebSettings.UserAgentString = mWebSettings.UserAgentString + mExtraUA;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
            {
                mWebSettings.MediaPlaybackRequiresUserGesture = true;
            }

            mChromeClient = new DailyMotionWebChromeClient(this, this.Context);

            SetWebChromeClient(mChromeClient);
            SetWebViewClient(new DailyMotionWebViewClient(this));
        }

        public void CallPlayerMethod(string method)
        {
            LoadUrl("javascript:player.api(\"" + method + "\")");
        }

        public void SetVideoId(string videoId)
        {
            LoadUrl(string.Format(mEmbedUrl, videoId, mAllowAutomaticNativeFullscreen, this.Context.PackageName));
        }

        public void SetVideoId(string videoId, bool autoPlay)
        {
            this.AutoPlay = autoPlay;

            LoadUrl(string.Format(mEmbedUrl, videoId, mAllowAutomaticNativeFullscreen, this.Context.PackageName));
        }

        public void HideVideoView()
        {
            if (this.IsFullScreen)
            {
                if (this.CustomVideoView != null)
                {
                    this.CustomVideoView.StopPlayback();
                }

                mRootLayout.RemoveView(mVideoLayout);
                this.ViewCallback.OnCustomViewHidden();

                ((Activity)this.Context).VolumeControlStream = Stream.NotificationDefault;

                this.IsFullScreen = false;
            }
        }

        public void SetupVideoLayout(View video)
        {
            /**
             * As we don't want the touch events to be processed by the underlying WebView, we do not set the WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE flag
             * But then we have to handle directly back press in our View to exit fullscreen.
             * Otherwise the back button will be handled by the topmost Window, id-est the player controller
             */
            mVideoLayout = new DailyMotionFrameLayout(this, this.Context);
            mVideoLayout.SetBackgroundResource(Android.Resource.Color.Black);
            mVideoLayout.AddView(video);

            var lp = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            lp.Gravity = GravityFlags.Center;

            mRootLayout.AddView(mVideoLayout, lp);

            this.IsFullScreen = true;
        }

        public void HandleBackPress(Activity activity)
        {
            if (this.IsFullScreen)
            {
                HideVideoView();
            }
            else
            {
                LoadUrl(string.Empty); //Hack to stop video
                activity.Finish();
            }
        }

        public void SetAllowAutomaticNativeFullscreen(bool allowAutomaticNativeFullscreen)
        {
            mAllowAutomaticNativeFullscreen = allowAutomaticNativeFullscreen;
        }
    }
}