using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    // игрок
    GameObject player;

    void Start()
    {
        // ищем игрока по tag-у
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UseBattery()
    {
        // добавляем заряд игроку
        player.GetComponentInChildren<myFlashLight>().AddCharge(50f);
        // уничтожаем батарейку
        Destroy(gameObject);
    }
}