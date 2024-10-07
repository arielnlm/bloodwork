using System;
using System.Collections.Generic;
using BloodWork.Commons;
using UnityEngine;
using static System.Single;

namespace BloodWork.Entity
{
    public class Environment
    {
        private readonly Dictionary<EntityPlatformState, int>                    m_PlatformMap;
        private readonly Dictionary<int, EntityPlatformState>                    m_IdentifierMap;
        private readonly Rigidbody2D                                             m_Rigidbody;
        private readonly IEnumerable<EntityPlatformState>                        m_PlatformStates;
        private readonly Dictionary<EntityPlatformState, EntityEnvironmentState> m_EnvironmentStateMap;

        public Environment(Rigidbody2D rigidbody)
        {
            m_Rigidbody      = rigidbody;
            m_PlatformStates = (EntityPlatformState[])Enum.GetValues(typeof(EntityPlatformState));

            m_PlatformMap         = new Dictionary<EntityPlatformState, int>();
            m_IdentifierMap       = new Dictionary<int, EntityPlatformState>();
            m_EnvironmentStateMap = new Dictionary<EntityPlatformState, EntityEnvironmentState>();

            foreach (var platformState in m_PlatformStates)
                m_PlatformMap[platformState] = 0;

            InitialiseEnvironmentStateMap();
        }

        private void InitialiseEnvironmentStateMap()
        {
            m_EnvironmentStateMap.Add(EntityPlatformState.OnGround,  EntityEnvironmentState.OnGround);
            m_EnvironmentStateMap.Add(EntityPlatformState.OnCeiling, EntityEnvironmentState.OnCeiling);
            m_EnvironmentStateMap.Add(EntityPlatformState.OnWallLeft,    EntityEnvironmentState.OnWallLeft);
            m_EnvironmentStateMap.Add(EntityPlatformState.OnWallRight,    EntityEnvironmentState.OnWallRight);
        }

        private Environment Add(int id, EntityPlatformState entityPlatformState)
        {
            m_PlatformMap[entityPlatformState] += 1;

            m_IdentifierMap.Add(id, entityPlatformState);

            return this;
        }

        private Environment Remove(int id)
        {
            var entityPlatformState = m_IdentifierMap[id];
            m_IdentifierMap.Remove(id);

            if (m_PlatformMap[entityPlatformState] == 0)
                throw new Exception("Platform environment map cannot have values below zero.");

            m_PlatformMap[entityPlatformState] -= 1;

            return this;
        }

        public EntityEnvironmentState Get()
        {
            foreach (var platformState in m_PlatformStates)
                if (m_PlatformMap[platformState] > 0)
                    return m_EnvironmentStateMap[platformState];

            return m_Rigidbody.velocity.y switch
                   {
                       > 0 => EntityEnvironmentState.Rising,
                       < 0 => EntityEnvironmentState.Falling,
                       0   => EntityEnvironmentState.Constant,
                       NaN => throw new Exception("Entity's velocity is NaN.")
                   };
        }

        public static Environment operator+(Environment environment, (int id, EntityPlatformState platformState) item)
        {
            return environment.Add(item.id, item.platformState);
        }

        public static Environment operator-(Environment environment, int id)
        {
            return environment.Remove(id);
        }
    }
}
