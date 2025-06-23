using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class ApiResponseTests
    {
        [TestMethod]
        public void ApiResponse_Defaults_AreCorrect()
        {
            var response = new ApiResponse<string>();
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Error);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void ApiResponse_CanSetSuccessAndData()
        {
            var response = new ApiResponse<int>
            {
                Success = true,
                Data = 42
            };

            Assert.IsTrue(response.Success);
            Assert.AreEqual(42, response.Data);
            Assert.IsNull(response.Error);
        }

        [TestMethod]
        public void ApiResponse_CanSetError()
        {
            var response = new ApiResponse<object>
            {
                Success = false,
                Error = "Something went wrong"
            };

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Something went wrong", response.Error);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void ApiResponse_CanSetAllProperties()
        {
            var response = new ApiResponse<string>
            {
                Success = true,
                Error = "Warning",
                Data = "Test"
            };

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Warning", response.Error);
            Assert.AreEqual("Test", response.Data);
        }
    }
}