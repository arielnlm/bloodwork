using System.Collections.Generic;
using UnityEngine;

namespace BloodWork.Entity
{
    public class EntityBehaviour : MonoBehaviour
    {
        protected AbstractEntity Entity { get; private set; }

        protected virtual void Awake()
        {
            Entity = GetComponent<AbstractEntity>();
        }
    }
}
