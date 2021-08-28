using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MOFServer.UnitTest
{
    public class CacheSvcTest
    {
        [Test]
        public void TestTrue()
        {
            bool r = true;
            Assert.True(r);
        }
        [Test]
        public void TestCacheSvcExist()
        {
            var cacheSvc = CacheSvc.Instance;
            cacheSvc.Init();
            bool r = false;
            if (cacheSvc != null)
            {
                r = true;
            }            
            Assert.True(r);
        }
    }
}
