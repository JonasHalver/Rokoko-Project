using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animation Library", menuName = "Library/Animations")]
public class AnimLibrary : ScriptableObject
{
    public List<string> leftPunches = new List<string>();
    public List<string> rightPunches = new List<string>();
    public List<string> specials = new List<string>();
}
