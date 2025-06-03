using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 2f, -5f);

    [Header("Rotation Settings")]
    [SerializeField] float sensitivityX = 3f;
    [SerializeField] float sensitivityY = 2f;
    [SerializeField] float minY = -20f;
    [SerializeField] float maxY = 60f;

    float currentX = 0f;
    float currentY = 20f;

    void LateUpdate()
    {
        if (target == null) return;

        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        // calculando rotacao e posicao de acordo com o mouse e alvo
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = desiredPosition;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}