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

        [Header("Essence Drop")]
        public EssenceController essencePrefab;
        public Vector3 dropOffset;
        [Range(0, 1)]
        public float essenceDropRate;
        public List<Essence.EssenceDrop> dropPool;

        internal NavMeshAgent agent;

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

        public abstract void AIUpdate();
    }
}