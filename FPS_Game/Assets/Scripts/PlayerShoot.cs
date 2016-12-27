using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    // Use this for initialization
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera");
            this.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot() {

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.weaponRange, mask))
        {
            //Debug.Log("We hit " + _hit.collider.name);
            if (_hit.collider.tag == PLAYER_TAG) {

                CmdPlayerShoot(_hit.collider.name, weapon.weaponDamage);
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
