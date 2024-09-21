using System.Collections;
using BloodWork.Entity;
using UnityEngine;

namespace BloodWork.Ability
{
    public abstract class AbstractAbility : EntityBehaviour
    {
        [SerializeField] protected float CooldownTimeLimit;

        protected bool IsOnCoolDown;

        protected IEnumerator Cooldown()
        {
            IsOnCoolDown = true;

            yield return new WaitForSeconds(CooldownTimeLimit);

            IsOnCoolDown = false;
        }

    }
}