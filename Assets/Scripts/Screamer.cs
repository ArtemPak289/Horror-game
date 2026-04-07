using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour
{
    // картинка скримера
    public GameObject screamerImg;
    // компонент для воспроизведения звука
    AudioSource aud;
    // страшный звук
    public AudioClip scarySound;

    void Start()
    {
        // отключаем картинку
        screamerImg.SetActive(false);
        // получаем компонент AudioSource, чтобы управлять им
        aud = GetComponent<AudioSource>();
    }

    // метод срабатывает, когда объект входит в зону
    void OnTriggerEnter(Collider other)
    {
        // если в зону вошел игрок
        if (other.tag == "Player")
        {
            // включить картинку
            screamerImg.SetActive(true);
            // запустить страшный звук
            aud.PlayOneShot(scarySound);

            // уничтожить картинку через 2 секунды
            Destroy(screamerImg, 2f);
            // уничтожить Trigger через 2 секунды
            Destroy(gameObject, 2f);
        }
    }
}