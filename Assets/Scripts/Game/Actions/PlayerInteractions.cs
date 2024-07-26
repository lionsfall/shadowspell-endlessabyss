namespace Dogabeey
{
    [System.Serializable]
    public class CastProjectileToCreature : PlayerInteraction
    {
        public Projectile projectile;

        public override void Invoke(Player player, Creature creature)
        {
            projectile.FireStandaloneProjectile(player, creature);
        }
    }
}
