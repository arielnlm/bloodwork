using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct ControllerStateParams
    {
        public BehaviourState State;

        public ControllerStateParams(BehaviourState state)
        {
            State = state;
        }
    }

    public struct GamePauseParams
    {
        public bool Pause;

        public GamePauseParams(bool pause)
        {
            Pause = pause;
        }
    }
}
