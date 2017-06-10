using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationController : MonoBehaviour
{
    public Dropdown LanguageDropDown;
    public Text TitleText;
    public Text LoadingText;
    public Text MissionText;    
    public Text WaterHelicopterDetailsText;
    public Text ChemicalsHelicopterDetailsText;
    public Text BombsHelicopterDetailsText;
    public Text Tip1Text;
    public Text Tip2Text;
    public Text Tip3Text;    
    public Text NoResourcesText;


    private void Start ()
    {
        
        if (!PlayerPrefs.HasKey("lang"))
        {
            PlayerPrefs.SetInt("lang", 0);
        }
        else if (LanguageDropDown != null)
        {
            LanguageDropDown.value = PlayerPrefs.GetInt("lang");
        }
        SetLanguage();
    }	
    public void ChangeLanguage()
    {
        if (LanguageDropDown != null)
        {
            PlayerPrefs.SetInt("lang", LanguageDropDown.value);
        }
        SetLanguage();
    }
    private void SetLanguage()
    {
        if(PlayerPrefs.GetInt("lang") == 0)
        {
            if (TitleText != null)
            {
                TitleText.text = "Forest Saviors";
            }
            if (LoadingText != null)
            {
                LoadingText.text = "Loading...";
            }
            if (MissionText != null)
            {
                MissionText.text = "Mission: put out fire and save as many trees as possible.\nGeneral tip: put out fire against wind.";
            }
            if (WaterHelicopterDetailsText != null)
            {
                WaterHelicopterDetailsText.text = "Speed: low\nDrops number: 4\nRefill: fast, at water, manual";
            }
            if (ChemicalsHelicopterDetailsText != null)
            {
                ChemicalsHelicopterDetailsText.text = "Speed: high\nDrops number: 6\nRefill: slow, on base, automatic";
            }
            if (BombsHelicopterDetailsText != null)
            {
                BombsHelicopterDetailsText.text = "Speed: medium\nDrops number: 8\nRefill: slow, on base, automatic";
            }
            if (Tip1Text != null)
            {
                Tip1Text.text = "Tap fire to send helicopter to put it out.\nTap water to send helicopter to refill tank.";
            }
            if (Tip2Text != null)
            {
                Tip2Text.text = "Tap buttons on the left to switch helicopters.";
            }
            if (Tip3Text != null)
            {
                Tip3Text.text = "Don't let house catch fire.";
            }
            if (NoResourcesText != null)
            {
                NoResourcesText.text = "Not enough resources, need to refill";
            }
        }
        else if (PlayerPrefs.GetInt("lang") == 1)
        {
            if (TitleText != null)
            {
                TitleText.text = "Рятівники лісу";
            }
            if (LoadingText != null)
            {
                LoadingText.text = "Завантаження...";
            }
            if (MissionText != null)
            {
                MissionText.text = "Місія: загасити вогонь і врятувати якомога більше дерев.\nПідсказка: гасити пожежу потрібно проти напрямку вітру.";
            }
            if (WaterHelicopterDetailsText != null)
            {
                WaterHelicopterDetailsText.text = "Швидкість: низька\nК-сть зкидувань: 4\nПоповнення: швидке, у водоймі, ручне";
            }
            if (ChemicalsHelicopterDetailsText != null)
            {
                ChemicalsHelicopterDetailsText.text = "Швидкість: висока\nК-сть зкидувань: 6\nПоповнення: повільне, на базі, автоматичне";
            }
            if (BombsHelicopterDetailsText != null)
            {
                BombsHelicopterDetailsText.text = "Швидкість: середня\nК-сть зкидувань: 8\nПоповнення: повільне, на базі, автоматичне";
            }
            if (Tip1Text != null)
            {
                Tip1Text.text = "Натисніть на пожежі, щоб відправити гелікоптер погасити її.\nНатисніть на водоймі, щоб відправити гелікоптер поповнити резеруар.";
            }
            if (Tip2Text != null)
            {
                Tip2Text.text = "Натискайте на кнопки зліва, щоб перемикатись між гелікоптерами.";
            }
            if (Tip3Text != null)
            {
                Tip3Text.text = "Не дайте хатині загорітись.";
            }
            if (NoResourcesText != null)
            {
                NoResourcesText.text = "Не достатньо ресурсів, потрібно їх поповнити";
            }
        }
    }    
}
