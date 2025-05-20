using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float sensibility;
    Vector3 direction;

    public virtual void Movement()
    {

    }
}
