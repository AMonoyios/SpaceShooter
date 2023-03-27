using System;

public sealed class EventsManager : MonoSingleton<EventsManager>
{
    public event Action<float> OnEnemyDamagedEvent;
    public void EnemyDamaged(float amount)
    {
        OnEnemyDamagedEvent?.Invoke(amount);
    }
}
