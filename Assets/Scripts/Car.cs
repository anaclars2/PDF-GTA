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
    [SerializeField] Transform exitPoint;
    Vector3 exitOffset = new Vector3(-2f, 0f, 0f);

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

    private void Update()
    {
        if (exitPoint != null && canMove)
        {
            Vector3 offset = transform.TransformDirection(new Vector3(exitOffset.x, 0f, exitOffset.z));
            Vector3 desiredPosition = transform.position + offset;
            desiredPosition.y = exitPoint.position.y;
            exitPoint.position = desiredPosition;
        }
    }

    private void FixedUpdate()
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

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeft.gameObject.transform);
        UpdateSingleWheel(frontRight, frontRight.gameObject.transform);
        UpdateSingleWheel(rearLeft, rearLeft.gameObject.transform);
        UpdateSingleWheel(rearRight, rearRight.gameObject.transform);
    }

    private void UpdateSingleWheel(WheelCollider collider, Transform wheelTransform)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
