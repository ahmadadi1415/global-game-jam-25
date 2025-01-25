using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene Config", fileName = "Scene Config")]
public class SceneConfigSO : ScriptableObject
{
    [Header("General")]
    public int MaxTurns;

    [Header("Power Up Limits")]
    public int HorizontalLimit;
    public int VerticalLimit;
    public int SurroundLimit;
    public int CrossLimit;
    public int TripleLimit;

    [Header("Grid")]
    public int Width;
    public int Height;
    [Tooltip("Hole started from 0 in cartesian coordinates.")]
    public List<Vector2Int> Holes = new();
}
