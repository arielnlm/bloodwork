using BloodWork.Commons;
using BloodWork.Entity;
using BloodWork.Entity.EventParams;
using UnityEngine;

namespace BloodWork.Movement
{
    public class Movement : EntityBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 300f;

        private MoveDirection m_Direction;
        protected BehaviourState State;

        protected override void Awake()
        {
            base.Awake();
            ChangeState(new MoveStateParams(BehaviourState.Enable));
        }

        private void OnEnable()
        {
            Entity.Events.OnPerformMove += SetDirection;
            Entity.Events.OnMoveStateEvent += ChangeState;
        }

        private void ChangeState(MoveStateParams controllerStateParams)
        {
            State = controllerStateParams.State;
        }

        private void OnDisable()
        {
            Entity.Events.OnPerformMove -= SetDirection;
        }

        private void SetDirection(Move move)
        {
            m_Direction = move.Direction;
        }

        private void FixedUpdate()
        {
            if (State == BehaviourState.Disable)
                return;
            Entity.Rigidbody.velocity = new Vector2(m_Direction.GetValue() * m_MaxSpeed * Time.fixedDeltaTime, Entity.Rigidbody.velocity.y);
        }
    }
}
