using UnityEngine;

public class Car : Entity
{
    [SerializeField] Rigidbody rigidBody;

    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    public float motorForce = 1500f;
    public float maxSteerAngle = 30f;

    public override void Movement()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        direction = new Vector3();
    }

    void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical") * motorForce;
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;

        // Direção
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;

        // Tração
        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        UpdateWheels();
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftTransform);
        UpdateSingleWheel(frontRight, frontRightTransform);
        UpdateSingleWheel(rearLeft, rearLeftTransform);
        UpdateSingleWheel(rearRight, rearRightTransform);

        Debug.Log("Estou entrando");
    }

    void UpdateSingleWheel(WheelCollider collider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        trans.position = pos;
        trans.rotation = rot;
    }
}
