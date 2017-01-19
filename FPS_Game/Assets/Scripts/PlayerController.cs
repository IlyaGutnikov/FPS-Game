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

    [SerializeField]
    private float fuelBurnSpeed = 1f;
    [SerializeField]
    private float fuelRegenSpeed = 0.3f;
    private float fuelAmount = 1f;

    [SerializeField]
    private LayerMask envMask;

    [Header("Spring Settings")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;

    private ConfigurableJoint joint;

    public Animator animator;

    public float GetFuelAmount() {

        return fuelAmount;
    }

    void Start()
    {

        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);

    }

    void Update()
    {
        RaycastHit _hit;
        //Set target position for the spring
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f,envMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y,0f);
        } else {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        // Get movment
        float _xMovment = Input.GetAxis("Horizontal");
        float _zMovment = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * _xMovment;
        Vector3 movVertical = transform.forward * _zMovment;

        //Нормализация говорит что длина векторов не важна => длина вектора всегда 1
        Vector3 _velocity = (movHorizontal + movVertical) * speed;

        animator.SetFloat("forwardVelocity",_zMovment);

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
        if (Input.GetButton("Jump") && (fuelAmount > 0f))
        {
            fuelAmount -= fuelBurnSpeed * Time.deltaTime;

            if (fuelAmount >= 0.01f) {

                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
        }
        else {

            fuelAmount += fuelRegenSpeed * Time.deltaTime;
       
            SetJointSettings(jointSpring);
        }

        fuelAmount = Mathf.Clamp(fuelAmount, 0f, 1f);

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
