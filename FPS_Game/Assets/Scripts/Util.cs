using UnityEngine;

public class Util {

    public static void SetLayerRecursevly(GameObject _obj, int _newLayer) {

        if (_obj == null) {

            return;
        }

        _obj.layer = _newLayer;
        foreach (Transform child in _obj.transform)
        {

            if (child == null) {

                continue;
            }

            SetLayerRecursevly(child.gameObject, _newLayer);

        }


    }

}
