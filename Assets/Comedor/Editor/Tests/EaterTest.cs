using Eaters;
using Eaters.Messages;
using NUnit.Framework;

namespace Comedor.Editor.Tests
{
    public class EaterTest
    {
        private Eater eater;
        

        [Test]
        public void increment_hunger()
        {
            eater = new Eater(hunger: 0);

            var result = Eater.Update(EaterMsg.IncrementHunger, eater);
            
            Assert.AreEqual(1, result.hunger);
        }

        [Test]
        public void decrease_hunger()
        {
            eater = new Eater(hunger: 10);
            var parameters = new DecreaseHunger(5);

            var result = Eater.Update(EaterMsg.DecreaseHunger, eater, parameters);
            
            Assert.AreEqual(5, result.hunger);
        }

        [Test]
        public void sad_state()
        {
            eater = new Eater(hunger: 0);

            var result = Eater.Update(EaterMsg.IncrementHunger, eater);
            
            Assert.AreEqual(EaterState.Sad, result.state);
        }
        
        [Test]
        public void happy_state()
        {
            eater = new Eater(hunger: 1);

            var result = Eater.Update(EaterMsg.DecreaseHunger, eater, new DecreaseHunger(3));
            
            Assert.AreEqual(EaterState.Happy, result.state);
        }
    }

}