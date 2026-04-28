using UnityEngine;
using UnityEngine.UI;

// Interact
// - Пускает луч (Raycast) вперёд от этого объекта (обычно игрок/камера).
// - Если луч попадает в объект на слое `interactLayer` в пределах `interactDistance`, показывает иконку взаимодействия.
// - По нажатию E выполняет действие в зависимости от тега объекта (Battery, SafeBox и т.д.).
public class Interact : MonoBehaviour
{
    // Слои, которые считаются "интерактивными" для Raycast (настраивается в инспекторе).
    public LayerMask interactLayer;
    // Максимальная дистанция взаимодействия (в единицах мира / метрах).
    public float interactDistance = 3f;
    // UI-иконка, которая показывает "можно взаимодействовать".
    public Image interactIcon;

    void Start()
    {
        // В начале игры блокируем и прячем курсор, чтобы управление мышью работало как в FPS.
        // (Так мы остаёмся в режиме геймплея, а не в режиме UI.)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Прячем иконку взаимодействия, пока не смотрим на интерактивный объект.
        if (interactIcon != null) interactIcon.enabled = false;
    }

    void Update()
    {
        // Если открыт какой-то UI (курсор видим) — просто прячем иконку взаимодействия.
        // Мы специально НЕ делаем `return;`, чтобы не ломать остальную логику.
        if (Cursor.visible)
        {
            if (interactIcon != null) interactIcon.enabled = false;
        }

        // Создаём луч из позиции объекта вперёд (transform.forward).
        Ray ray = new Ray(transform.position, transform.forward);
        // RaycastHit хранит информацию о попадании (коллайдер, точка, нормаль и т.д.).
        RaycastHit hit;

        // Рисуем луч в окне Scene (для отладки). Длина красной линии = interactDistance.
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        // Physics.Raycast возвращает true, если луч во что-то попал.
        // Также мы фильтруем по `interactLayer`, чтобы попадать только в нужные объекты.
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // Лог для отладки (по желанию): показывает имя объекта, в который попал луч.
            // Debug.Log("Вижу объект: " + hit.collider.name);

            // Показываем иконку только когда мы не в режиме UI (курсор скрыт).
            if (!Cursor.visible && interactIcon != null)
                interactIcon.enabled = true;

            // Если игрок нажал кнопку взаимодействия — выбираем действие по тегу.
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Если смотрим на батарейку — "используем" её (заряжаем фонарик и уничтожаем батарейку).
                if (hit.collider.CompareTag("Battery"))
                {
                    // Пытаемся получить компонент Battery на объекте, в который попали.
                    var battery = hit.collider.GetComponent<Battery>();
                    // Проверка на null, чтобы не было ошибки, если компонента нет.
                    if (battery != null) battery.UseBattery();
                }
                // Если смотрим на сейф — открываем его панель ввода кода.
                else if (hit.collider.CompareTag("SafeBox"))
                {
                    // Пытаемся получить компонент SafeBox на объекте, в который попали.
                    var safe = hit.collider.GetComponent<SafeBox>();
                    // Если компонент есть — показываем панель.
                    if (safe != null) safe.ShowPanel();
                }
            }
        }
        else
        {
            // Если перед нами нет интерактивных объектов — прячем иконку.
            if (interactIcon != null) interactIcon.enabled = false;
        }
    }
}