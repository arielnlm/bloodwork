using BloodWork.Attack.Range.Bullets;
using BloodWork.Commons;
using BloodWork.Entity.EventParams.Attack;
using UnityEngine;

namespace BloodWork.Attack.Range
{
    public sealed class BloodOrbRangeAttack : AbstractRangeAttack
    {

        [SerializeField] private BloodOrb m_BloodOrbPrefab;
        [SerializeField] private Transform m_PositionOffset;

        private TriggerState m_TriggerState;

        private void OnEnable()
        {
            Entity.Events.OnPerformBloodOrbAttack += SetTriggerState;
        }

        private void OnDisable()
        {
            Entity.Events.OnPerformBloodOrbAttack -= SetTriggerState;
        }

        private void SetTriggerState(PerformBloodOrbAttackParams performBloodOrbAttack)
        {
            m_TriggerState = performBloodOrbAttack.State;

            Attack();
        }

        private void Attack()
        {
            if (m_TriggerState != TriggerState.Start)
                return;

            Instantiate(m_BloodOrbPrefab, m_PositionOffset.position, CalculateRotation());
        }

        private Quaternion CalculateRotation()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)m_PositionOffset.position).normalized;
            return Quaternion.FromToRotation(Vector3.right, direction);
        }
    }
}