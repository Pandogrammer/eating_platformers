using Flying.Scripts.Fly;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Flying.Editor.Tests.Flying
{
    public class FlyingAreaTest
    {

        [Test]
        public void inside_area()
        {
            var target = new Vector3(0, 3, 0);
            var agent = FlyingAgent.Create(position: new Vector3(0, 2.999f, 0));
            var area = FlyingArea.Create(agent, target);

            var result = FlyingArea.IsInsideArea(area);

            Assert.IsTrue(result);
        }
        
    }
}