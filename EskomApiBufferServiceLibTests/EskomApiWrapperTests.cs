using EskomApiBufferService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EskomApiBufferServiceLibTests
{
    public class EskomApiWrapperTests
    {
        [Fact]
        public async Task GetStatusReturnsStringBetweenOneAndSix()
        {
            // Arrange
            EskomApiWrapper eskomApiWrapper = new EskomApiWrapper();

            // Act
            string result = await eskomApiWrapper.GetStatusAsync();
            int intResult;
            if (!Int32.TryParse(result, out intResult))
            {
                throw new Exception("Failed to parse GetStatusAsync() results to int.");
            }

            // Assert
            Assert.InRange(intResult, 0, 6);
        }
    }
}
