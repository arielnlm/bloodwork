using System.Collections;
using BloodWork.Entity;
using UnityEngine;

namespace BloodWork.Ability
{
    public abstract class AbstractAbility : EntityBehaviour
    {
        [SerializeField] protected float CooldownTimeLimit;

        protected bool m_IsOnCoolDown;

        protected IEnumerator Cooldown()
        {
            m_IsOnCoolDown = true;

            yield return new WaitForSeconds(CooldownTimeLimit);

            m_IsOnCoolDown = false;
        }

    }
}