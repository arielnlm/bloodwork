using System;
using System.Collections;
using System.Numerics;
using BloodWork.Assets.Scripts.Commons;
using BloodWork.Commons;
using BloodWork.Entity.EventParams;
using BloodWork.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BloodWork.Jump
{
    //IMPORTANT: For wall jump to work, Player MUST NOT have friction (friction = 0)
    //TODO Huge Jump when holding left and right direction and pressing jump
    public sealed class WallJump : AbstractJump
    {
        [Header("Wall Jump Properties")]
        [SerializeField] private float m_WallSlideSpeed = 40f;
        [SerializeField] private float m_HorizontalSpeedAfterJumping = 400f;
        [SerializeField] private float m_WallJumpTimeLimit = 0.06f;
        [SerializeField] private float m_DisableMovementTimeLimit = 0.15f;

        private EntityEnvironmentState m_EntityEnviroment;
        private float m_MinRange = -4f;
        private float m_MaxRange = float.MaxValue;
        private float m_WallJumpTime;
        private MoveDirection m_OnWallDirection;
        private bool m_OneJumpPassed;
        private EntityEnvironmentState m_EntityEnviromentOnJump;

        private void Update()
        {
            m_EntityEnviroment = Entity.Environment.Get();
            if (IsWallSliding())
                m_OnWallDirection = MoveDirections.ValueOf(Entity.transform.right.x);
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

            if (ApplyJumpForce && !m_OneJumpPassed)
            {
                Jump();
                m_OneJumpPassed = true;
            }

            JumpTime += Time.fixedDeltaTime;
        }

        private void Jump()
        {
            float xVelocity = Entity.Rigidbody.velocity.x;
            float yVelocity = JumpForce;

            Debug.Log(m_EntityEnviromentOnJump);
            if (m_EntityEnviromentOnJump is EntityEnvironmentState.OnWallLeft)
                xVelocity = m_HorizontalSpeedAfterJumping * Time.fixedDeltaTime;
            else if (m_EntityEnviromentOnJump is EntityEnvironmentState.OnWallRight)
                xVelocity = -m_HorizontalSpeedAfterJumping * Time.fixedDeltaTime;
            else
                yVelocity *= 0.8f;

            Entity.Rigidbody.velocity = Vector2.zero;
            Entity.Rigidbody.AddForce(new Vector2(xVelocity, yVelocity), ForceMode2D.Impulse);
        }

        private void CheckWallJump()
        {
            m_WallJumpTime = IsWallSliding() ? 0f : m_WallJumpTime + Time.deltaTime;
            m_OneJumpPassed = ApplyJumpForce && m_OneJumpPassed;

            if (BehaviourState == BehaviourState.Disable || !IsJumpOwner && (TriggerState != TriggerState.Start || JumpState == JumpState.Jumping))
                return;

            if (!ChangeReference.IsChangedTo(ref ApplyJumpForce, ShouldApplyJumpForce(), true))
                return;

            ReadyForJump();
        }

        private void ReadyForJump()
        {
            JumpTime = 0f;
            m_EntityEnviromentOnJump = m_EntityEnviroment;
            if (IsWallSliding())
            {
                StartCoroutine(EnableDisableMovement());
                Vector3 direction = Entity.transform.right;
                Entity.transform.right = new Vector3(-direction.x, direction.y, direction.z);
            }
            Entity.Events.OnJumpState.Invoke(new JumpStateParams(JumpState.Jumping, GetInstanceID()));
        }

        private bool ShouldApplyJumpForce()
        {
            return ApplyJumpForce && !IsCeilingHit() && (!IsMinimumJumpTimePassed() || (TriggerState == TriggerState.Continue && !IsExtendedTimePassed())) ||
                   !ApplyJumpForce && TriggerState == TriggerState.Start && (IsWallSliding() || !HasWallJumpTimePassed() && m_OnWallDirection != MoveDirections.ValueOf(Entity.transform.right.x));
        }

        private bool HasWallJumpTimePassed()
        {
            return m_WallJumpTime > m_WallJumpTimeLimit;
        }

        private bool IsWallSliding()
        {
            return m_EntityEnviroment is EntityEnvironmentState.OnWallLeft or EntityEnvironmentState.OnWallRight;
        }

        /// <summary>
        /// Player will lose controller of character for some time before getting it back.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is so if player is on wall that he cant stick again to the wall immediately after jumping
        /// </remarks>
        private IEnumerator EnableDisableMovement()
        {
            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Disable));

            yield return new WaitForSeconds(m_DisableMovementTimeLimit);

            Entity.Events.OnMoveChangeState?.Invoke(new MoveBehaviourStateParams(BehaviourState.Enable));
        }
    }
}
