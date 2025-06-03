using UnityEngine;

public class Car : Entity
{
    [SerializeField] Rigidbody rigidBody;

    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider rearLeft;
    [SerializeField] WheelCollider rearRight;

    [SerializeField] float motorForce = 1500f;
    [SerializeField] float maxSteerAngle = 30f;
    bool canMove = false;
    Transform exitPoint;

    public bool getCanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }
    public Transform getExitPoint
    {
        set { exitPoint = value; }
        get { return exitPoint; }
    }

    public void Movement()
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
        UpdateSingleWheel(frontLeft, frontLeft.gameObject.transform);
        UpdateSingleWheel(frontRight, frontRight.gameObject.transform);
        UpdateSingleWheel(rearLeft, rearLeft.gameObject.transform);
        UpdateSingleWheel(rearRight, rearRight.gameObject.transform);

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
