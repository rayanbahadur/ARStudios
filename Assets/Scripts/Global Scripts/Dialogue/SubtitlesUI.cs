using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitlesUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitleText = default;

    public static SubtitlesUI instance;
    void Start()
    {
        instance = this;
        ClearSubtitles();
    }

    // Update is called once per frame
    public void SetSubtitle(string subtitle, float delay)
    {
        subtitleText.text = subtitle;
        StartCoroutine(ClearAfterSeconds(delay));
    }

    public void ClearSubtitles()
    {
        subtitleText.text = "";
    }

    private IEnumerator ClearAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearSubtitles();
    }


}
