using System.Collections;
using BloodWork.Attack.Range.Bullets;
using BloodWork.Commons;
using BloodWork.Entity.EventParams.Attack;
using UnityEngine;

namespace BloodWork.Attack.Range
{
    public sealed class BloodOrbRangeAttack : AbstractRangeAttack
    {
        [SerializeField] private Transform m_PositionOffset;
        [SerializeField] private Transform m_AimPosition;
        [SerializeField] private float m_CoolDownTimeLimit;

        private TriggerState m_TriggerState;
        private bool m_IsOnCooldown;

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
            if (m_TriggerState != TriggerState.Start || m_IsOnCooldown)
                return;

            AbstractAmmo ammo = GetPooledAmmo();
            if (ammo == null)
                return;

            ammo.transform.position = m_PositionOffset.position;
            ammo.transform.rotation = CalculateRotation();
            ammo.gameObject.SetActive(true);

            StartCoroutine(Cooldown());
        }

        private void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - m_AimPosition.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            m_AimPosition.rotation = Quaternion.Euler(0, 0, -angle);
        }

        private Quaternion CalculateRotation()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)m_PositionOffset.position).normalized;
            return Quaternion.FromToRotation(Vector3.right, direction);
        }

        private IEnumerator Cooldown()
        {
            m_IsOnCooldown = true;

            yield return new WaitForSeconds(m_CoolDownTimeLimit);

            m_IsOnCooldown = false;
        }
    }
}