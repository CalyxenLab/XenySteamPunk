using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBox : MonoBehaviour
{
    Vector3 initialBoxPosition;
    Rigidbody m_Rigidbody;

    public void Start()
    {
        initialBoxPosition = this.transform.position;
        //Debug.Log("Initial Position : " + initialBoxPosition);
    }

    public void RespawnBoxFunction()
    {
        var newBox = Instantiate(this.gameObject);
        newBox.transform.position = initialBoxPosition;
        m_Rigidbody = newBox.GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }
}
