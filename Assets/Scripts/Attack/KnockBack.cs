using System.Collections;
using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using BloodWork.Entity.EventParams.Attack;
using UnityEngine;

namespace BloodWork.Attack
{
    public class KnockBack : EntityBehaviour
    {

        private float m_TimeToPauseMovement;

        private void OnEnable()
        {
            Entity.Events.OnKnockBack += ApplyKnockBack;
        }
        private void OnDisable()
        {
            Entity.Events.OnKnockBack -= ApplyKnockBack;
        }

        private void ApplyKnockBack(EntityKnockBackParams entityKnockBackParams)
        {
            m_TimeToPauseMovement = entityKnockBackParams.TimeToPauseMovement;
            StartCoroutine(EnableDisableMovement());
            Entity.Rigidbody.AddForce(entityKnockBackParams.PowerOfKnockBack, ForceMode2D.Impulse);
        }

        private IEnumerator EnableDisableMovement()
        {
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Disable));

            yield return new WaitForSeconds(m_TimeToPauseMovement);

            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Enable));
        }

    }
}
