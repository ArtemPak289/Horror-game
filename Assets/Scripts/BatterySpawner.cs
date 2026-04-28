// BatterySpawner
// - Периодически создаёт (спавнит) префабы батареек в случайных точках.
// - Ведёт счётчик, чтобы на сцене одновременно было не больше `maxBatteries`.
// - Использует `BatteryInstance`, чтобы созданная батарейка могла сообщить спавнеру о своём уничтожении.
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [Header("Battery Settings")]
    // Префаб батарейки, который будет создаваться (назначается в инспекторе).
    [SerializeField] private GameObject batteryPrefab;

    [Header("Spawn Points")]
    // Список точек спавна (позиция/поворот), где может появиться батарейка (назначается в инспекторе).
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Settings")]
    // Интервал (в секундах) между попытками спавна, пока мы ниже лимита.
    [SerializeField] private float spawnInterval = 30f;
    // Максимальное количество батареек, которые могут существовать одновременно.
    [SerializeField] private int maxBatteries = 1;

    // Накопитель времени с момента последней попытки спавна.
    private float timer = 0f;
    // Сколько заспавненных батареек сейчас "живы" на сцене.
    private int currentBatteryCount = 0;

    private void Update()
    {
        // Запускаем таймер спавна только если мы ниже максимума по количеству батареек.
        if (currentBatteryCount < maxBatteries)
        {
            // Добавляем время, прошедшее с прошлого кадра.
            timer += Time.deltaTime;

            // Когда накопили достаточно времени — пробуем заспавнить и сбрасываем таймер.
            if (timer >= spawnInterval)
            {
                TrySpawnBattery();
                timer = 0f;
            }
        }
    }

    private void TrySpawnBattery()
    {
        // Проверка: без префаба мы ничего не сможем создать.
        if (batteryPrefab == null)
        {
            Debug.LogWarning("BatterySpawner: batteryPrefab is not assigned!");
            return;
        }

        // Проверка: без точек спавна мы не знаем, где создавать батарейку.
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("BatterySpawner: no spawn points assigned!");
            return;
        }

        // Если уже достигли максимума — ничего не делаем.
        if (currentBatteryCount >= maxBatteries)
        {
            return;
        }

        // Выбираем случайный индекс точки спавна в диапазоне [0, spawnPoints.Length).
        int randomIndex = Random.Range(0, spawnPoints.Length);
        // Берём Transform выбранной точки спавна.
        Transform spawnPoint = spawnPoints[randomIndex];

        // Создаём (Instantiate) батарейку в позиции/повороте точки спавна.
        GameObject battery = Instantiate(batteryPrefab, spawnPoint.position, spawnPoint.rotation);

        // Убеждаемся, что у созданной батарейки есть BatteryInstance, чтобы она могла сообщить о своём уничтожении.
        BatteryInstance batteryInstance = battery.GetComponent<BatteryInstance>();
        if (batteryInstance == null)
        {
            // Если компонента нет — добавляем его во время выполнения.
            batteryInstance = battery.AddComponent<BatteryInstance>();
        }

        // Передаём BatteryInstance ссылку на этот спавнер (чтобы OnDestroy смог вызвать нас).
        batteryInstance.SetSpawner(this);

        // Увеличиваем счётчик: на сцене появилась новая батарейка.
        currentBatteryCount++;
    }

    // Вызывается батарейкой (через BatteryInstance.OnDestroy), когда батарейка уничтожена/подобрана.
    public void OnBatteryDestroyed()
    {
        // Уменьшаем счётчик, но не даём ему уйти ниже 0.
        currentBatteryCount = Mathf.Max(0, currentBatteryCount - 1);
    }
}
