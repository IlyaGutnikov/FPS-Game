using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    //сделано так чтобы иметь возможность синхронизировать переменную
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;


    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();

    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _coll = GetComponent<Collider>();
        if (_coll != null)
        {
            _coll.enabled = true;
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {

        if (isDead) { return; }

        currentHealth -= _amount;
        Debug.Log(transform.name + " has health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider _coll = GetComponent<Collider>();
        if (_coll != null)
        {
            _coll.enabled = false;
        }

        isDead = true;
        Debug.Log(transform.name + " is dead");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {

        yield return new WaitForSeconds(GameManager.instanse.matchSettings.respawnDelay);
        Debug.Log(transform.name + " respawned");
        SetDefaults();
        Transform _startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _startPoint.position;
        transform.rotation = _startPoint.rotation;
    } 
}
