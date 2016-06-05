using System;
using System.Runtime.InteropServices;
using System.Security;

namespace L2dotNET.Utility
{
    /// <summary>
    /// Represents extended hight performance counter class.
    /// </summary>
    public sealed class Stopwatch
    {
        /// <summary>
        /// Native QueryPerformanceCounter function import.
        /// </summary>
        /// <param name="x">Output <paramref name="x"/> value.</param>
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity()]
        private static extern short QueryPerformanceCounter(ref long x);

        /// <summary>
        /// Native QueryPerformanceFrequency function import.
        /// </summary>
        /// <param name="x">Output <paramref name="x"/> value.</param>
        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity()]
        private static extern short QueryPerformanceFrequency(ref long x);

        /// <summary>
        /// <see cref="Stopwatch"/> start time value.
        /// </summary>
        private long StartTime;

        /// <summary>
        /// <see cref="Stopwatch"/> stop time value.
        /// </summary>
        private long StopTime;

        /// <summary>
        /// <see cref="Stopwatch"/> clack frequency value.
        /// </summary>
        private long ClockFrequency;

        /// <summary>
        /// <see cref="Stopwatch"/> calibration time value.
        /// </summary>
        private long CalibrationTime;

        /// <summary>
        /// Initializes new instance of <see cref="Stopwatch"/> object.
        /// </summary>
        public Stopwatch()
        {
            StartTime = 0;
            StopTime = 0;
            ClockFrequency = 0;
            CalibrationTime = 0;
            Calibrate();
        }

        /// <summary>
        /// Resets <see cref="Stopwatch"/> object: sets it start and stop time to zero values.
        /// </summary>
        public void Reset()
        {
            StartTime = 0;
            StopTime = 0;
        }

        /// <summary>
        /// Start <see cref="Stopwatch"/> counter.
        /// </summary>
        public void Start()
        {
            QueryPerformanceCounter(ref StartTime);
        }

        /// <summary>
        /// Stops <see cref="Stopwatch"/> counter.
        /// </summary>
        public void Stop()
        {
            QueryPerformanceCounter(ref StopTime);
        }

        /// <summary>
        /// Gets <see cref="TimeSpan"/> value, elapsed between <see cref="Stopwatch"/> start and stop events.
        /// </summary>
        /// <returns><see cref="TimeSpan"/> value between <see cref="Stopwatch"/> has been started and stopped.</returns>
        public TimeSpan GetElapsedTimeSpan()
        {
            return TimeSpan.FromMilliseconds(InternalGetElapsedTimeMS());
        }

        /// <summary>
        /// Gets time, elapsed between <see cref="Stopwatch"/> start and stop events in microseconds.
        /// </summary>
        /// <returns><see cref="double"/> value in microseconds between <see cref="Stopwatch"/> has been started and stopped.</returns>
        public double GetElapsedTimeInMicroseconds()
        {
            return (((StopTime - StartTime - CalibrationTime) * 1000000.0 / ClockFrequency));
        }

        /// <summary>
        /// Calibrates <see cref="Stopwatch"/> object.
        /// </summary>
        private void Calibrate()
        {
            QueryPerformanceFrequency(ref ClockFrequency);

            for (int i = 0; i < 1000; i++)
            {
                Start();
                Stop();
                CalibrationTime += StopTime - StartTime;
            }

            CalibrationTime /= 1000;
        }

        private double InternalGetElapsedTimeMS()
        {
            return (((StopTime - StartTime - CalibrationTime) * 1000000.0 / ClockFrequency) / 1000.0);
        }
    }
}