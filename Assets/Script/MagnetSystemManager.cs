using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplePhysicsToolkit;

public class MagnetSystemManager : MonoBehaviour
{
    public static MagnetSystemManager instance;
    public bool boxOnHand = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
}
