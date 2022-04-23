using EskomApiBufferService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EskomApiBufferServiceLibTests
{
    public class EskomApiBufferServiceTests
    {
        [Fact]
        public async void BufferServiceShouldReturnTheMostRecentStatus()
        {
            // Arrange
            MockEskomApiWrapper eskomApiWrapper = new MockEskomApiWrapper();
            eskomApiWrapper.Delay = 500;
            eskomApiWrapper.GetStatusResponse = "3";

            BufferService bufferService = new BufferService(null, eskomApiWrapper);
            bufferService.DelayInMinutes = 0;

            // Act
            bufferService.Start();
            await Task.Delay(1000);
            Status status = bufferService.MostRecentStatus;

            // Assert
            Assert.NotNull(status);
            Assert.Equal("3", status.Level.ToString());
        }

        [Fact]
        public async void BufferServiceStatusesLoggedShouldBeMoreThanOne()
        {
            // Arrange
            MockEskomApiWrapper eskomApiWrapper = new MockEskomApiWrapper();
            eskomApiWrapper.Delay = 500;
            eskomApiWrapper.GetStatusResponse = "3";

            BufferService bufferService = new BufferService(null, eskomApiWrapper);
            bufferService.DelayInMinutes = 0;

            // Act
            bufferService.Start();
            await Task.Delay(2000);

            // Assert
            Assert.True(bufferService.StatusesLogged > 1);
        }

        [Fact]
        public async void BufferServiceStatusesLoggedShouldBeZero()
        {
            // Arrange
            MockEskomApiWrapper eskomApiWrapper = new MockEskomApiWrapper();
            eskomApiWrapper.Delay = 500;
            eskomApiWrapper.GetStatusResponse = String.Empty;

            BufferService bufferService = new BufferService(null, eskomApiWrapper);
            bufferService.DelayInMinutes = 0;

            // Act
            bufferService.Start();
            await Task.Delay(2000);

            // Assert
            Assert.True(bufferService.StatusesLogged == 0);
        }

        [Fact]
        public async void BufferServiceShouldHandleWebException()
        {
            // Arrange
            MockEskomApiWrapper eskomApiWrapper = new MockEskomApiWrapper();
            eskomApiWrapper.Delay = 100;
            eskomApiWrapper.GetStatusResponse = "3";
            eskomApiWrapper.ThrowWebException = true;

            BufferService bufferService = new BufferService(null, eskomApiWrapper);
            bufferService.DelayInMinutes = 0;

            // Act
            bufferService.Start();
            await Task.Delay(1000);
            Status status = bufferService.MostRecentStatus;

            // Assert
            Assert.NotNull(status);
            Assert.Equal("3", status.Level.ToString());
        }

        [Fact]
        public async void BufferServiceShouldStop()
        {
            // Arrange
            MockEskomApiWrapper eskomApiWrapper = new MockEskomApiWrapper();
            eskomApiWrapper.Delay = 500;
            eskomApiWrapper.GetStatusResponse = "3";

            BufferService bufferService = new BufferService(null, eskomApiWrapper);
            bufferService.DelayInMinutes = 0;

            // Act
            #pragma warning disable CS4014 
            Task.Run(async () =>
            {
                await Task.Delay(2000);
                bufferService.Stop();
            });
            #pragma warning restore CS4014

            await bufferService.StartAsync();

            // Assert
            Assert.True(bufferService.StatusesLogged > 1);
        }

        [Fact]
        public async void BufferServiceCleansUpStatusLogs()
        {
            // Arrange
            MockEskomApiWrapper eskomApiWrapper = new MockEskomApiWrapper();
            eskomApiWrapper.Delay = 100;
            eskomApiWrapper.GetStatusResponse = "3";

            BufferService bufferService = new BufferService(null, eskomApiWrapper);
            bufferService.DelayInMinutes = 0;
            bufferService.MaxLogs = 20;

            // Act
            bufferService.Start();

            do
            {
                await Task.Delay(100);
            } while (bufferService.StatusesLogged < 20);
            await Task.Delay(300);

            // Assert
            Assert.InRange(bufferService.StatusesLogged, 10, 20);
        }
    }
}
