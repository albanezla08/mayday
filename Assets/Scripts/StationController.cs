using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StationController : MonoBehaviour, IInteractibleWithShip
{
    public UnityEvent OnPlayerWin;

    void IInteractibleWithShip.OnPlayerInteract(PlayerController playerScript)
    {
        OnPlayerWin?.Invoke();
    }
}
