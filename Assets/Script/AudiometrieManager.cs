using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using System.IO;
using System;


public class AudiometrieManager : MonoBehaviour
{
    public static AudiometrieManager instance;

    //public GameObject gameObjectAudioSource;
   

    public int indexSoundToPlay;

    public int nextStep = 4;
    public int score1;
    public int score2;
    public int audiofile_On;
    public int audiofile_Off;

    public Text indexSoundToPlayText;

    public Text nextStepText;
    public Text score1Text;
    public Text score2Text;

    public LineChart chart;
    LineDataSet set = new LineDataSet();
    //LineDataSet setRightEar = new LineDataSet();

    List<int> randomArray = new List<int>();

    public float[,] calibrationArray = new float[,] //simulation du fichier de calibration
    {
        { 0.1f,0.2f,0.3f,0.4f,0.5f,0.6f,0.7f,0.8f,0.9f,1f, 0, 0}, // les deux derniers index correspondent aux score1 et score2
        { 0.1f,0.6f,0.9f,0.1f,0.5f,03f,0.8f,1f,0.4f,0.5f, 0, 0}, // à 0 par défaut en début d'experience
        { 0.5f,0.4f,1f,0.8f,0.5f,0.1f,0.9f,6f,0.1f,0.7f, 0, 0},
        { 0.5f,0.4f,1f,0.8f,0.5f,0.1f,0.9f,6f,0.1f,0.7f, 0, 0},
        { 0.5f,0.4f,1f,0.8f,0.5f,0.1f,0.9f,6f,0.1f,0.7f, 0, 0}
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
            
    }

    private void Update()
    {
        ScoringAudiometrie();
    }

    public void StopSound()
    {
       
    }

    public void PlaySound()
    {
        
    }

    public void CreateDataSet()
    {
        Debug.Log("IndexSoundToPlay Chart : " + indexSoundToPlay);
        // Create data set for entries
        //LineDataSet set = new LineDataSet();
        // Add entries to data set
        set.AddEntry(new LineEntry(indexSoundToPlay, calibrationArray[indexSoundToPlay,nextStep]));
            
        // Configure line
        set.LineColor = Color.red;
        set.LineThickness = 4;
        set.UseBezier = false;
        // Add data set to chart data
        chart.GetChartData().DataSets.Add(set);
        // Refresh chart after data change
        chart.SetDirty();
    }


    public void ScoringAudiometrie()
    {
        // if(score2 == 0)
        // {
        //     if(audiofile_On != 0 && audiofile_Off != 0 && score1 == 0)
        //     {
        //         score1 = audiofile_On;
        //         nextStep = audiofile_Off;
        //         audiofile_On = 0;
        //         audiofile_Off = 0;
        //         Debug.Log("Score1 : " + score1);
        //     }

        //     if(audiofile_On != 0 && audiofile_Off != 0 && score1 != 0)
        //     {
        //         score2 = audiofile_On;
        //         nextStep = audiofile_Off;
        //         audiofile_On = 0;
        //         audiofile_Off = 0;
        //         Debug.Log("Score2 : " + score2);
        //     }

        //     if(score1 != score2 && score2 != 0)
        //     {
        //         score1 = 0;
        //         score2 = 0;
        //         Debug.Log("reset des scores");
                
        //     }else if(score1 == score2 && score1 != 0){
        //         Debug.Log("Son Suivant");
        //         indexSoundToPlay += 1;
        //     }
        // } // Ce code marche avec un seul son, les lignes dessous servent d'update pour enregistrer les scores de plusieurs sons différent en même temps


        if(calibrationArray[indexSoundToPlay,11] == 0)
        {
            if(audiofile_On != 0 && audiofile_Off != 0 && calibrationArray[indexSoundToPlay,10] == 0)
            {
                score1 = audiofile_On;
                calibrationArray[indexSoundToPlay,10] = audiofile_On;

                nextStep = audiofile_Off;
                audiofile_On = 0;
                audiofile_Off = 0;
                score1Text.text = "Score 1 : " + calibrationArray[indexSoundToPlay,10].ToString();

                CreateDataSet();

                Debug.Log("calibration array length " + calibrationArray.Length);
                // génère un random entre 0 et le nombre de sons (ex : 5) qui n'a pas de score 2, 
                for (int i = 0; i < calibrationArray.GetLength(0); i++)
                {
                    if(calibrationArray[i,11] == 0)
                    {
                        randomArray.Add(i);
                        Debug.Log("Element added in random array");
                    }
                }

                System.Random rnd = new System.Random();
                int randIndex = rnd.Next(randomArray.Count);
                indexSoundToPlay =  randomArray[randIndex];

                randomArray.Clear();
                Debug.Log("randomArray length : " + randomArray.Count);

                indexSoundToPlayText.text = "Son n° " +  indexSoundToPlay;
                Debug.Log("indexSoundToPlay value : " + indexSoundToPlay);
            }

            if(audiofile_On != 0 && audiofile_Off != 0 && calibrationArray[indexSoundToPlay,10] != 0)
            {
                score2 = audiofile_On;
                calibrationArray[indexSoundToPlay,11] = audiofile_On;

                score2Text.text = "Score 2 : " + calibrationArray[indexSoundToPlay,10].ToString();
                nextStep = audiofile_Off;
                audiofile_On = 0;
                audiofile_Off = 0;
                
                CreateDataSet();

                for (int i = 0; i < 5; i++)
                {
                    if(calibrationArray[i,11] == 0)
                    {
                        randomArray.Add(i);
                        Debug.Log("Element added in random array");
                    }
                }

                System.Random rnd = new System.Random();
                int randIndex = rnd.Next(randomArray.Count);
                indexSoundToPlay =  randomArray[randIndex];
                randomArray.Clear();
                Debug.Log("randomArray length : " + randomArray.Count);

                indexSoundToPlayText.text = "Son n° " +  indexSoundToPlay;
                Debug.Log("indexSoundToPlay value : " + indexSoundToPlay);
            }
            
            // Si score1 != de score2
            if(calibrationArray[indexSoundToPlay,10] != calibrationArray[indexSoundToPlay,11] && calibrationArray[indexSoundToPlay,11] != 0)
            {
                score1 = 0;
                score2 = 0;
                calibrationArray[indexSoundToPlay,10] = 0;
                calibrationArray[indexSoundToPlay,11] = 0;
                Debug.Log("reset des scores");
                
            }else if(calibrationArray[indexSoundToPlay,10] == calibrationArray[indexSoundToPlay,11] && calibrationArray[indexSoundToPlay,10] != 0){ // Si score1 =  score2
                Debug.Log("Scores identiques, son suivant");
            }
        }
    }
}
