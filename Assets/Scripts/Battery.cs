using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Battery
// - Батарейка-предмет на сцене, который можно подобрать.
// - Когда её "используют" (взаимодействуют), она находит фонарик игрока, добавляет заряд и уничтожается.
public class Battery : MonoBehaviour
{
    // Ссылка на игрока (кэшируем, находим по тегу).
    GameObject player;

    void Start()
    {
        // Находим игрока один раз при старте по тегу "Player".
        // (Предполагается, что в сцене ровно один объект с тегом "Player".)
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UseBattery()
    {
        // Ищем компонент фонарика где-то внутри иерархии игрока
        // и добавляем 50 единиц заряда.
        player.GetComponentInChildren<myFlashLight>().AddCharge(50f);
        // Уничтожаем GameObject батарейки, чтобы она исчезла после подбора.
        Destroy(gameObject);
    }
}