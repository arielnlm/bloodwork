using System.Collections;
using System.Numerics;
using BloodWork.Attack.Range.Bullets;
using BloodWork.Commons;
using BloodWork.Entity.EventParams.Attack;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BloodWork.Attack.Range
{
    public sealed class BloodOrbRangeAttack : AbstractRangeAttack
    {
        [SerializeField] private Transform m_PositionOffset;
        [SerializeField] private Transform m_AimPosition;
        [SerializeField] private float m_CoolDownTimeLimit;

        [Header("KnockBack")] 
        [SerializeField] private float m_TimeToPauseMovement = 0.2f;
        [SerializeField] private float m_XKnockBack = 5f;
        [SerializeField] private float m_YKnockBack = 5f;

        private TriggerState m_TriggerState;
        private bool m_IsOnCooldown;
        private Camera m_Camera;

        protected override void Awake()
        {
            base.Awake();
            m_Camera = Camera.main;
        }


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

            EntityEnvironmentState entityEnvironmentState = Entity.Environment.Get();
            if (entityEnvironmentState is EntityEnvironmentState.Falling or EntityEnvironmentState.Rising)
                ApplyKnockBack(entityEnvironmentState);
        }

        private void ApplyKnockBack(EntityEnvironmentState entityEnvironmentState)
        {
            Vector3 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = ((Vector2)mousePosition - (Vector2)m_AimPosition.position).normalized;
            float xVelocity = Entity.Rigidbody.velocity.x - Vector2.Dot(Vector2.right, direction) * m_XKnockBack;
            float yVelocity = -Vector2.Dot(Vector2.up, direction)    * m_YKnockBack;
            Entity.Events.OnKnockBack(new EntityKnockBackParams(m_TimeToPauseMovement, new Vector2(xVelocity, yVelocity)));
        }

        private void Update()
        {
            Vector3 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - m_AimPosition.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            m_AimPosition.rotation = Quaternion.Euler(0, 0, -angle);
        }

        private Quaternion CalculateRotation()
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - (Vector2)m_PositionOffset.position).normalized;
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