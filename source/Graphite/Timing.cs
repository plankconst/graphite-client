﻿using System;

namespace Graphite
{
    internal class Timing
    {
        private readonly bool ownsWatch = false;

        private readonly IStopwatch watch;

        private readonly long startTicks;

        private long? stopTicks;

        private bool disposed;

        public Timing(IStopwatch watch)
        {
            if (watch == null)
                throw new ArgumentNullException("watch");

            this.watch = watch;
            this.startTicks = watch.ElapsedTicks;
        }

        public Timing()
            : this(StopwatchWrapper.StartNew())
        {
            this.ownsWatch = true;
        }

        public int? ComputeElapsedMilliseconds()
        {
            if (!this.stopTicks.HasValue)
                return default(int?);

            return Helpers.ConvertTicksToMs(this.stopTicks.Value - this.startTicks, this.watch.Frequency);
        }

        public bool Stop()
        {
            if (this.watch != null && !this.stopTicks.HasValue)
            {
                this.stopTicks = this.watch.ElapsedTicks;

                return true;
            }

            return false;
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.Stop();

                if (this.watch != null && this.ownsWatch)
                {
                    this.watch.Stop();
                }

                this.disposed = true;
            }
        }
    }
}
