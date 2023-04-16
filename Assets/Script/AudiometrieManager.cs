using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealSpace3D;

public class AudiometrieManager : MonoBehaviour
{
    public static AudiometrieManager instance;

    public GameObject gameObjectAudioSource;
    RealSpace3D_AudioSource myAudioSource;

    public int indexSoundToPlay = 0;

    public int nextStep = 4;
    public int score1;
    public int score2;
    public int audiofile_On;
    public int audiofile_Off;


    float[,] calibrationArray = new float[,] //simulation du fichier de calibration
    {
        { 0.1f,0.2f,0.3f,0.4f,0.5f,0.6f,0.7f,0.8f,0.9f,1f}, 
        { 0.1f,0.6f,0.9f,0.1f,0.5f,03f,0.8f,1f,0.4f,0.5f}, 
        { 0.5f,0.4f,1f,0.8f,0.5f,0.1f,0.9f,6f,0.1f,0.7f}
    };

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

    private void Start()
    {
        myAudioSource = gameObjectAudioSource.GetComponent<RealSpace3D.RealSpace3D_AudioSource>();       
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        ScoringAudiometrie();
    }

    public void StopSound()
    {
        myAudioSource.rs3d_StopAllSounds();
    }

    public void PlaySound()
    {
        if(myAudioSource.rs3d_IsPlaying() == false)
            {
                myAudioSource.rs3d_AdjustVolume(calibrationArray[indexSoundToPlay,nextStep]);
                myAudioSource.rs3d_PlaySound(indexSoundToPlay);
            }
    }


    public void ScoringAudiometrie()
    {
        if(score2 == 0)
        {
            if(audiofile_On != 0 && audiofile_Off != 0 && score1 == 0)
            {
                score1 = audiofile_On;
                nextStep = audiofile_Off;
                audiofile_On = 0;
                audiofile_Off = 0;
                Debug.Log("Score1 : " + score1);
            }

            if(audiofile_On != 0 && audiofile_Off != 0 && score1 != 0)
            {
                score2 = audiofile_On;
                nextStep = audiofile_Off;
                audiofile_On = 0;
                audiofile_Off = 0;
                Debug.Log("Score2 : " + score2);
            }

            if(score1 != score2 && score2 != 0)
            {
                score1 = 0;
                score2 = 0;
                Debug.Log("reset des scores");
                
            }else if(score1 == score2 && score1 != 0){
                Debug.Log("Son Suivant");
                indexSoundToPlay += 1;
            }
        } 
    }
}
