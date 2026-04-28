using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Screamer
// - Скример на триггере (jump scare).
// - Когда игрок входит в триггер, показывает картинку, проигрывает страшный звук и затем удаляет объекты.
public class Screamer : MonoBehaviour
{
    // GameObject с картинкой скримера (часто это UI-элемент). Назначается в инспекторе.
    public GameObject screamerImg;
    // AudioSource, через который проигрываем звук (кэшируем в Start).
    AudioSource aud;
    // Аудиоклип страшного звука, который проигрывается при срабатывании.
    public AudioClip scarySound;

    void Start()
    {
        // В начале игры прячем картинку скримера.
        screamerImg.SetActive(false);
        // Получаем AudioSource на этом объекте, чтобы проигрывать звуки.
        aud = GetComponent<AudioSource>();
    }

    // Unity-событие: вызывается, когда другой коллайдер входит в триггер.
    // Чтобы это работало:
    // - у этого объекта должен быть Collider с включённым "Is Trigger"
    // - у входящего объекта должен быть Collider (обычно ещё нужен Rigidbody где-то в связке)
    void OnTriggerEnter(Collider other)
    {
        // Реагируем только если в триггер вошёл игрок.
        if (other.tag == "Player")
        {
            // Показываем картинку.
            screamerImg.SetActive(true);
            // Проигрываем страшный звук один раз (не обязательно прерывает другие звуки).
            aud.PlayOneShot(scarySound);

            // Уничтожаем картинку через 2 секунды (чтобы она исчезла).
            Destroy(screamerImg, 2f);
            // Уничтожаем сам триггер через 2 секунды (чтобы не срабатывал повторно).
            Destroy(gameObject, 2f);
        }
    }
}