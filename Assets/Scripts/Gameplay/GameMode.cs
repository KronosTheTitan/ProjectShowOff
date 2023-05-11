using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    public abstract float[] GetScores();
    public abstract void Update();
}