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
        private List<CollectorAgent> agentCollectors;

        [SetUp]
        public void Setup()
        {
            agentCollectors = new List<CollectorAgent>();
            foodSpawner = Substitute.For<CollectorFoodSpawner>();
            area = new CollectingArea();
            area.Setup(agentCollectors, foodSpawner);
        }
    }
}