using System;
using System.Windows.Forms;

namespace ATG_Notifier
{
    public class FadeTimer1 : Timer
    {
        //private int steps = 100;
        private double currentStep;

        private int fadeDelay;

        private int fadeOpInterval = 55; // The timer interval in milliseconds
        private double granularity;

        private readonly static int TIMER_BUSY = -1;

        private Form targetForm;

        private bool fadedOut = false;
        private bool started = false;

        public enum FADE_TIMER_STATUS
        {
            TIMER_BUSY  = -1,
            TIMER_OK    = 0
        }

        public enum CANCEL_FADE_TIMER_STATUS
        {
            
        }

        public FadeTimer1(Form target)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"> The form the timer operates on. </param>
        /// <param name="delay"> The delay time for the the fade-operation. In milliseconds. </param>
        /// <param name="duration">Determines the duration of the fade-operation. In milliseconds. </param>
        public FadeTimer1(Form target, int delay, int duration)
        {
            targetForm = target;
            this.fadeDelay = delay;
            this.Interval = delay;
            granularity = 1 / (duration / (double)fadeOpInterval);

#if DEBUG
            this.Tag = "Debug";
#endif
            //currentStep = granularity;
        }

        public FADE_TIMER_STATUS FadeIn()
        {
            if (this.started)
            {
                return FADE_TIMER_STATUS.TIMER_BUSY;
            }

            if (fadedOut)
            {
                this.Tick -= _doFadeOut;
                //targetForm.Show();
            }

            if (!targetForm.Visible)
            {
                targetForm.Show();
            }

            this.Tick -= _doFadeIn;

            //currentStep = steps;     
            this.Tick += _doFadeIn;
            fadedOut = false;
            this.Start();

            return FADE_TIMER_STATUS.TIMER_OK;
        }

        public FADE_TIMER_STATUS FadeOutStart()
        {
            if (this.started)
            {
                return FADE_TIMER_STATUS.TIMER_BUSY;
            }

            this.Tick -= _doFadeOut;

            this.Tick -= FadeOut;
            this.Tick += FadeOut;

            this.Interval = fadeDelay;
            this.Start();

            return FADE_TIMER_STATUS.TIMER_OK;
        }

        public void FadeOut(object sender, EventArgs e)
        {
            if (this.started)
            {
                //return FADE_TIMER_STATUS.TIMER_BUSY;
            }

            this.Stop();

            this.Tick -= FadeOut;

            if (!fadedOut)
            {
                this.Tick -= _doFadeIn;
            }

            this.Tick -= _doFadeOut;

            //currentStep = steps;

            this.Tick += _doFadeOut;
            this.Interval = fadeOpInterval;

            fadedOut = true;
            this.Start();

            //return FADE_TIMER_STATUS.TIMER_OK;
        }

        public void CancelFadeOut()
        {
            if (!fadedOut)
            {

            }

            this.Stop();

            if (targetForm.Opacity != 1)
            {
                targetForm.Opacity = 1;
            }
        }

        public void CancelFadeIn()
        {
            if (fadedOut)
            {

            }

            this.Stop();
            targetForm.Opacity = 0;
            targetForm.Close();
        }

        private void _doFadeOut(object sender, EventArgs e)
        {
            targetForm.Opacity -= granularity;

            //targetForm.Opacity = currentStep / steps;
            //currentStep--;

            if (targetForm.Opacity == 0)
            {
                targetForm.Close();
                targetForm.Dispose();
                this.Stop();
                this.Dispose();
            }
        }

        private void _doFadeIn(object sender, EventArgs e)
        {
            targetForm.Opacity += granularity;
            //currentStep++;

            if (targetForm.Opacity == 1)
            {
                this.Stop();
                this.Dispose();
            }
        }
    }
}
