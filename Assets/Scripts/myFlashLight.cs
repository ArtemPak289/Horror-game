using UnityEngine;
using UnityEngine.UI;

// myFlashLight
// - Управляет компонентом Light фонарика: включение/выключение и расход заряда со временем.
// - Обновляет простой UI-индикатор (масштаб Transform), чтобы показывать оставшийся заряд.
public class myFlashLight : MonoBehaviour
{
    [Header("Flashlight")]
    // Клавиша включения/выключения фонарика.
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    // Максимальный заряд, который может иметь фонарик.
    [SerializeField] private float maxCharge = 100f;
    // Скорость расхода заряда (в единицах в секунду), пока фонарик включён.
    [SerializeField] private float lightDrain = 5f;

    [Header("UI")]
    // UI-элемент, показывающий заряд (мы меняем scale по X от 0..1).
    [SerializeField] private Transform indicator;

    // Ссылка на Light на этом GameObject (кэшируем).
    private Light flashLight;
    // Текущий заряд (0..maxCharge).
    private float currentCharge;
    // Состояние: считается ли фонарик включённым.
    private bool isOn;

    private void Start()
    {
        // Получаем компонент Light на этом же GameObject.
        flashLight = GetComponent<Light>();
        // Стартуем с полного заряда.
        currentCharge = maxCharge;
        // Инициализируем isOn по текущему состоянию Light.enabled (если Light существует).
        isOn = flashLight != null && flashLight.enabled;

        // Обновляем UI в начале, чтобы сразу показать правильный заряд.
        UpdateIndicator();
    }

    private void Update()
    {
        // Обрабатываем ввод и переключение фонарика.
        HandleToggle();
        // Если фонарик включён — расходуем заряд.
        DrainCharge();
        // Обновляем UI каждый кадр (чтобы индикатор выглядел плавно).
        UpdateIndicator();
    }

    private void HandleToggle()
    {
        // Переключаем только если нажата клавиша и заряд больше 0.
        if (Input.GetKeyDown(toggleKey) && currentCharge > 0f)
        {
            // Меняем состояние на противоположное (вкл/выкл).
            isOn = !isOn;

            // Если Light существует — применяем состояние к Unity Light.
            if (flashLight != null)
            {
                flashLight.enabled = isOn;
            }
        }
    }

    private void DrainCharge()
    {
        // Расходуем заряд только пока фонарик включён.
        if (isOn)
        {
            // Уменьшаем заряд с учётом времени, чтобы не зависеть от FPS.
            currentCharge -= lightDrain * Time.deltaTime;

            // Если заряд закончился — ограничиваем до 0 и принудительно выключаем фонарик.
            if (currentCharge <= 0f)
            {
                // Ставим заряд ровно 0 (не допускаем отрицательных значений).
                currentCharge = 0f;
                // Обновляем внутреннее состояние.
                isOn = false;

                // Если Light существует — выключаем его в Unity.
                if (flashLight != null)
                {
                    flashLight.enabled = false;
                }
            }
        }
    }

    private void UpdateIndicator()
    {
        // Процент оставшегося заряда в диапазоне 0..1.
        float percent;

        // Значение по умолчанию на случай, если maxCharge некорректен (0), чтобы не делить на 0.
        percent = 0f;

        // Считаем процент только если maxCharge больше 0.
        if (maxCharge > 0f)
        {
            percent = currentCharge / maxCharge;
        }

        // Если индикатор назначен — масштабируем его в соответствии с процентом.
        if (indicator != null)
        {
            // Scale по X показывает остаток, по Y/Z оставляем 1.
            indicator.localScale = new Vector3(percent, 1f, 1f);
        }
    }

    public void AddCharge(float amount)
    {
        // Добавляем указанное количество к текущему заряду.
        currentCharge += amount;

        // Ограничиваем до maxCharge, чтобы заряд не "переполнялся".
        if (currentCharge > maxCharge)
        {
            currentCharge = maxCharge;
        }

        // Если фонарик считается включённым и заряд теперь есть — убеждаемся, что Light включён.
        if (currentCharge > 0f && flashLight != null && isOn)
        {
            flashLight.enabled = true;
        }

        // Сразу обновляем UI, чтобы подбор батарейки ощущался мгновенно.
        UpdateIndicator();
    }
}
