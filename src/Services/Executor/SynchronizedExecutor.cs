﻿#pragma warning disable 1591

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Shinden.Logger;

namespace Sanakan.Services.Executor
{
    public class SynchronizedExecutor : IExecutor
    {
        private IServiceProvider _provider;
        private ILogger _logger;

        private SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
        private BlockingCollection<IExecutable> _queue = new BlockingCollection<IExecutable>(100);

        public SynchronizedExecutor(ILogger logger)
        {
            _logger = logger;
        }
        
        public void Initialize(IServiceProvider provider)
        {
            _provider = provider;
            
            RunWorker();
        }

        public bool TryAdd(IExecutable task, TimeSpan timeout) 
        {
            if (_queue.TryAdd(task, timeout))
            {
                RunWorker();
                return true;
            }
            return false;
        }

        public void RunWorker() => _ = Task.Run(async () => await ProcessCommandsAsync());

        private async Task ProcessCommandsAsync()
        {
            if (!await _semaphore.WaitAsync(0))
                return;

            try
            {
                while (_queue.Count > 0)
                {
                    if (_queue.TryTake(out var cmd, 50))
                    {
                        try
                        {
                            await cmd.ExecuteAsync(_provider);
                        }
                        catch (Exception ex)
                        {
                            _logger.Log($"Executor: {ex}");
                        }
                    }
                    await Task.Delay(10);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}