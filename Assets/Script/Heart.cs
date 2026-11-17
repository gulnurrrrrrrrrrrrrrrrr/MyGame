public class Heart : Hazard
{
    protected override void OnHitPlayer(Health player)
    {
        if (player == null || itemData == null)
            return;

        player.Heal(itemData.healAmount);
        player.PlayPickup();
    }
}