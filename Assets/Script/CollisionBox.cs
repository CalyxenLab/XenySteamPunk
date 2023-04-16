using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealSpace3D;

public class CollisionBox : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public GameObject magnetHand;
    public bool isGrabbed = false;
    public bool isOntheForcefield = false;

    public GameObject forceFieldHeardCenter;
    public GameObject forceFieldSilenceCenter;

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

    private void Update()
    {
        DistanceWithHand();
        DistanceWithForceField();
    }

    public void DistanceWithHand()
    {
        if(MagnetSystemManager.instance.boxOnHand == true && isGrabbed)
        {
            isOntheForcefield = false;
            AudiometrieManager.instance.PlaySound();

            float dist = Vector3.Distance(magnetHand.transform.position, transform.position);
            //Debug.Log("Distance : " + dist);

            if(dist > 0.3 && dist < 1)
            {
                //Debug.Log("Box détachée, distance : " + dist);
                
                this.transform.position = Vector3.MoveTowards(transform.position, magnetHand.transform.position, 0.5f * Time.deltaTime);
            
            }else if(dist >= 1)
            {
                //Debug.Log("Box détachée, distance : " + dist);
                this.transform.position = Vector3.MoveTowards(transform.position, magnetHand.transform.position, 1f * Time.deltaTime);
            }
        }
    }

    public void DistanceWithForceField()
    {
        float distFromHand = Vector3.Distance(magnetHand.transform.position, transform.position);

        if(MagnetSystemManager.instance.boxOnHand == true && isGrabbed && distFromHand > 0.5)
        {
            float distFromHeardCenter = Vector3.Distance(forceFieldHeardCenter.transform.position, transform.position);
            float distFromSilenceCenter = Vector3.Distance(forceFieldSilenceCenter.transform.position, transform.position);

            if(distFromHeardCenter < 1.1 && isOntheForcefield == false)
            {
                Debug.Log("La box est dans le champ de force d'écoute");
                
                MagnetSystemManager.instance.boxOnHand = false;
                isGrabbed = false;
                Destroy(this.GetComponent("InteractableItem"));
                AudiometrieManager.instance.StopSound();

                AudiometrieManager.instance.audiofile_On = AudiometrieManager.instance.nextStep; // correspond a la ligne 17 dans notre fichier
                AudiometrieManager.instance.nextStep -= 1;
                Debug.Log("Next Step : " + AudiometrieManager.instance.nextStep);


                isOntheForcefield = true;
                Debug.Log("isOntheForcefield State : " + isOntheForcefield);
                this.GetComponent<RespawnBox>().RespawnBoxFunction();
            }

            if(distFromSilenceCenter < 1.1 && isOntheForcefield == false && distFromHand > 0.5)
            {
                Debug.Log("La box est dans le champ de force de silence");

                MagnetSystemManager.instance.boxOnHand = false;
                isGrabbed = false;
                Destroy(this.GetComponent("InteractableItem"));

                AudiometrieManager.instance.StopSound();

                AudiometrieManager.instance.audiofile_Off = AudiometrieManager.instance.nextStep;
                AudiometrieManager.instance.nextStep += 1;
                Debug.Log("Next Step : " + AudiometrieManager.instance.nextStep);

                isOntheForcefield = true;
                Debug.Log("isOntheForcefield State : " + isOntheForcefield);
                this.GetComponent<RespawnBox>().RespawnBoxFunction();
            }
        }
    }
}
