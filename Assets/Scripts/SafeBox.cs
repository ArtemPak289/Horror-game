using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

// SafeBox
// - Показывает панель/клавиатуру для ввода кода.
// - Если введённый код совпал, плавно открывает дверцу сейфа и отключает дальнейшее взаимодействие.
public class SafeBox : MonoBehaviour
{
    // Массив UI Text элементов, где хранится каждая цифра кода (например, 4 цифры).
    public Text[] texts;
    // UI-панель (GameObject), в которой находится интерфейс ввода кода.
    public GameObject panel;
    // Ссылка на игрока (кэшируем, находим по тегу).
    GameObject player;
    // Правильный код, который нужно ввести (string удобен для сравнения склеенных цифр).
    public string code;
    // Если true — сейф считается открытым, и дверца будет анимироваться в открытое положение.
    public bool isOpen = false;
    // True, пока игрок взаимодействует с панелью (нужно, чтобы закрывать по Escape).
    bool isInteracting = false;
    // Часть сейфа (дверца/крышка), которая будет поворачиваться при открытии.
    public GameObject safeBoxCase;
    // Целевой угол поворота по оси Y (в градусах) для состояния "открыто".
    public float safeBoxOpenAngle = 90f;
    // Скорость сглаживания поворота (для Slerp).
    public float smooth = 2f;

    void Start()
    {
        // Прячем панель в начале игры (чтобы не отображалась сразу).
        if (panel != null) panel.SetActive(false);
        // Находим игрока один раз, чтобы включать/выключать управление, пока открыта UI-панель.
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Если сейф открыт — плавно поворачиваем дверцу к "открытому" положению.
        if (isOpen)
        {
            // Целевой поворот: вокруг оси Y на заданный угол открытия.
            Quaternion targetRotationOpen = Quaternion.Euler(0, safeBoxOpenAngle, 0);
            // Плавно интерполируем текущий localRotation к целевому повороту.
            safeBoxCase.transform.localRotation = Quaternion.Slerp(
                safeBoxCase.transform.localRotation,
                targetRotationOpen,
                smooth * Time.deltaTime
            );
        }

        // Пока взаимодействуем — разрешаем закрывать панель клавишей Escape.
        if (Input.GetKeyDown(KeyCode.Escape) && isInteracting)
        {
            // Закрываем UI и возвращаем управление игроку.
            DisablePanel();
        }
    }

    public void ShowPanel()
    {
        // Показываем UI-панель с вводом кода.
        if (panel != null) panel.SetActive(true);
        // Отмечаем, что мы в режиме взаимодействия (чтобы Escape закрывал панель).
        isInteracting = true;

        // Выключаем управление игроком, пока открыта панель (чтобы мышь работала по UI).
        if (player != null)
        {
            // FirstPersonController из Standard Assets: выключение останавливает движение/поворот камеры.
            var controller = player.GetComponent<FirstPersonController>();
            if (controller != null) controller.enabled = false;
        }

        // Разблокируем и показываем курсор, чтобы можно было нажимать кнопки UI.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisablePanel()
    {
        // Прячем UI-панель.
        if (panel != null) panel.SetActive(false);

        // Возвращаем управление игроку, когда закончили взаимодействие.
        if (player != null)
        {
            // Включаем FirstPersonController обратно.
            var controller = player.GetComponent<FirstPersonController>();
            if (controller != null) controller.enabled = true;
        }

        // Блокируем и прячем курсор, возвращаем FPS-управление.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Больше не взаимодействуем.
        isInteracting = false;
    }

    public void CheckCode()
    {
        // Здесь собираем текущую комбинацию, введённую в UI.
        string curCombination = "";

        // Склеиваем код из всех Text элементов (цифр).
        for (int i = 0; i < texts.Length; i++)
        {
            // Добавляем текущую цифру/символ из этого Text в строку комбинации.
            curCombination += texts[i].text;
        }

        // Если введённая комбинация совпала с правильным кодом...
        if (curCombination == code)
        {
            // Отмечаем сейф как открытый, чтобы Update() начал анимацию открытия.
            isOpen = true;
            // Закрываем UI и возвращаем управление игроку.
            DisablePanel();
            // Переводим объект сейфа в слой Default, чтобы он больше не считался интерактивным (нельзя открыть снова).
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}