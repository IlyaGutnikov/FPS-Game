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

        if (currentWeapon.weaponFireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.weaponFireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");

            }
        }
    }


    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject hitEffect = (GameObject) Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(hitEffect, 2f);
    }

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();

    }

    [ClientRpc]
    void RpcDoShootEffect()
    {

        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    [Client]
    void Shoot()
    {

        if (!isLocalPlayer) {

            return;
        }

        CmdOnShoot();

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.weaponRange, mask))
        {
            //Debug.Log("We hit " + _hit.collider.name);
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShoot(_hit.collider.name, currentWeapon.weaponDamage);
            }

            CmdOnHit(_hit.point, _hit.normal);

        }
    }

    [Command]
    void CmdPlayerShoot(string _playerId, int _damage)
    {

        Debug.Log(_playerId + "has been shoot");
        Player _player = GameManager.GetPlayer(_playerId);
        _player.RpcTakeDamage(_damage);
    }
}
