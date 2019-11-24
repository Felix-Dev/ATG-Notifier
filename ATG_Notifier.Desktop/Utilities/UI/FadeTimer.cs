using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Utilities.UI
{
    internal class FadeTimer : Timer
    {
        private const int FadeOpInterval = 55; // The timer interval in milliseconds

        //private int steps = 100;
        private double currentStep;

        private int fadeDelay;

        private double granularity;

        private readonly Form targetForm;

        private bool fadedOut = false;
        private bool started = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"> The form the timer operates on. </param>
        /// <param name="delay"> The delay time for the the fade-operation. In milliseconds. </param>
        /// <param name="duration">Determines the duration of the fade-operation. In milliseconds. </param>
        public FadeTimer(Form target, int delay, int duration)
        {
            targetForm = target ?? throw new ArgumentNullException(nameof(target));

            if (delay < 0 || duration < 0)
            {
                throw new ArgumentOutOfRangeException("Delay/Duration for a FadeTimer cannot be negative!");
            }

            this.fadeDelay = delay;
            this.Interval = delay;
            granularity = 1 / (duration / (double)FadeOpInterval);

#if DEBUG
            this.Tag = "Debug";
#endif
            //currentStep = granularity;
        }

        public FadeTimerStatus FadeIn()
        {
            if (this.started)
            {
                return FadeTimerStatus.Busy;
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

            return FadeTimerStatus.Ok;
        }

        public FadeTimerStatus FadeOutStart()
        {
            if (this.started)
            {
                return FadeTimerStatus.Busy;
            }

            this.Tick -= _doFadeOut;

            this.Tick -= FadeOut;
            this.Tick += FadeOut;

            this.Interval = fadeDelay;
            this.Start();

            return FadeTimerStatus.Ok;
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
            this.Interval = FadeOpInterval;

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
