using System.Collections.Generic;
using GenericScripts.Stepable;
using Nolose.Scripts;
using NSubstitute;
using NUnit.Framework;

namespace Nolose.Editor.Tests
{
    public class CollectingAreaTest
    {
        private CollectingArea area;
        private CollectorFoodSpawner foodSpawner;
        private List<AgentCollector> agentCollectors;

        [SetUp]
        public void Setup()
        {
            agentCollectors = new List<AgentCollector>();
            foodSpawner = Substitute.For<CollectorFoodSpawner>();
            area = new CollectingArea();
            area.Setup(agentCollectors, foodSpawner);
        }
        
        [Test]
        public void area_reset()
        {
            for (int i = 0; i < 15; i++)
            {
                area.Step(10);            
            }
            
            Assert.AreEqual(5, area.step);
        }
    }
}