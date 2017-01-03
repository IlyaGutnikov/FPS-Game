using UnityEngine;

[System.Serializable]
public class PlayerWeapon  {

    public string weaponName = "Glock";
    public int weaponDamage = 10;
    public float weaponRange = 200f;
    public float weaponFireRate = 0f; // 0f means a single-fire weapon

    public GameObject weaponGFX;
}
