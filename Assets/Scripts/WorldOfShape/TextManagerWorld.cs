using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace MiniGame.WorldOfShape
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;

    public class TextManagerWorld : MonoBehaviour
    {
        public LanguageList languageList;

        public TextInputSt[] textInputs;

        public static TextManagerWorld instance;
        public Sprite[] gameNames;
        public Image gameName;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PopulateLnagauages();
            FirstLogin();
            if (AudioManagerWorld.instance != null)
            {
                OnChangeLanguage();
            }

        }
        public void FirstLogin()
        {
            if (AudioManagerWorld.instance != null)
            {
                if (!PlayerPrefs.HasKey("language_set"))
                {
                    switch (Application.systemLanguage)
                    {
                        case SystemLanguage.English:
                            AudioManagerWorld.instance.IncrementToLanguage0();
                            Debug.Log("English");
                            break;
                        case SystemLanguage.Lithuanian:
                            AudioManagerWorld.instance.IncrementToLanguage1();
                            Debug.Log("Lithuanian");
                            break;
                        case SystemLanguage.Estonian:
                            AudioManagerWorld.instance.IncrementToLanguage2();

                            Debug.Log("Estonian");
                            break;
                        case SystemLanguage.Latvian:
                            AudioManagerWorld.instance.IncrementToLanguage3();

                            Debug.Log("Latvian");
                            break;
                        case SystemLanguage.Polish:
                            AudioManagerWorld.instance.IncrementToLanguage4();

                            Debug.Log("Polish");
                            break;
                        case SystemLanguage.Czech:
                            AudioManagerWorld.instance.IncrementToLanguage5();

                            Debug.Log("Czech");
                            break;
                        case SystemLanguage.Slovak:
                            AudioManagerWorld.instance.IncrementToLangaue6();

                            Debug.Log("Slovak");
                            break;
                        default:
                            AudioManagerWorld.instance.IncrementToLanguage0();
                            break;
                    }

                    PlayerPrefs.SetInt("language_set", 1);
                    PlayerPrefs.Save();
                }

                OnChangeLanguage();
            }

        }
        public void OnChangeLanguage()
        {
            switch (AudioManagerWorld.instance.currentLanguage)
            {
                case AudioManagerWorld.LanguagesType.English:
                    {

                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            textInputs[i].text.text = languageList.languages[0].lines[i];
                        }
                        gameName.sprite = gameNames[0];
                        // kidsMathS.sprite = kidsMath[0];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = englishInputs[0];

                    }
                    break;
                case AudioManagerWorld.LanguagesType.Lithuanian:
                    {
                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            // textInputs[i].text.text = languageList.languages[1].lines[i];
                            textInputs[i].text.text = languageList.languages[1].lines[i];
                        }
                        gameName.sprite = gameNames[1];

                        // kidsMathS.sprite = kidsMath[1];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = LatvianInputs[1];

                    }
                    break;
                case AudioManagerWorld.LanguagesType.Estonian:
                    {
                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            textInputs[i].text.text = languageList.languages[2].lines[i];
                        }
                        gameName.sprite = gameNames[2];

                        // kidsMathS.sprite = kidsMath[1];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = LatvianInputs[1];

                    }
                    break;
                case AudioManagerWorld.LanguagesType.Latvian:
                    {
                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            textInputs[i].text.text = languageList.languages[3].lines[i];
                        }
                        gameName.sprite = gameNames[3];

                        // kidsMathS.sprite = kidsMath[2];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = LatvianInputs[2];

                    }
                    break;
                case AudioManagerWorld.LanguagesType.Polish:
                    {
                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            textInputs[i].text.text = languageList.languages[4].lines[i];
                        }
                        gameName.sprite = gameNames[4];

                        // kidsMathS.sprite = kidsMath[2];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = LatvianInputs[2];

                    }
                    break;
                case AudioManagerWorld.LanguagesType.Czech:
                    {
                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            textInputs[i].text.text = languageList.languages[5].lines[i];
                        }
                        gameName.sprite = gameNames[5];
                        // kidsMathS.sprite = kidsMath[2];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = LatvianInputs[2];

                    }
                    break;
                case AudioManagerWorld.LanguagesType.Slovak:
                    {
                        for (int i = 0; i < textInputs.Length; i++)
                        {
                            textInputs[i].text.text = languageList.languages[6].lines[i];
                        }
                        gameName.sprite = gameNames[6];

                        // kidsMathS.sprite = kidsMath[2];
                        // GamePlayPanelSize.instance.QuestionInstruction.text = LatvianInputs[2];

                    }
                    break;
            }
        }

        void PopulateLnagauages()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("language_translations_Find");
            if (jsonFile != null)
            {
                languageList = JsonUtility.FromJson<LanguageList>(jsonFile.text);
            }
            else
            {
                Debug.LogError("JSON file not found");
            }



        }

    }
}

[System.Serializable]
public class LanguageData
{
    public string name;
    public List<string> lines;
}

[System.Serializable]
public class LanguageList
{
    public List<LanguageData> languages;
}

[System.Serializable]
public class TextInputSt
{
    public string name;
    public TextMeshProUGUI text;
}
