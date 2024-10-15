using System;
using System.Collections.Generic;
using BloodWork.Commons.Types;
using UnityEngine;

namespace BloodWork.Entity
{
    public class EntityEnvironment
    {
        private readonly IEnumerable<EntityEnvironmentState>     m_EnvironmentStates;
        private readonly Dictionary<int, EntityEnvironmentState> m_IdentifierMap;
        private readonly Dictionary<EntityEnvironmentState, int> m_EnvironmentMap;
        private          EntityEnvironmentValue                  m_EntityEnvironmentValue;
        private          bool                                    m_IsChanged;
        
        public EntityEnvironment()
        {
            m_EnvironmentStates      = (EntityEnvironmentState[])Enum.GetValues(typeof(EntityEnvironmentState));
            m_IdentifierMap          = new Dictionary<int, EntityEnvironmentState>();
            m_EnvironmentMap         = new Dictionary<EntityEnvironmentState, int>();
            m_EntityEnvironmentValue = new EntityEnvironmentValue(EntityEnvironmentState.Neutral);
            m_IsChanged              = true;

            foreach (var environmentState in m_EnvironmentStates)
                m_EnvironmentMap[environmentState] = 0;
        }

        private EntityEnvironment Add(int id, EntityEnvironmentState entityEnvironmentState)
        {
            m_EnvironmentMap[entityEnvironmentState] += 1;
            m_IdentifierMap.Add(id, entityEnvironmentState);
            m_IsChanged = true;

            return this;
        }

        private EntityEnvironment Remove(int id)
        {
            var entityEnvironmentState = m_IdentifierMap[id];
            
            m_IdentifierMap.Remove(id);

            if (m_EnvironmentMap[entityEnvironmentState] == 0)
                throw new Exception("Platform environment map cannot have values below zero.");

            m_EnvironmentMap[entityEnvironmentState] -= 1;
            m_IsChanged = true;

            return this;
        }
        
        public EntityEnvironmentValue Get()
        {
            CalculateValue();
            
            return m_EntityEnvironmentValue;
        }

        public bool Is(EntityEnvironmentFlag entityEnvironmentFlag)
        {
            CalculateValue();
            
            return m_EntityEnvironmentValue == entityEnvironmentFlag;
        }

        private void CalculateValue()
        {
            if (!m_IsChanged)
                return;
            
            m_EntityEnvironmentValue.Reset();
            m_IsChanged = false;
            
            foreach (var environmentState in m_EnvironmentStates)
                if (m_EnvironmentMap[environmentState] > 0)
                    m_EntityEnvironmentValue += environmentState;
        }

        public static EntityEnvironment operator+(EntityEnvironment entityEnvironment, (int id, EntityEnvironmentState environmentState) item)
        {
            return entityEnvironment.Add(item.id, item.environmentState);
        }

        public static EntityEnvironment operator-(EntityEnvironment entityEnvironment, int id)
        {
            return entityEnvironment.Remove(id);
        }
    }
}
