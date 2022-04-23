using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EskomApiBufferService
{
    public class BufferService
    {
        private readonly ILogger _logger;
        private IEskomApiWrapper _eskomApiWrapper = null;
        private ConcurrentStack<Status> _statusLogs = new ConcurrentStack<Status>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public int Retries { get; set; } = 3;
        public int StatusMinRange { get; set; } = 0;
        public int StatusMaxRange { get; set; } = 6;
        public int DelayInMinutes { get; set; } = 1;
        public int StatusesLogged { get => _statusLogs.Count; }
        public int MaxLogs { get; set; } = 1000;
        public Status MostRecentStatus
        {
            get
            {
                Status status = null;
                _statusLogs.TryPeek(out status);
                return status;
            }
        }
        public Status[] Statuses { get => _statusLogs.ToArray(); }

        public BufferService(ILogger<BufferService> logger, IEskomApiWrapper eskomApiWrapper)
        {
            if (eskomApiWrapper == null) { throw new ArgumentNullException(nameof(eskomApiWrapper)); }

            _logger = logger;
            _eskomApiWrapper = eskomApiWrapper;
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                try
                {
                    do
                    {
                        await Task.Delay(DelayInMinutes * 60 * 1000);

                        await GetStatusUpdate(_cancellationTokenSource.Token);

                        cleanUpStatusLogs();
                    } while (!_cancellationTokenSource.Token.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(exception: ex, "BufferService error.");
                    Console.WriteLine(ex.Message);
                }
            });
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        public async Task StartAsync()
        {
            await Task.Run(async () =>
            {
                do
                {
                    await Task.Delay(DelayInMinutes * 60 * 1000);

                    await GetStatusUpdate(_cancellationTokenSource.Token);

                    cleanUpStatusLogs();
                } while (!_cancellationTokenSource.Token.IsCancellationRequested);
            });
        }

        private async Task GetStatusUpdate(CancellationToken cancellationToken)
        {
            int retry = 0;
            do
            {
                if (cancellationToken.IsCancellationRequested) { break; }

                try
                {
                    string response = await _eskomApiWrapper.GetStatusAsync();
                    if (IsValidStatusResponse(response))
                    {
                        Status status = new Status(response);
                        _statusLogs.Push(status);
                        break;
                    }
                    else
                    {
                        retry++;
                    }
                }
                catch (System.Net.WebException ex)
                {
                    _logger?.LogError(exception: ex, $"Network Error: Failed to get new eskom status. Retries: '{retry}'.");
                    retry++;
                }
            } while (retry < Retries);
        }

        private void cleanUpStatusLogs()
        {
            if (StatusesLogged > MaxLogs)
            {
                Status[] temp = _statusLogs.ToArray().Take(10).Reverse().ToArray();
                _statusLogs.Clear();
                _statusLogs.PushRange(temp);
            }
        }

        private bool IsValidStatusResponse(string response)
        {
            int intValue = 0;
            if (!Int32.TryParse(response, out intValue))
            {
                return false;
            }

            if (intValue < StatusMinRange || intValue > StatusMaxRange)
            {
                return false;
            }

            return true;
        }
    }
}
