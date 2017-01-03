using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    // Use this for initialization
    void Start()
    {

        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }

    // Update is called once per frame
    void Update()
    {

        currentWeapon = weaponManager.GetCurrentWeapon();

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot() {

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.weaponRange, mask))
        {
            //Debug.Log("We hit " + _hit.collider.name);
            if (_hit.collider.tag == PLAYER_TAG) {

                CmdPlayerShoot(_hit.collider.name, currentWeapon.weaponDamage);
            }

        }
    }

    [Command]
    void CmdPlayerShoot(string _playerId, int _damage) {

        Debug.Log(_playerId + "has been shoot");
        Player _player = GameManager.GetPlayer(_playerId);
        _player.RpcTakeDamage(_damage);
    }
}
