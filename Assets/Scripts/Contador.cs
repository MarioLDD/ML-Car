using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    [SerializeField] private TMP_Text stepCountText;
    [SerializeField] private int stepsAmount;

    public static Contador Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void AddSteps()
    {
        stepsAmount++;
        stepCountText.text = $"Episodes: {stepsAmount}";
    }

}
