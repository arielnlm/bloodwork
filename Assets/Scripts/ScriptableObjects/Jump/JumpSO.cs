using System;
using UnityEditor;
using UnityEngine;

namespace BloodWork.ScriptableObjects.Jump
{
    public abstract class JumpSO : ScriptableObject
    {
        private static float s_JumpForce;
        private static float s_ExtendJumpTimeLimit;

        protected static Action OnJumpValuesChange;
        
        [field: Header("Shared Properties")]
        [field: SerializeField] public float JumpForce           { get; private set; }
        [field: SerializeField] public float ExtendJumpTimeLimit { get; private set; }
        
        private void OnEnable()
        {
            SyncAndSave();

            OnJumpValuesChange += SyncAndSave;
        }

        private void OnDisable()
        {
            OnJumpValuesChange -= SyncAndSave;
        }

        private void OnValidate()
        {
            UpdateSharedValues();
    
            OnJumpValuesChange?.Invoke();
        }
        
        protected virtual void UpdateSharedValues()
        {
            s_JumpForce           = JumpForce;
            s_ExtendJumpTimeLimit = ExtendJumpTimeLimit;
        }
        
        protected virtual void SyncAndSave()
        {
            if (!IsValid())
                return;
            
            JumpForce           = s_JumpForce;
            ExtendJumpTimeLimit = s_ExtendJumpTimeLimit;
            
            EditorUtility.SetDirty(this);
        }
        
        private bool IsValid()
        {
            if (this)
                return true;
            
            OnJumpValuesChange -= SyncAndSave;
            
            return false;
        }
    }
}
