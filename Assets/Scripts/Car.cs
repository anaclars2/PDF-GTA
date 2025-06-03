using UnityEngine;

public class Car : MonoBehaviour
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

    void FixedUpdate()
    {
        if (canMove == false) { return; }

        float motor = Input.GetAxis("Vertical") * motorForce;
        float steer = Input.GetAxis("Horizontal") * maxSteerAngle;

        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;
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
    }

    void UpdateSingleWheel(WheelCollider collider, Transform wheelTransform)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
