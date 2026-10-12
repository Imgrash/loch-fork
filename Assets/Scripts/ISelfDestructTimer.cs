using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelfDestructTimer
{
    void StartSelfDestructTimer();
    void SetSelfDestructTimer(float selfDestructTimer);
    void StopSelfDestruct(bool resetTimer);
    float SelfDestructTimer { get; }
}
