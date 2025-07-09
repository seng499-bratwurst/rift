using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rift.Models;

namespace Rift.Tests.Models
{
    [TestClass]
    public class RoleChangeResultTests
    {
        [TestMethod]
        public void RoleChangeResult_Enum_HasExpectedValues()
        {
            Assert.AreEqual(0, (int)RoleChangeResult.Success);
            Assert.AreEqual(1, (int)RoleChangeResult.UserNotFound);
            Assert.AreEqual(2, (int)RoleChangeResult.RemoveRolesFailed);
            Assert.AreEqual(3, (int)RoleChangeResult.AddRoleFailed);
        }

        [TestMethod]
        public void RoleChangeResult_Enum_CanBeParsedFromString()
        {
            Assert.IsTrue(Enum.TryParse("Success", out RoleChangeResult result));
            Assert.AreEqual(RoleChangeResult.Success, result);

            Assert.IsTrue(Enum.TryParse("UserNotFound", out result));
            Assert.AreEqual(RoleChangeResult.UserNotFound, result);

            Assert.IsTrue(Enum.TryParse("RemoveRolesFailed", out result));
            Assert.AreEqual(RoleChangeResult.RemoveRolesFailed, result);

            Assert.IsTrue(Enum.TryParse("AddRoleFailed", out result));
            Assert.AreEqual(RoleChangeResult.AddRoleFailed, result);
        }
    }
}