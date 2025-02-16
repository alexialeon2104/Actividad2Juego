using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hit;
    public AudioSource miss;
    public TMPro.TextMeshPro scoreText;
    static int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }

    public static void Hit()
    {
        comboScore += 1;
       Instance.hit.Play();
    }

    public static void Miss()
    {
        comboScore -= 1;
        Instance.miss.Play();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}
