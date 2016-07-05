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
        [DllImport("kernel32.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern short QueryPerformanceCounter(ref long x);

        /// <summary>
        /// Native QueryPerformanceFrequency function import.
        /// </summary>
        /// <param name="x">Output <paramref name="x"/> value.</param>
        [DllImport("kernel32.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern short QueryPerformanceFrequency(ref long x);

        /// <summary>
        /// <see cref="Stopwatch"/> start time value.
        /// </summary>
        private long _startTime;

        /// <summary>
        /// <see cref="Stopwatch"/> stop time value.
        /// </summary>
        private long _stopTime;

        /// <summary>
        /// <see cref="Stopwatch"/> clack frequency value.
        /// </summary>
        private long _clockFrequency;

        /// <summary>
        /// <see cref="Stopwatch"/> calibration time value.
        /// </summary>
        private long _calibrationTime;

        /// <summary>
        /// Initializes new instance of <see cref="Stopwatch"/> object.
        /// </summary>
        public Stopwatch()
        {
            _startTime = 0;
            _stopTime = 0;
            _clockFrequency = 0;
            _calibrationTime = 0;
            Calibrate();
        }

        /// <summary>
        /// Resets <see cref="Stopwatch"/> object: sets it start and stop time to zero values.
        /// </summary>
        public void Reset()
        {
            _startTime = 0;
            _stopTime = 0;
        }

        /// <summary>
        /// Start <see cref="Stopwatch"/> counter.
        /// </summary>
        public void Start()
        {
            QueryPerformanceCounter(ref _startTime);
        }

        /// <summary>
        /// Stops <see cref="Stopwatch"/> counter.
        /// </summary>
        public void Stop()
        {
            QueryPerformanceCounter(ref _stopTime);
        }

        /// <summary>
        /// Gets <see cref="TimeSpan"/> value, elapsed between <see cref="Stopwatch"/> start and stop events.
        /// </summary>
        /// <returns><see cref="TimeSpan"/> value between <see cref="Stopwatch"/> has been started and stopped.</returns>
        public TimeSpan GetElapsedTimeSpan()
        {
            return TimeSpan.FromMilliseconds(InternalGetElapsedTimeMs());
        }

        /// <summary>
        /// Gets time, elapsed between <see cref="Stopwatch"/> start and stop events in microseconds.
        /// </summary>
        /// <returns><see cref="double"/> value in microseconds between <see cref="Stopwatch"/> has been started and stopped.</returns>
        public double GetElapsedTimeInMicroseconds()
        {
            return ((((_stopTime - _startTime - _calibrationTime) * 1000000.0) / _clockFrequency));
        }

        /// <summary>
        /// Calibrates <see cref="Stopwatch"/> object.
        /// </summary>
        private void Calibrate()
        {
            QueryPerformanceFrequency(ref _clockFrequency);

            for (int i = 0; i < 1000; i++)
            {
                Start();
                Stop();
                _calibrationTime += _stopTime - _startTime;
            }

            _calibrationTime /= 1000;
        }

        private double InternalGetElapsedTimeMs()
        {
            return ((((_stopTime - _startTime - _calibrationTime) * 1000000.0) / _clockFrequency) / 1000.0);
        }
    }
}