using System;
using UnityEngine;

/// <summary>
///     Managr for events. Currently is being used for UI updates.
/// </summary>
public sealed class EventsManager : MonoPersistentSingleton<EventsManager>
{
    public event Action OnUpdateUpgradesHUD;
    public void UpdateUpgradesHUD()
    {
        Debug.Log("Updating upgrades HUD");
        OnUpdateUpgradesHUD?.Invoke();
    }

    public event Action OnUpdateMetagameHUD;
    public void UpdateMetagameHUD()
    {
        Debug.Log("Updating metagame HUD");
        OnUpdateMetagameHUD?.Invoke();
    }
}
