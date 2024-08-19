using BloodWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BloodWork.Entity
{
    public class Gravity
    {
        private readonly Dictionary<Priority, List<float>>               m_GravityMap;
        private readonly Dictionary<int, (Priority priority, int index)> m_IndexMap; // for removing gravity with id
        private readonly Rigidbody2D                                     m_Rigidbody;

        public Gravity(Rigidbody2D rigidbody)
        {
            m_Rigidbody = rigidbody;
            
            m_GravityMap = new Dictionary<Priority, List<float>>();
            m_IndexMap   = new Dictionary<int, (Priority priority, int index)>();

            foreach (Priority priority in GetPriorities())
                m_GravityMap.Add(priority, new List<float>());

            Add(Priority.Low, rigidbody.gravityScale, 0);
        }

        private Gravity Add(Priority priority, float gravity, int id)
        {
            List<float> priorityList = m_GravityMap[priority];
            
            m_IndexMap.Add(id, (priority, priorityList.Count));
            priorityList.Add(gravity);

            m_Rigidbody.gravityScale = Get();
            
            return this;
        }

        private Gravity Remove(int id)
        {
            (Priority priority, int index) = m_IndexMap[id];

            m_GravityMap[priority].RemoveAt(index);
            m_IndexMap.Remove(id);
            
            m_Rigidbody.gravityScale = Get();

            return this;
        }

        public float Get()
        {
            foreach (Priority priority in GetPriorities())
            {
                if (!m_GravityMap[priority].Any())
                    continue;

                return m_GravityMap[priority].Last();
            }

            throw new Exception("Gravity map can't be empty");
        }

        private IEnumerable<Priority> GetPriorities()
        {
            return ((Priority[])Enum.GetValues(typeof(Priority))).Reverse();
        }

        public static Gravity operator+(Gravity gravity, (Priority priority, float gravity, int id) item)
        {
            return gravity.Add(item.priority, item.gravity, item.id);
        }

        public static Gravity operator-(Gravity gravity, int id)
        {
            return gravity.Remove(id);
        }
    } 
}
