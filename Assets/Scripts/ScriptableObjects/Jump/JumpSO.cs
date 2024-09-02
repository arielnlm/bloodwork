using System;
using UnityEngine;

namespace BloodWork.ScriptableObjects.Jump
{
    [CreateAssetMenu(fileName = "Jump", menuName = "Bloodwork/Jump/Jump", order = 0)]
    public class JumpSO : ScriptableObject
    {
        private static float s_JumpForce;
        private static float s_ExtendJumpTimeLimit;

        protected static Action OnJumpForceChange;
        protected static Action OnExtendJumpTimeLimitChange;

        [field: Header("Shared Properties")]
        [field: SerializeField] public float JumpForce           { get; private set; }
        [field: SerializeField] public float ExtendJumpTimeLimit { get; private set; }

        private void OnEnable()
        {
            JumpForce           = s_JumpForce;
            ExtendJumpTimeLimit = s_ExtendJumpTimeLimit;

            OnJumpForceChange           += () => JumpForce           = s_JumpForce;
            OnExtendJumpTimeLimitChange += () => ExtendJumpTimeLimit = s_ExtendJumpTimeLimit;
        }

        private void OnDisable()
        {
            OnJumpForceChange           -= () => JumpForce           = s_JumpForce;
            OnExtendJumpTimeLimitChange -= () => ExtendJumpTimeLimit = s_ExtendJumpTimeLimit;
        }

        private void OnValidate()
        {
            s_JumpForce = JumpForce;
            s_ExtendJumpTimeLimit = ExtendJumpTimeLimit;
            
            OnJumpForceChange?.Invoke();
            OnExtendJumpTimeLimitChange?.Invoke();
        }
    }
}
