using System;
using UnityEngine;

namespace BloodWork.ScriptableObjects.Jump
{
    public abstract class JumpSO : SharedSO
    {
        private static float s_JumpForce;
        private static float s_ExtendJumpTimeLimit;

        protected static Action OnJumpValuesChange;
        
        [field: Header("Shared Properties")]
        [field: SerializeField] public float JumpForce           { get; private set; }
        [field: SerializeField] public float ExtendJumpTimeLimit { get; private set; }


        protected override void Subscribe()
        {
            OnJumpValuesChange += SyncLocalValuesAndSave;
        }

        protected override void Unsubscribe()
        {
            OnJumpValuesChange -= SyncLocalValuesAndSave;
        }

        protected override void Dispatch()
        {
            OnJumpValuesChange?.Invoke();
        }

        protected override void SyncSharedValues()
        {
            s_JumpForce           = JumpForce;
            s_ExtendJumpTimeLimit = ExtendJumpTimeLimit;
        }

        protected override void SyncLocalValues()
        {
            JumpForce           = s_JumpForce;
            ExtendJumpTimeLimit = s_ExtendJumpTimeLimit;
        }
    }
}
