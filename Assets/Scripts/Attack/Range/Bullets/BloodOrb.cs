using UnityEngine;

namespace BloodWork.Attack.Range.Bullets
{
    public sealed class BloodOrb : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 10f;
        [SerializeField] private float m_Gravity = 0f;

        private Rigidbody2D m_rigidbody2D;

        private void Awake()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_rigidbody2D.gravityScale = m_Gravity;
        }

        private void FixedUpdate()
        {
            m_rigidbody2D.velocity = m_Speed * Time.fixedDeltaTime * transform.right;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            gameObject.SetActive(false);
        }
    }
}