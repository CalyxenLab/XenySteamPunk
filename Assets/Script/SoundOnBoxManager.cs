using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealSpace3D;

public class SoundOnBoxManager : MonoBehaviour
{
    RealSpace3D_AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = this.GetComponent<RealSpace3D.RealSpace3D_AudioSource>();
    }

    public void SoundOnBox()
    {
        if(myAudioSource.rs3d_IsPlaying() == false)
        {
            myAudioSource.rs3d_PlaySound();
        }   
    }
}
