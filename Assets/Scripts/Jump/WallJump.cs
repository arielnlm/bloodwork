using System;
using System.Collections;
using BloodWork.Assets.Scripts.Commons;
using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace BloodWork.Jump
{
    //IMPORTANT: For wall jump to work, Player MUST NOT have friction (friction = 0)
    public sealed class WallJump : AbstractJump
    {
        [Header("Wall Jump Properties")]
        [SerializeField] private float m_WallSlideSpeed = 40f;
        [SerializeField] private float m_HorizontalSpeedAfterJumping = 400f;
        [SerializeField] private float m_WallJumpTimeLimit = 0.06f;
        [SerializeField] private float m_DisableMovementTimeLimit = 0.15f;

        private EntityWallState m_SideOfWall;
        private float m_MinRange = -4f;
        private float m_MaxRange = float.MaxValue;
        private float m_WallJumpTime;
        private MoveDirection m_Direction;


        protected override void OnEnable()
        {
            base.OnEnable();
            Entity.Events.OnPerformMove += SetDirection;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Entity.Events.OnPerformMove -= SetDirection;
        }


        private void SetDirection(PerformMoveParams performMoveParams)
        {
            m_Direction = performMoveParams.Direction;
        }

        private void Update()
        {
            m_SideOfWall = Entity.EntityWallState;
            CheckWallJump();
        }

        void FixedUpdate()
        {
            if (IsWallSliding())
            {
                Vector2 oldVelocity = Entity.Rigidbody.velocity;
                float yVelocity = Mathf.Clamp(oldVelocity.y, m_MinRange, m_MaxRange) * m_WallSlideSpeed * Time.fixedDeltaTime;
                Entity.Rigidbody.velocity = new Vector2(oldVelocity.x, yVelocity);
            }

            if (JumpState == JumpState.Default && !ApplyJumpForce)
                return;

            if (ApplyJumpForce)
                Entity.Rigidbody.velocity = GetNewVelocityVector2();

            JumpTime += Time.fixedDeltaTime;
        }

        private Vector2 GetNewVelocityVector2()
        {
            float xVelocity = Entity.Rigidbody.velocity.x;


            if (m_SideOfWall == EntityWallState.OnWallLeft)
                xVelocity = m_HorizontalSpeedAfterJumping * Time.fixedDeltaTime;
            else if (m_SideOfWall == EntityWallState.OnWallRight)
                xVelocity = -m_HorizontalSpeedAfterJumping * Time.fixedDeltaTime;

            return new Vector2(xVelocity, JumpForce);
        }

        private void CheckWallJump()
        {
            m_WallJumpTime = IsWallSliding() ? 0f : m_WallJumpTime + Time.deltaTime;

            if (BehaviourState == BehaviourState.Disable || !IsJumpOwner && (TriggerState != TriggerState.Start || JumpState == JumpState.Jumping))
                return;

            if (!ChangeReference.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                return;

            JumpTime = 0f;

            if (IsWallSliding())
                StartCoroutine(EnableDisableMovement());
            Entity.Events.OnJumpState.Invoke(new JumpStateParams(JumpState.Jumping, GetInstanceID()));
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && TriggerState == TriggerState.Start && !HasWallJumpTimePassed();
        }

        private bool HasWallJumpTimePassed()
        {
            return m_WallJumpTime > m_WallJumpTimeLimit;
        }

        private bool IsWallSliding()
        {
            return m_SideOfWall != EntityWallState.None;
        }

        /// <summary>
        /// Player will lose controller of character for some time before getting it back.
        /// This is so if player is on wall that he cant stick again to the wall immediately after jumping
        /// </summary>
        /// <returns></returns>
        private IEnumerator EnableDisableMovement()
        {
            Entity.Events.OnMoveDirectionChange?.Invoke(new MoveBehaviourStateParams(BehaviourState.Disable));

            yield return new WaitForSeconds(m_DisableMovementTimeLimit);

            Entity.Events.OnMoveDirectionChange?.Invoke(new MoveBehaviourStateParams(BehaviourState.Enable));
        }
    }
}
