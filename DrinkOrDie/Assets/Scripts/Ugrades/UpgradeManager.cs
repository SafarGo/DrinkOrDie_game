using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.ShootSpeed:
                player.AddShootSpeed(upgrade.value);
                break;

            case UpgradeType.Health:
                player.AddHealth(upgrade.value);
                break;

            case UpgradeType.MoveSpeed:
                player.AddMoveSpeed(upgrade.value);
                break;

            case UpgradeType.MeleeDamage:
                player.AddMeleeDamage(upgrade.value);
                break;

            case UpgradeType.ProjectileType1:
                player.SetProjectileType(0);
                break;

            case UpgradeType.ProjectileType2:
                player.SetProjectileType(1);
                break;

            case UpgradeType.ProjectileType3:
                player.SetProjectileType(2);
                break;
        }
    }
}