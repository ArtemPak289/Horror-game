using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public LayerMask interactLayer;
    public float interactDistance = 3f;
    public Image interactIcon;

    void Start()
    {
        // Принудительно скрываем курсор в начале, чтобы Raycast заработал
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (interactIcon != null) interactIcon.enabled = false;
    }

    void Update()
    {
        // Если открыта панель сейфа (курсор виден), просто выключаем иконку, 
        // но НЕ выходим из метода через return, чтобы не ломать логику.
        if (Cursor.visible)
        {
            if (interactIcon != null) interactIcon.enabled = false;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // РИСУЕМ ЛУЧ в окне Scene (красная линия). Если она не долетает до сейфа — увеличь Distance.
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // ЛОГ В КОНСОЛЬ: покажет имя объекта, в который попал луч
            // Debug.Log("Вижу объект: " + hit.collider.name);

            if (!Cursor.visible && interactIcon != null)
                interactIcon.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Battery"))
                {
                    var battery = hit.collider.GetComponent<Battery>();
                    if (battery != null) battery.UseBattery();
                }
                else if (hit.collider.CompareTag("SafeBox"))
                {
                    var safe = hit.collider.GetComponent<SafeBox>();
                    if (safe != null) safe.ShowPanel();
                }
            }
        }
        else
        {
            if (interactIcon != null) interactIcon.enabled = false;
        }
    }
}