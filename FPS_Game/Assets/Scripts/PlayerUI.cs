using UnityEngine;

public class PlayerUI : MonoBehaviour {

   [SerializeField]
    RectTransform fuelFill;

    void SetFuelAmount(float _amount) {

        fuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
