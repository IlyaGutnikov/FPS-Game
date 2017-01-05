using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    // Use this for initialization
    void Start()
    {

        //Equip weapon
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {

        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {

        return currentGraphics;
    }

    void EquipWeapon(PlayerWeapon weapon)
    {

        currentWeapon = weapon;
        GameObject _weaponInstance = (GameObject)Instantiate(weapon.weaponGFX, weaponHolder.position, weaponHolder.rotation);
        _weaponInstance.transform.SetParent(weaponHolder);

        currentGraphics = _weaponInstance.GetComponent<WeaponGraphics>();

        if (currentGraphics == null) {

            Debug.LogError("No weapon graphics " + _weaponInstance.name);
        }

        if (isLocalPlayer)
        {
            _weaponInstance.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
