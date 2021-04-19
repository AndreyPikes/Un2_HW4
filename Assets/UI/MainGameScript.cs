using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class MainGameScript : MonoBehaviour
{
    [SerializeField] private GameObject[] arrayQuestions; //массив панелей с вопросами
    [SerializeField] private GameObject startMenu; //панель стартового меню
    [SerializeField] private GameObject finishResult; //финишная панель с баллами
    private GameObject currentScreen; //текущая открытая панель
    private int questionIndex = 0; //номер вопроса
    [SerializeField] private Text scoreText; //текстовое поле для вывода очков

    private int scoreCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        startMenu.SetActive(true);
        currentScreen = startMenu;
        var random = new System.Random(DateTime.Now.Millisecond);
        arrayQuestions = arrayQuestions.OrderBy(x => random.Next()).ToArray();
    }
    /// <summary>
    /// нажатие на кнопку с правильным ответом
    /// </summary>
    public void GoodAnswerClicked()
    {
        //прибавить очки
        scoreCounter++;
        //переключить на след экран        
        StartCoroutine("ChangeColorGreen");  
    }
    /// <summary>
    /// Нажатие на кнопку с неправильным ответом
    /// </summary>
    public void BadAnswerClicked()
    {
        //переключить на след экран
        StartCoroutine("ChangeColorRed");
    }
    
    /// <summary>
    /// Нажатие кнопки отправки текстового ответа
    /// </summary>
    public void ConfirmButtonClicked()
    {
        string rightAnswer = currentScreen.GetComponentInChildren<Text>().text;
        string currentAnswer = currentScreen.GetComponentInChildren<InputField>().text;
        if (currentAnswer != "")
        {
            if (currentAnswer == rightAnswer)
            {
                scoreCounter++;
                StartCoroutine("ChangeColorGreen");
            }
            else StartCoroutine("ChangeColorRed");
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartNewGame()
    {
        scoreCounter = 0;
        OpenNextQuestion();
    }
    public void ReturnToMainMenu()
    {
        currentScreen.SetActive(false);
        startMenu.SetActive(true);
        currentScreen = startMenu;
        questionIndex = 0;
    }
    private void OpenNextQuestion()
    {
        if (questionIndex < arrayQuestions.Length)
        {
            currentScreen.SetActive(false);
            arrayQuestions[questionIndex].SetActive(true);            
            currentScreen = arrayQuestions[questionIndex];
            if (currentScreen.GetComponentInChildren<InputField>() != null)
            {
                currentScreen.GetComponentInChildren<InputField>().text = "";
            }
        }
        questionIndex++;
        if (questionIndex == arrayQuestions.Length+1)
        {
            currentScreen.SetActive(false);
            finishResult.SetActive(true);
            currentScreen = finishResult;
            scoreText.text = $"Количество правильных ответов {scoreCounter.ToString()} " +
                $"из {arrayQuestions.Length}";
        }
    }
    /// <summary>
    /// корутина временно подсвечивающая правильный ответ зеленым цветом
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColorGreen() 
    {
        Image image = currentScreen.GetComponent<Image>();
        Color color = image.color;
        image.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        image.color = color;
        OpenNextQuestion();
    }
    /// <summary>
    /// корутина временно подсвечивающая неправильный ответ красным цветом
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColorRed()
    {
        Image image = currentScreen.GetComponent<Image>();
        Color color = image.color;
        image.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        image.color = color;
        OpenNextQuestion();
    }
}
