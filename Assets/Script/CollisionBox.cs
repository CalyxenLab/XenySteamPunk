using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public GameObject magnetHand;
    public bool isGrabbed = false;

    public GameObject forceFieldCenter;

    private void Start()
    {
       magnetHand = GameObject.FindWithTag("magnetHand");
    }

    void OnCollisionEnter(Collision collision) 
    {
       if (collision.gameObject.tag == "magnetHand" && MagnetSystemManager.instance.boxOnHand == false)
       {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            MagnetSystemManager.instance.boxOnHand = true;
            Debug.Log("Collision");
            isGrabbed = true;
       }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        DistanceWithHand();
        DistanceWithForceField();
    }

    public void DistanceWithHand()
    {
        if(MagnetSystemManager.instance.boxOnHand == true && isGrabbed)
        {
            float dist = Vector3.Distance(magnetHand.transform.position, transform.position);
            //Debug.Log("Distance : " + dist);

            if(dist > 0.3 && dist < 1)
            {
                Debug.Log("Box détachée, distance : " + dist);
                
                this.transform.position = Vector3.MoveTowards(transform.position, magnetHand.transform.position, 0.5f * Time.deltaTime);
            
            }else if(dist >= 1)
            {
                Debug.Log("Box détachée, distance : " + dist);
                this.transform.position = Vector3.MoveTowards(transform.position, magnetHand.transform.position, 1f * Time.deltaTime);
            }
        }
    }

    public void DistanceWithForceField()
    {
        if(MagnetSystemManager.instance.boxOnHand == true && isGrabbed)
        {
            float dist = Vector3.Distance(forceFieldCenter.transform.position, transform.position);
            //Debug.Log("Distance : " + dist);

            if(dist < 1.1)
            {
                Debug.Log("La box est dans le champ de force");
                MagnetSystemManager.instance.boxOnHand = false;
                isGrabbed = false;
                Destroy(this.GetComponent("InteractableItem"));
            }
        }
    }
}
