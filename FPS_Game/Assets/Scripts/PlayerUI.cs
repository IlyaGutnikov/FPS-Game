using UnityEngine;

public class PlayerUI : MonoBehaviour {

   [SerializeField]
    RectTransform fuelFill;

    private PlayerController controller;

    public void SetPLayerController(PlayerController _controller)
    {
        controller = _controller;
    }

    void Update() {

        if (controller != null)
        {
            SetFuelAmount(controller.GetFuelAmount());
        }
    }

    void SetFuelAmount(float _amount) {

        fuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
