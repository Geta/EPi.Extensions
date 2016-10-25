using System;
using System.Collections.Generic;
using EPiFakeMaker;

namespace Geta.EPi.Extensions.Tests.Base
{
    public class Fixture
    {
        public static IEnumerable<FakePage> RandomPages(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return FakePage.Create(Guid.NewGuid().ToString());
            }
        }
    }
}