using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DailymotionXamarinAndroidSdk
{
    public class DailyMotionFrameLayout : FrameLayout
    {
        private readonly DailyMotionWebVideoView _dailyMotionWebVideoView;

        #region Constructors

        public DailyMotionFrameLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public DailyMotionFrameLayout(Context context)
            : base(context)
        {
        }

        public DailyMotionFrameLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public DailyMotionFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public DailyMotionFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public DailyMotionFrameLayout(DailyMotionWebVideoView dailyMotionWebVideoView, Context context)
            : base(context)
        {
            _dailyMotionWebVideoView = dailyMotionWebVideoView;
        }

        #endregion

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (e.KeyCode == Keycode.Back && e.Action == KeyEventActions.Up)
            {
                _dailyMotionWebVideoView.HideVideoView();

                return true;
            }

            return base.DispatchKeyEvent(e);
        }
    }
}