using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding_Visualizer.Utilities
{
    public class AnimationController
    {
        public int animationDelay { get; set; } = 20;

        private readonly ManualResetEventSlim _pauseEvent = new(true);

        private CancellationTokenSource _cts = new();

        public CancellationToken Token => _cts.Token;

        public async Task WaitAsync()
        {
            _pauseEvent.Wait();

            Token.ThrowIfCancellationRequested();

            await Task.Delay(animationDelay, Token);
        }

        public void Pause()
        {
            _pauseEvent.Reset();
        }

        public void Resume()
        {
            _pauseEvent.Set();
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void Reset()
        {
            _cts = new CancellationTokenSource();
            _pauseEvent.Set();
        }
    }
}
