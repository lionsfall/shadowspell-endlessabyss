using System.Linq;
using UnityEngine;

namespace Dogabeey
{
    [System.Serializable]
    public abstract class CreaturePicker
    {

        public abstract Creature GetCreature(Creature sourceEntity);
    }
    [System.Serializable]
    public class PickFarthestWithinRange : CreaturePicker
    {
        public float range;
        public override Creature GetCreature(Creature sourceEntity)
        {
            float range = this.range > 0 ? this.range : sourceEntity.Range;
            Creature farthestEntity = null;
            float farthestDistance = 0;
            Creature[] entities = Physics.OverlapSphere(sourceEntity.transform.position, range).Select(x => x.GetComponent<Creature>()).Where(x => x != null).ToArray();
            foreach (var entity in entities)
            {
                if (entity == sourceEntity)
                    continue;
                float distance = Vector3.Distance(entity.transform.position, sourceEntity.transform.position);
                if (distance <= range && distance > farthestDistance)
                {
                    farthestEntity = entity;
                    farthestDistance = distance;
                }
            }
            return farthestEntity;
        }
    }
    [System.Serializable]
    public class PickRandomWithinRange : CreaturePicker
    {
        public float range;
        public override Creature GetCreature(Creature sourceEntity)
        {
            float range = this.range > 0 ? this.range : sourceEntity.Range;
            Creature[] entities = Physics.OverlapSphere(sourceEntity.transform.position, range)
                .Select(x => x.GetComponent<Creature>())
                .Where(x => x != null && x != sourceEntity).ToArray();
            Random.InitState(System.DateTime.Now.Millisecond * sourceEntity.transform.GetHashCode());
            return entities.Length > 0 ? entities[Random.Range(0, entities.Length)] : null;
        }
    }
    [System.Serializable]
    public class PickClosestWithinRange : CreaturePicker
    {
        public float range;
        public override Creature GetCreature(Creature sourceEntity)
        {
            float range = this.range > 0 ? this.range : sourceEntity.Range;
            Creature closestEntity = null;
            float closestDistance = range;
            Creature[] entities = Physics.OverlapSphere(sourceEntity.transform.position, range).Select(x => x.GetComponent<Creature>()).Where(x => x != null).ToArray();
            foreach (var entity in entities)
            {
                if (entity == sourceEntity)
                    continue;
                float distance = Vector3.Distance(entity.transform.position, sourceEntity.transform.position);
                if (distance <= range && distance < closestDistance)
                {
                    closestEntity = entity;
                    closestDistance = distance;
                }
            }
            return closestEntity;
        }
    }
    [System.Serializable]
    public class PickWithLowestHealthInRange : CreaturePicker
    {
        public float range;
        public override Creature GetCreature(Creature sourceEntity)
        {
            float range = this.range > 0 ? this.range : sourceEntity.Range;
            Creature lowestHealthEntity = null;
            float lowestHealth = float.MaxValue;
            Creature[] entities = Physics.OverlapSphere(sourceEntity.transform.position, range).Select(x => x.GetComponent<Creature>()).Where(x => x != null).ToArray();
            foreach (var entity in entities)
            {
                if (entity == sourceEntity)
                    continue;
                if (entity.CurrentHealth < lowestHealth)
                {
                    lowestHealthEntity = entity;
                    lowestHealth = entity.CurrentHealth;
                }
            }
            return lowestHealthEntity;
        }
    }
    [System.Serializable]
    public class PickWithHighestHealthInRange : CreaturePicker
    {
        public float range;
        public override Creature GetCreature(Creature sourceEntity)
        {
            float range = this.range > 0 ? this.range : sourceEntity.Range;
            Creature highestHealthEntity = null;
            float highestHealth = 0;
            Creature[] entities = Physics.OverlapSphere(sourceEntity.transform.position, range).Select(x => x.GetComponent<Creature>()).Where(x => x != null).ToArray();
            foreach (var entity in entities)
            {
                if (entity == sourceEntity)
                    continue;
                if (entity.CurrentHealth > highestHealth)
                {
                    highestHealthEntity = entity;
                    highestHealth = entity.CurrentHealth;
                }
            }
            return highestHealthEntity;
        }
    }
}
