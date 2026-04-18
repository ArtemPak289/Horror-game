using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    public SafeBox safeBox;

    // Указываем все 4 текста прямо в инспекторе
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;

    // Кнопки Плюс вызывают по индексу
    public void AddNumber1() { AddNumber(text1); }
    public void AddNumber2() { AddNumber(text2); }
    public void AddNumber3() { AddNumber(text3); }
    public void AddNumber4() { AddNumber(text4); }

    // Кнопки Минус вызывают по индексу
    public void DecreaseNumber1() { DecreaseNumber(text1); }
    public void DecreaseNumber2() { DecreaseNumber(text2); }
    public void DecreaseNumber3() { DecreaseNumber(text3); }
    public void DecreaseNumber4() { DecreaseNumber(text4); }

    void AddNumber(Text text)
    {
        int numb = int.Parse(text.text);
        if (numb < 9) numb++;
        else numb = 0;
        text.text = numb.ToString();
        // Убрали проверку кода отсюда!
    }

    void DecreaseNumber(Text text)
    {
        int numb = int.Parse(text.text);
        if (numb > 0) numb--;
        else numb = 9;
        text.text = numb.ToString();
        // Убрали проверку кода отсюда!
    }

    // НОВЫЙ МЕТОД ДЛЯ КНОПКИ "ОК"
    public void SubmitCode()
    {
        safeBox.CheckCode();
    }
}