using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams.Attack;
using System.Collections;
using UnityEngine;

namespace BloodWork
{
    public class SwordAttack : EntityBehaviour
    {
        [SerializeField] private float m_CoolDownTimeLimit;

        private bool m_IsOnCooldown;

        [Header("KnockBack")]
        [SerializeField] private float m_TimeToPauseMovement = 0.2f;
        [SerializeField] private float m_XKnockBack = 5f;
        [SerializeField] private float m_YKnockBack = 5f;

        private Camera m_Camera;


        protected override void Awake()
        {
            base.Awake();
            m_Camera = Camera.main;
        }


        private void Update()
        {
            if (!m_IsOnCooldown && Input.GetKeyDown(KeyCode.Mouse0))
            {

                ApplyKnockBack();
                StartCoroutine(Cooldown());
            }
        }


        private void ApplyKnockBack()
        {
            Vector3 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = ((Vector2)mousePosition - (Vector2)Entity.gameObject.transform.position).normalized;
            float xVelocity = -Vector2.Dot(Vector2.right, direction) * m_XKnockBack;
            float yVelocity = -Vector2.Dot(Vector2.up, direction) * m_YKnockBack;

            if (Entity.Rigidbody.linearVelocity.x < 0 && xVelocity < 0 || Entity.Rigidbody.linearVelocity.x > 0 && xVelocity > 0)
                xVelocity = Entity.Rigidbody.linearVelocity.x + xVelocity;

            Entity.Events.OnKnockBack(new EntityKnockBackParams(Mathf.Abs(xVelocity) < 1f ? 0f : m_TimeToPauseMovement, new Vector2(xVelocity, yVelocity)));
        }

        private IEnumerator Cooldown()
        {
            m_IsOnCooldown = true;

            yield return new WaitForSeconds(m_CoolDownTimeLimit);

            m_IsOnCooldown = false;
        }

        private Quaternion CalculateRotation()
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - (Vector2)Entity.gameObject.transform.position).normalized;
            return Quaternion.FromToRotation(Vector3.right, direction);
        }
    }
}
