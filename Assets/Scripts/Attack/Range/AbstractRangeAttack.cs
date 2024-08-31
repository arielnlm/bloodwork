using System;
using System.Collections.Generic;
using BloodWork.Attack.Range.Bullets;
using UnityEngine;
using UnityEngine.Serialization;

namespace BloodWork.Attack.Range
{
    public abstract class AbstractRangeAttack : AbstractAttack
    {
        [SerializeField] private AbstractAmmo m_ObjectToPool;
        [SerializeField] private int m_AmoutToPool;

        private List<AbstractAmmo> m_PooledAmmo;

        private void Start()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            m_PooledAmmo = new List<AbstractAmmo>();
            for (int i = 0; i < m_AmoutToPool; i++)
            {
                AbstractAmmo tmp = Instantiate(m_ObjectToPool);
                tmp.gameObject.SetActive(false);
                m_PooledAmmo.Add(tmp);
            }
        }

        protected AbstractAmmo GetPooledAmmo()
        {
            for (int i = 0; i < m_AmoutToPool; i++)
            {
                if (!m_PooledAmmo[i].gameObject.activeInHierarchy)
                    return m_PooledAmmo[i];
            }

            return null;
        }
    }
}