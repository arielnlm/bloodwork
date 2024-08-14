using UnityEngine;

namespace BloodWork.Entity
{
    public abstract class AbstractEntity : MonoBehaviour
    {
        public Events      Events;
        public Rigidbody2D Rigidbody { get; private set; }

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
