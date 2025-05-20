using UnityEngine;

public class Car : Entity
{
    [SerializeField] Rigidbody rigidBody;

    public override void Movement()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        direction = new Vector3();
    }

    public void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddForce(transform.forward * speed * Time.deltaTime);
        }
        
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            rigidBody.AddForce(transform.forward * speed * Time.deltaTime);
            transform.Rotate(0f, 1 * Time.deltaTime, 0f);
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            rigidBody.AddForce(transform.forward * speed * Time.deltaTime);
            transform.Rotate(0f, -1 * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rigidBody.AddForce(-1 * transform.forward * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            rigidBody.AddForce(-1 * transform.forward * speed * Time.deltaTime);
            transform.Rotate(0f, 1 * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            rigidBody.AddForce(-1 * transform.forward * speed * Time.deltaTime);
            transform.Rotate(0f, -1 * Time.deltaTime, 0f);
        }
    }
}
