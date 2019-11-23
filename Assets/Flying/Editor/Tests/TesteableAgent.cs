using MLAgents;

namespace Flying.Editor.Tests
{
    public abstract class TesteableAgent : Agent
    {
        public new virtual void AddReward(float reward)
        {
            base.AddReward(reward);
        }

        public new virtual void SetReward(float reward)
        {
            base.SetReward(reward);
        }

        public new virtual void Done()
        {
            base.Done();
        }
    }
}