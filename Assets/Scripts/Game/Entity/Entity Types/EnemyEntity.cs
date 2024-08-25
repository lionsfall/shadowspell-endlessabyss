using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dogabeey
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyEntity : Creature
    {
        public override bool IsPlayer => false;

        [FoldoutGroup("Essence Drop")]
        public EssenceController essencePrefab;
        [FoldoutGroup("Essence Drop")]
        public Vector3 dropOffset;
        [Range(0, 1)]
        [FoldoutGroup("Essence Drop")]
        public float essenceDropRate;
        [FoldoutGroup("Essence Drop")]
        public List<Essence.EssenceDrop> dropPool;

        internal NavMeshAgent agent;
        internal EnemyState enemyState;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
        }

        public override void OnDeath(Entity killer)
        {
            base.OnDeath(killer);
            if(UnityEngine.Random.value <= essenceDropRate)
            {
                List<Essence> weightedPool = new List<Essence>();
                // We add the essence to the pool based on their drop rate. More of the same elements = more chance to drop.
                foreach (Essence.EssenceDrop drop in dropPool)
                {
                    for (int i = 0; i < drop.dropWeight; i++)
                    {
                        weightedPool.Add(drop.essence);
                    }
                }
                weightedPool[UnityEngine.Random.Range(0, weightedPool.Count)].DropEssence(essencePrefab, transform.position);
            }
        }

        protected virtual void Update()
        {
            AIUpdate();
        }

        /// <summary>
        /// This is where the AI logic is implemented. It usually makes use of the NavMeshAgent to move the enemy based on player location or something else. 
        /// You should also set the State and enemyState variables here based on the enemy's current action. enemyState is just there to help you make the AI logic easier.
        /// State is used to trigger the animations and other events.
        /// </summary>
        public abstract void AIUpdate();
    }

    public enum EnemyState
    {
        Idle,
        Flee,
        Chase,
        Attack
    }
}