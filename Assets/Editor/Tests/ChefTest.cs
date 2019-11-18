using Chefs;
using NUnit.Framework;

namespace Editor.Tests
{
    public class ChefTest
    {
        private Chef chef;

        [Test]
        public void decrease_happiness()
        {
            chef = new Chef(happiness: 0);

            var result = Chef.Update(ChefMsg.DecreaseHappiness, chef);
            
            Assert.AreEqual(-1, result.happiness);
        }
        
        [Test]
        public void increase_happiness()
        {
            chef = new Chef(happiness: 0);

            var result = Chef.Update(ChefMsg.IncreaseHappiness, chef);
            
            Assert.AreEqual(1, result.happiness);
        }

        [Test]
        public void sad_state()
        {
            chef = new Chef(happiness: -1);
            
            Assert.AreEqual(ChefState.Sad, chef.state);
        }
        
        [Test]
        public void happy_state()
        {
            chef = new Chef(happiness: 1);
            
            Assert.AreEqual(ChefState.Happy, chef.state);
        }
    }
}