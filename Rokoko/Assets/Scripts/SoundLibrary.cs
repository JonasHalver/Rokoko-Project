using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Library", menuName ="Library")]
public class SoundLibrary : ScriptableObject
{
    public List<AudioClip> woodSounds = new List<AudioClip>();
    public List<AudioClip> metalSounds = new List<AudioClip>();
    public List<AudioClip> glassSounds = new List<AudioClip>();
}
