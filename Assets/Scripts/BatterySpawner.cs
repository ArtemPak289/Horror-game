// BatterySpawner.cs
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [Header("Battery Settings")]
    [SerializeField] private GameObject batteryPrefab;   // Префаб батарейки

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;    // Точки спауна

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 30f;  // Интервал между попытками спавна
    [SerializeField] private int maxBatteries = 1;       // Макс. кол-во батареек на сцене

    private float timer = 0f;
    private int currentBatteryCount = 0;

    private void Update()
    {
        if (currentBatteryCount < maxBatteries)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                TrySpawnBattery();
                timer = 0f;
            }
        }
    }

    private void TrySpawnBattery()
    {
        if (batteryPrefab == null)
        {
            Debug.LogWarning("BatterySpawner: batteryPrefab is not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("BatterySpawner: no spawn points assigned!");
            return;
        }

        // Если уже достигли максимума — не спавним
        if (currentBatteryCount >= maxBatteries)
        {
            return;
        }

        // Выбираем случайную точку спауна
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Создаём батарейку
        GameObject battery = Instantiate(batteryPrefab, spawnPoint.position, spawnPoint.rotation);

        // Пытаемся получить компонент BatteryInstance (см. следующий шаг)
        BatteryInstance batteryInstance = battery.GetComponent<BatteryInstance>();
        if (batteryInstance == null)
        {
            batteryInstance = battery.AddComponent<BatteryInstance>();
        }

        batteryInstance.SetSpawner(this);

        currentBatteryCount++;
    }

    // Вызывается батарейкой при уничтожении
    public void OnBatteryDestroyed()
    {
        currentBatteryCount = Mathf.Max(0, currentBatteryCount - 1);
    }
}
