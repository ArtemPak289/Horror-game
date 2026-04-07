using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    // слой, на котором расположены объекты, с которыми можно взаимодействовать
    public LayerMask interactLayer;
    // расстояние, на котором персонаж сможет взаимодействовать с объектом
    public float interactDistance;
    // картинка, которая будет появляться, при попадании луча на объект
    public Image interactIcon;

    void Start()
    {
        // отключаем иконку
        interactIcon.enabled = false;
    }

    void Update()
    {
        // создаем луч и указываем откуда он должен быть запущен и в каком направлении
        Ray ray = new Ray(transform.position, transform.forward);
        // переменная для хранения информации о том объекте, в который попадет луч
        RaycastHit hit;

        // если луч коснулся чего-то
        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // включаем иконку
            interactIcon.enabled = true;

            // если нажата клавиша E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // если tag объекта Battery
                if (hit.collider.tag == "Battery")
                {
                    // активируем метод подбора
                    hit.collider.GetComponent<Battery>().UseBattery();
                }
            }
        }
        else
        {
            // выключаем иконку
            interactIcon.enabled = false;
        }
    }
}