// BatteryInstance
// - Маленький вспомогательный компонент, который висит на каждой заспавненной батарейке.
// - Хранит ссылку на спавнер, чтобы сообщить ему, когда эта батарейка уничтожится.
// - Так счётчик `currentBatteryCount` у спавнера остаётся корректным (например, при подборе батарейки).
using UnityEngine;

public class BatteryInstance : MonoBehaviour
{
    // Ссылка на спавнер, который создал эту батарейку (устанавливается сразу после Instantiate).
    private BatterySpawner spawner;

    public void SetSpawner(BatterySpawner spawnerRef)
    {
        // Сохраняем ссылку, чтобы потом можно было "отчитаться" спавнеру.
        spawner = spawnerRef;
    }

    private void OnDestroy()
    {
        // Unity-событие: вызывается, когда объект/компонент уничтожается.
        // Это происходит при подборе (Destroy), при выгрузке сцены и т.д.
        if (spawner != null)
        {
            // Сообщаем спавнеру, что одной батарейкой стало меньше.
            spawner.OnBatteryDestroyed();
        }
    }
}
