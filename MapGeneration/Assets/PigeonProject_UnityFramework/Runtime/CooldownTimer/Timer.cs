using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    public delegate void TimerEffect();
    public class Timer
    {
        /// <summary>
        /// Initial duration of the timer in seconds.
        /// Restarting sets the timer to this value.
        /// </summary>
        public float Duration { get; private set; } = -1f;
        /// <summary>
        /// The remaining duration of a currently running timer in seconds.
        /// </summary>
        public float RemainingTime { get; private set; } = -1f;
        /// <summary>
        /// The remaining duration relative to the initial duration.
        /// Is -1 if the timer has terminatet or initial duration is not set.
        /// </summary>
        public float RemainingTimeRelative
        {
            get
            {
                if (Duration < 0 || RemainingTime < 0)
                    return -1f;
                return RemainingTime / Duration;
            }
        }
        /// <summary>
        /// True while remaining time is greater than zero regardless of whether the timer  is active.
        /// </summary>
        public bool IsRunning { get => RemainingTime > 0; }
        /// <summary>
        /// Whether the timer should currently be updated.
        /// </summary>
        public bool IsActive { get; set; } = false;
        /// <summary>
        /// Whether the timer should invoke the delegated effects when terminating.
        /// </summary>
        public bool InvokeTimerEffect { get; set; } = true;
        
        
        TimerEffect _timerEffect;


        /// <summary>
        /// Create a timer for cooldowns or delayed effects.
        /// </summary>
        /// <remarks>
        /// You should not directly instantiate a timer.
        /// Instead use TimerSystem.RegisterTimer().
        /// </remarks>
        public Timer(float duration = -1f, bool startImmediately = false)
        {
            Duration = duration;
            if (startImmediately)
                Restart();
        }


        /// <summary>
        /// Called by the TimerSystem.
        /// Advances the timer state.
        /// </summary>
        /// <remarks>
        /// You should not call this yourself.
        /// </remarks>
        public void Update()
        {
            if (IsActive)
            {
                RemainingTime -= Time.deltaTime;
                if (RemainingTime <= 0)
                {
                    Terminate(InvokeTimerEffect);
                }
            }
        }


        /// <summary>
        /// Resets remaining time to initial duration and sets the timer active.
        /// </summary>
        public void Restart()
        {
            RemainingTime = Duration;
            IsActive = true;
        }
        /// <summary>
        /// Restarts the timer with a specific duration regardless of initial duration.
        /// </summary>
        /// <param name="val">Duration for this activation.</param>
        public void Restart(float val)
        {
            if (val > 0)
            {
                RemainingTime = val;
                IsActive = true;
            }
        }

        /// <summary>
        /// Sets remaining time to val if val is greater than currently remaining time.
        /// </summary>
        public void SetTime(float val)
        {
            if (val > RemainingTime)
                RemainingTime = val;
        }

        /// <summary>
        /// Adds val to remaining time.
        /// </summary>
        public void AddTime(float val)
        {
            RemainingTime += val;
        }

        /// <summary>
        /// Sets remaining time to -1 and marks the timer inactive.
        /// </summary>
        public void Terminate(bool invokeTimerEffect = true)
        {
            RemainingTime = -1f;
            if (invokeTimerEffect)
                _timerEffect?.Invoke();
            IsActive = false;
        }

        /// <summary>
        /// Register a delegate to be called when the timer terminates.
        /// </summary>
        /// <param name="function">The function to be called when the timer terminates.</param>
        public void AddTimerEffect(TimerEffect function)
        {
            _timerEffect += function;
        }

        /// <summary>
        /// Deregister a function bound to the timer.
        /// </summary>
        /// <param name="function">The function to be removed from the delegate.</param>
        public void RemoveTimerEffect(TimerEffect function)
        {
            _timerEffect -= function;
        }
    }
}
