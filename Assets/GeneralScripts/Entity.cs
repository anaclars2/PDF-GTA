using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float speed;
    [SerializeField] protected float sensibility;
    protected Vector3 direction;

    // public abstract void Movement();
}
