using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CodePanel
// - Скрипт для UI-клавиатуры сейфа.
// - Каждая кнопка "плюс/минус" вызывает публичные методы ниже и меняет одну цифру.
// - Кнопка "ОК" вызывает SubmitCode(), который просит SafeBox проверить весь код целиком.
public class CodePanel : MonoBehaviour
{
    // Ссылка на SafeBox, которым мы управляем (нужно, чтобы попросить его проверить код).
    public SafeBox safeBox;

    // 4 UI Text элемента (цифры) на панели (назначаются в инспекторе).
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;

    // Кнопки "Плюс": увеличивают соответствующую цифру (9 -> 0 по кругу).
    public void AddNumber1() { AddNumber(text1); }
    public void AddNumber2() { AddNumber(text2); }
    public void AddNumber3() { AddNumber(text3); }
    public void AddNumber4() { AddNumber(text4); }

    // Кнопки "Минус": уменьшают соответствующую цифру (0 -> 9 по кругу).
    public void DecreaseNumber1() { DecreaseNumber(text1); }
    public void DecreaseNumber2() { DecreaseNumber(text2); }
    public void DecreaseNumber3() { DecreaseNumber(text3); }
    public void DecreaseNumber4() { DecreaseNumber(text4); }

    void AddNumber(Text text)
    {
        // Парсим текущую цифру из UI-текста в int.
        int numb = int.Parse(text.text);
        // Увеличиваем цифру до 9; если было 9 — делаем 0.
        if (numb < 9) numb++;
        else numb = 0;
        // Записываем обновлённую цифру обратно в UI.
        text.text = numb.ToString();
        // Важно: код тут НЕ проверяем автоматически — это делает кнопка "ОК" через SubmitCode().
    }

    void DecreaseNumber(Text text)
    {
        // Парсим текущую цифру из UI-текста в int.
        int numb = int.Parse(text.text);
        // Уменьшаем цифру до 0; если было 0 — делаем 9.
        if (numb > 0) numb--;
        else numb = 9;
        // Записываем обновлённую цифру обратно в UI.
        text.text = numb.ToString();
        // Важно: код тут НЕ проверяем автоматически — это делает кнопка "ОК" через SubmitCode().
    }

    // Вызывается кнопкой "ОК" для проверки введённых цифр.
    public void SubmitCode()
    {
        // Просим SafeBox собрать текущую комбинацию и сравнить её с правильным кодом.
        safeBox.CheckCode();
    }
}