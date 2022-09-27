using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    public virtual IEnumerator Start()
    {
        yield break;
    }
}
