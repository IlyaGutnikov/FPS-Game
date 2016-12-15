using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;

    private PlayerMotor motor;

    void Start()
    {

        motor = GetComponent<PlayerMotor>();

    }

    void Update()
    {
        // Get movment
        float _xMovment = Input.GetAxisRaw("Horizontal");
        float _zMovment = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * _xMovment;
        Vector3 movVertical = transform.forward * _zMovment;

        //Нормализация говорит что длина векторов не важна => длина вектора всегда 1
        Vector3 _velocity = (movHorizontal + movVertical).normalized * speed;

        motor.Move(_velocity);
    }
}
