using UnityEngine;

namespace BloodWork.Entity.EventParams.Attack
{
    public struct EntityKnockBackParams
    {
        public float TimeToPauseMovement;
        public Vector2 PowerOfKnockBack;

        public EntityKnockBackParams(float timeToPauseMovement, Vector2 powerOfKnockBack)
        {
            TimeToPauseMovement = timeToPauseMovement;
            PowerOfKnockBack = powerOfKnockBack;
        }
    }
}