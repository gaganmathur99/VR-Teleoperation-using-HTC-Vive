using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gripper_R : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_minRange = 0.001f;
    public float m_maxRange = 0.001f;
    public float m_velocity = 0.001f;
    public float forceMult = 100;
    private Rigidbody rb;
    public KeyCode moveUp;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x<=m_minRange && transform.position.x >= m_maxRange)
        {
            Debug.Log("asd");
            if (Input.GetKeyDown(moveUp))
                rb.velocity = transform.forward*Time.deltaTime*forceMult;
        }
        
    }
}
