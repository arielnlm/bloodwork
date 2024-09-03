using UnityEditor;
using UnityEngine;

namespace BloodWork.ScriptableObjects
{
    public abstract class SharedSO : ScriptableObject
    {
        protected virtual void OnEnable()
        {
            SyncLocalValuesAndSave();
            Subscribe();
        }

        protected virtual void OnDisable()
        {
            Unsubscribe();
        }

        protected virtual void OnValidate()
        {
            SyncSharedValues();
            Dispatch();
        }

        /// <summary>
        /// Synchronises local with shared variables and mark the asset as ready to write on disk.
        /// </summary>
        protected virtual void SyncLocalValuesAndSave()
        {
            if (!IsValid())
                return;

            SyncLocalValues();
            
            EditorUtility.SetDirty(this);
        }
        
        /// <summary>
        /// Checks the object's validity state, and unsubscribes invalid object if autoUnsubscribe is true.
        /// </summary>
        /// <param name="autoUnsubscribe">whether null instance should auto unsubscribe</param>
        /// <returns>Returns true if object is not null, false otherwise</returns>
        protected bool IsValid(bool autoUnsubscribe = true)
        {
            if (this)
                return true;
            
            if (autoUnsubscribe) 
                Unsubscribe();
            
            return false;
        }
        
        /// <summary>
        /// Its called whenever an object needs to subscribe to an event.
        /// </summary>
        protected abstract void Subscribe();
        
        /// <summary>
        /// Its called whenever an object needs to unsubscribe from an event.
        /// </summary>
        protected abstract void Unsubscribe();
        
        /// <summary>
        /// Its called whenever the object needs to notify subscribers of an event.
        /// </summary>
        protected abstract void Dispatch();
        
        /// <summary>
        /// Its called whenever local variables change and shared variables need to be synchronized.
        /// </summary>
        protected abstract void SyncSharedValues();
        
        /// <summary>
        /// Its called whenever shared variables change and local variables need to be synchronized.
        /// </summary>
        protected abstract void SyncLocalValues();
    }
}
