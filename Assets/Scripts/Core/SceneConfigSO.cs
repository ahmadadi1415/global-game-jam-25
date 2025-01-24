using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scene Config", fileName ="Scene Config")]
public class SceneConfigSO : ScriptableObject
{
    [Header("General")]
    public int MaxTurns;

    [Header("Power Up")]
    public int Horizontal;
    public int Vertical;
    public int Surround;
    public int Cross;
    public int Triple;
}
