// BatteryInstance.cs
using UnityEngine;

public class BatteryInstance : MonoBehaviour
{
    private BatterySpawner spawner;

    public void SetSpawner(BatterySpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnBatteryDestroyed();
        }
    }
}
