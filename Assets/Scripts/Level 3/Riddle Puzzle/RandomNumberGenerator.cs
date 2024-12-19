using UnityEngine;
using TMPro;

public class RandomNumberGenerator : MonoBehaviour
{
    [Header("Number Fields Settings")]
    [SerializeField] private TMP_Text[] numberDisplays;
    private int[] randomNumbers;

    void Start()
    {
        GenerateRandomNumbers();
    }

    void GenerateRandomNumbers()
    {
        randomNumbers = new int[numberDisplays.Length];

        for (int i = 0; i < numberDisplays.Length; i++)
        {
            randomNumbers[i] = Random.Range(0, 10); // Generate a number between 0 and 9
            numberDisplays[i].text = randomNumbers[i].ToString();
        }
    }

    public int[] GetGeneratedNumbers()
    {
        return randomNumbers;
    }
}
