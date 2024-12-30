using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Object", menuName = "Dialogue/Create New Audio Object")]
public class AudioObject : ScriptableObject
{
    public AudioClip clip;
    public string subtitle;
}
