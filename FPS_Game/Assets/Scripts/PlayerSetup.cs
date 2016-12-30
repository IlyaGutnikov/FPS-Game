using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance; 

    Camera sceneCamera;

    // Use this for initialization
    void Start()
    {

        if (!isLocalPlayer)
        {
            AssignRemoteLayer();
            DisableComponents();
        }
        else {
            sceneCamera = Camera.main;

            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //disab;e player graphics for LP
            SetLayerRecurservly(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Create PlayerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }

        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        string _netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        base.OnStartClient();
        GameManager.RegisterPlayer(_netId, _player);
    }

    void AssignRemoteLayer() {

        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents() {

        for (int i = 0; i < componentsToDisable.Length; i++)
        {

            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);

    }

    void SetLayerRecurservly(GameObject obj, int newLayer) {

        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {

            SetLayerRecurservly(child.gameObject, newLayer);

        }

    }
}
