using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hit;
    public AudioSource miss;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro multiplierText;
    static int comboScore;
    static int hitStreak;
    static float scoreMultiplier = 1f;

    public Color normalColor = Color.white;
    public Color highMultiplierColor = Color.blue;
    public Color veryHighMultiplierColor = Color.magenta;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
        hitStreak = 0;
    }

    public static void Hit()
    {
        hitStreak++;
        scoreMultiplier = 1f + (hitStreak / 10f);
        comboScore += Mathf.RoundToInt(1 * scoreMultiplier);
       Instance.hit.Play();
    }

    public static void Miss()
    {
        comboScore -= 1;
        hitStreak = 0;
        scoreMultiplier = 1f;
        Instance.miss.Play();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = comboScore.ToString();
        multiplierText.text = "x" + scoreMultiplier.ToString("F1");

        // Cambiar el color basado en el multiplicador
        if (scoreMultiplier >= 2f)
        {
            multiplierText.color = highMultiplierColor;
        }
        else if (scoreMultiplier >= 4f)
        {
            multiplierText.color = veryHighMultiplierColor;
        }
        else
        {
            multiplierText.color = normalColor;
        }

        // Efecto de "brillo" (pulsación)
        float alpha = Mathf.Sin(Time.time * 3f) * 0.3f + 0.7f; // Pulsación entre 0.4 y 1.0
        Color currentColor = multiplierText.color;
        currentColor.a = alpha;
        multiplierText.color = currentColor;
    }
}
