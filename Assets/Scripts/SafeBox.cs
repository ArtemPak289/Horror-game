using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class SafeBox : MonoBehaviour
{
    public Text[] texts;
    public GameObject panel;
    GameObject player;
    public string code;
    public bool isOpen = false;
    bool isInteracting = false;
    public GameObject safeBoxCase;
    public float safeBoxOpenAngle = 90f;
    public float smooth = 2f;

    // Переменные для Классной работы и ДЗ
    public Text statusText;
    private int attempts = 3;
    public GameObject screamer; // Ссылка на пугалку

    void Start()
    {
        if (panel != null) panel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isOpen)
        {
            Quaternion targetRotationOpen = Quaternion.Euler(0, safeBoxOpenAngle, 0);
            safeBoxCase.transform.localRotation = Quaternion.Slerp(
                safeBoxCase.transform.localRotation,
                targetRotationOpen,
                smooth * Time.deltaTime
            );
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isInteracting)
        {
            DisablePanel();
        }
    }

    public void ShowPanel()
    {
        if (panel != null) panel.SetActive(true);
        isInteracting = true;

        if (player != null)
        {
            var controller = player.GetComponent<FirstPersonController>();
            if (controller != null) controller.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisablePanel()
    {
        if (panel != null) panel.SetActive(false);

        if (player != null)
        {
            var controller = player.GetComponent<FirstPersonController>();
            if (controller != null) controller.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isInteracting = false;
    }

    public void CheckCode()
    {
        string curCombination = "";
        // Собираем код из массива текстов
        for (int i = 0; i < texts.Length; i++)
        {
            curCombination += texts[i].text;
        }

        if (curCombination == code)
        {
            isOpen = true;
            if (statusText != null) statusText.text = "ВЕРНО!";
            DisablePanel();
            gameObject.layer = LayerMask.NameToLayer("Default"); // Сейф открыт
        }
        else
        {
            attempts--; // Отнимаем попытку только при нажатии "ОК"

            if (attempts > 0)
            {
                // Классная работа: Вывод сообщения об ошибке
                if (statusText != null) statusText.text = "НЕВЕРНЫЙ КОД! Попыток: " + attempts;
            }
            else
            {
                // Домашнее задание: Блокировка и скример
                if (statusText != null) statusText.text = "СИСТЕМА ЗАБЛОКИРОВАНА!";
                DisablePanel();
                gameObject.layer = LayerMask.NameToLayer("Default"); // Сейф заблокирован навсегда

                // Включаем скример
                if (screamer != null)
                {
                    screamer.SetActive(true);
                }
            }
        }
    }
}