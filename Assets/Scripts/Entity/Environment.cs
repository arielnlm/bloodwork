using System;
using System.Collections.Generic;
using BloodWork.Commons.Types;

namespace BloodWork.Entity
{
    public class Environment
    {
        private readonly IEnumerable<EntityEnvironmentState>     m_EnvironmentStates;
        private readonly Dictionary<int, EntityEnvironmentState> m_IdentifierMap;
        private readonly Dictionary<EntityEnvironmentState, int> m_EnvironmentMap;
        private          EntityEnvironmentValue                  m_EntityEnvironmentValue;
        
        public Environment()
        {
            m_EnvironmentStates      = (EntityEnvironmentState[])Enum.GetValues(typeof(EntityEnvironmentState));
            m_EnvironmentMap         = new Dictionary<EntityEnvironmentState, int>();
            m_EntityEnvironmentValue = new EntityEnvironmentValue(EntityEnvironmentState.Neutral);

            foreach (var environmentState in m_EnvironmentStates)
                m_EnvironmentMap[environmentState] = 0;
        }

        private Environment Add(int id, EntityEnvironmentState entityEnvironmentState)
        {
            m_EnvironmentMap[entityEnvironmentState] += 1;

            m_IdentifierMap.Add(id, entityEnvironmentState);

            return this;
        }

        private Environment Remove(int id)
        {
            var entityEnvironmentState = m_IdentifierMap[id];
            
            m_IdentifierMap.Remove(id);

            if (m_EnvironmentMap[entityEnvironmentState] == 0)
                throw new Exception("Platform environment map cannot have values below zero.");

            m_EnvironmentMap[entityEnvironmentState] -= 1;

            return this;
        }

        public EntityEnvironmentValue Get()
        {
            m_EntityEnvironmentValue.Reset();
            
            foreach (var environmentState in m_EnvironmentStates)
                if (m_EnvironmentMap[environmentState] > 0)
                    m_EntityEnvironmentValue += environmentState;
            
            return m_EntityEnvironmentValue;
        }

        public static Environment operator+(Environment environment, (int id, EntityEnvironmentState environmentState) item)
        {
            return environment.Add(item.id, item.environmentState);
        }

        public static Environment operator-(Environment environment, int id)
        {
            return environment.Remove(id);
        }
    }
}
