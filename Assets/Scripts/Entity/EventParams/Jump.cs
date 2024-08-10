using BloodWork.Commons;

namespace BloodWork.Entity.EventParams
{
    public struct PerformJumpParams
    {
        public KeyState KeyState;

        public PerformJumpParams(KeyState keyState)
        {
            KeyState = keyState;
        }
    }

    public struct JumpStateParams
    {
        public JumpState JumpState;

        public JumpStateParams(JumpState jumpState)
        {
            JumpState = jumpState;
        }
    }
}
