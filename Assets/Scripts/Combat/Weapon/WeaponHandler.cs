using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponHitbox;

    public void EnableWeapon() => weaponHitbox.SetActive(true);

    public void DisableWeapon() => weaponHitbox.SetActive(false);
}
