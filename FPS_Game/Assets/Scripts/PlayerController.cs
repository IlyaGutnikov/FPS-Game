using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Spring Settings")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;

    private ConfigurableJoint joint;

    void Start()
    {

        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);

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

        //Calculate rotation
        float _yRot = Input.GetAxisRaw("Mouse X");

        //ONLY FOR TURN AROUND
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        motor.Rotate(_rotation);


        //Calculate rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");

        //ONLY FOR TURN AROUND
        float _cameraRotationX = _xRot * lookSensitivity;

        motor.RotateCamera(_cameraRotationX);

        Vector3 _thrusterForce = Vector3.zero;

        //Apply the thruster force
        if (Input.GetButton("Jump"))
        {

            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);

        }
        else {

            SetJointSettings(jointSpring);
        }

        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring)
    {

        joint.yDrive = new JointDrive {
            mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce };
    }
}
