﻿using System;

namespace Cat.Infrastructure.Fake.BotApiComponents.OperationalClient
{
    public interface IFakeOperationalClientTimeout
    {
        TimeSpan Timeout { get; }
        DateTime LastReset { get; }
        TimeSpan CurrentDuration { get; }
        bool IsNotElapsed { get; }

        void Reset(TimeSpan timeout);
        void Reset();
    }

    public class FakeOperationalClientTimeout : IFakeOperationalClientTimeout
    {
        private readonly IFakeOperationalClientRandomUtils _randomUtils;

        public TimeSpan Timeout { get; private set; }
        public DateTime LastReset { get; private set; }

        public FakeOperationalClientTimeout(IFakeOperationalClientRandomUtils randomUtils)
        {
            _randomUtils = randomUtils;
            Reset(TimeSpan.Zero);
        }

        public TimeSpan CurrentDuration => DateTime.Now - LastReset;
        public bool IsNotElapsed => CurrentDuration < Timeout;

        public void Reset(TimeSpan timeout)
        {
            Timeout = timeout;
            LastReset = DateTime.Now;
        }

        public void Reset() =>
            Reset(_randomUtils.NextTimeout());
    }
}
