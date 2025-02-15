using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance; //Para accedder desde otras clases
    public AudioSource audioSource; // Para escuchar la canción
    public float songDelayInSeconds;
    public Lane[] lanes;
    public double marginOfError; // en segundos
    public int inputDelayInMilliseconds;

    public string MidiFileLocation;
    public float noteTime; // Tiempo desde el spawn de la nota hasta que la presiona el jugador
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    { 
        get { 
            return noteTapY - (noteSpawnY - noteTapY); 
            }
    } // La nota se despawnea cuando llega a la posición de tap
    public static MidiFile midiFile; // Carga el archivo midi en la memoria RAM


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite()
    { using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + MidiFileLocation))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();

                }
                
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + MidiFileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.SetTimeStamps(array);
        }
        Invoke(nameof(StartSong), songDelayInSeconds);

    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
