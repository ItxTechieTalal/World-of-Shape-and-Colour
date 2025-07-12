using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.FindAndMatch
{


    public class AudioManagerFind : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] EnglishButtonClickSound;
        public AudioClip[] LithuanianButtonClickSounds;
        public AudioClip[] EstonianButtonClickSounds;
        public AudioClip[] LatvianButtonClickSounds;
        public AudioClip[] PolishButtonClickSounds;
        public AudioClip[] CzechButtonClickSounds;
        public AudioClip[] SlovakButtonClickSounds;



        public AudioClip[] RandomSounds;
      
        public static AudioManagerFind instance;
        public Sprite[] sprites;
        public GameObject GameSoundButton;
        public GameObject ObjectSoundButton;
        public AudioSource BgmusicSource;
        public int langaugeIndex = 0;
        // public TextMeshProUGUI languageButtonText;

        //langauge buttons

        public Image activeLanguageButton;
        public GameObject ActiveLanguageBackground;
        public GameObject DropDownMenu;
        public CustomFlag[] flags;
        public Sprite[] logos;
        public GameObject MainLogoContainer;
        public GameObject gamePlayLogo;
        public GameObject gamePlayLogo2;

        public enum LanguagesType
        {
            English,
            Lithuanian,
            Estonian,
            Latvian,
            Polish,
            Czech,
            Slovak
        }
        public LanguagesType currentLanguage = LanguagesType.English;

        public bool numericSound = true;
        public bool gameSound = true;
        private void Awake()
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


        public bool CheckIfNumericSoundIsEnabled()
        {
            return numericSound;
        }
        public bool CheckIfGameSoundIsEnabled()
        {
            return gameSound;
        }

        public void ToggleButtonSound()
        {
            Handheld.Vibrate();

            numericSound = !numericSound;
            Image image = ObjectSoundButton.transform.GetChild(0).GetComponent<Image>();
            if (numericSound)
            {
                image.color = Color.white;
            }
            else
            {
                image.color = Color.gray;
            }
        }
        public void ToggleGameSound()
        {
            gameSound = !gameSound;
            Handheld.Vibrate();

            BgmusicSource.mute = !BgmusicSource.mute;
            Image image = GameSoundButton.transform.GetChild(0).GetComponent<Image>();
            if (gameSound)
            {
                // image.sprite = sprites[0];
                image.color = Color.white;
            }
            else
            {
                // image.sprite = sprites[1];
                image.color = Color.gray;
            }
        }
        public void PlayButtonSound(int i)
        {
            if (CheckIfNumericSoundIsEnabled())
            {
                switch (currentLanguage)
                {
                    case LanguagesType.English:
                        audioSource.PlayOneShot(EnglishButtonClickSound[i]);
                        break;
                    case LanguagesType.Lithuanian:
                        // audioSource.PlayOneShot(LithuanianButtonClickSounds[i]);
                        audioSource.PlayOneShot(LithuanianButtonClickSounds[i]);
                        break;
                    case LanguagesType.Estonian:
                        audioSource.PlayOneShot(EstonianButtonClickSounds[i]);
                        break;
                    case LanguagesType.Latvian:
                        audioSource.PlayOneShot(LatvianButtonClickSounds[i]);
                        break;
                    case LanguagesType.Polish:
                        audioSource.PlayOneShot(PolishButtonClickSounds[i]);
                        break;
                    case LanguagesType.Czech:
                        audioSource.PlayOneShot(CzechButtonClickSounds[i]);
                        break;
                    case LanguagesType.Slovak:
                        audioSource.PlayOneShot(SlovakButtonClickSounds[i]);
                        break;
                }
            }
        }
        public void PlayRandomSound(int i)
        {
            if (CheckIfNumericSoundIsEnabled())
            {
                audioSource.PlayOneShot(RandomSounds[i]);
            }
        }
        public void SetLangauage()
        {
            switch (langaugeIndex)
            {
                case 0:
                    currentLanguage = LanguagesType.English;
                    IncrementToLanguage0();

                    // languageButtonText.text = "English";

                    break;
                case 1:
                    currentLanguage = LanguagesType.Lithuanian;
                    IncrementToLanguage1();

                    // languageButtonText.text = "Lithuanian";
                    break;
                case 2:
                    currentLanguage = LanguagesType.Estonian;
                    IncrementToLanguage2();

                    break;
                case 3:
                    currentLanguage = LanguagesType.Latvian;
                    IncrementToLanguage3();
                    break;
                case 4:
                    currentLanguage = LanguagesType.Polish;
                    IncrementToLanguage4();
                    break;
                case 5:
                    currentLanguage = LanguagesType.Czech;
                    IncrementToLanguage5();
                    break;
                case 6:
                    currentLanguage = LanguagesType.Slovak;
                    IncrementToLangaue6();
                    break;
            }
        }

        public void IncrementToLanguage0()
        {
            Handheld.Vibrate();
            langaugeIndex = 0;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;
            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.English;
            TextManagerFind.instance.OnChangeLanguage();
            gamePlayLogo.SetActive(true);
            gamePlayLogo2.SetActive(false);


            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(false);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(true);

        }
        public void IncrementToLanguage1()
        {
            Handheld.Vibrate();

            langaugeIndex = 1;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;

            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.Lithuanian;
            TextManagerFind.instance.OnChangeLanguage();
            gamePlayLogo.SetActive(false);
            gamePlayLogo2.SetActive(true);
            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(true);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(false);

        }
        public void IncrementToLanguage2()
        {
            Handheld.Vibrate();

            langaugeIndex = 2;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;
            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.Estonian;
            TextManagerFind.instance.OnChangeLanguage();

            gamePlayLogo.SetActive(true);
            gamePlayLogo2.SetActive(false);



            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(false);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(true);

        }
        public void IncrementToLanguage3()
        {
            Handheld.Vibrate();

            langaugeIndex = 3;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;
            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.Latvian;
            TextManagerFind.instance.OnChangeLanguage();

            gamePlayLogo.SetActive(true);
            gamePlayLogo2.SetActive(false);



            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(false);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        public void IncrementToLanguage4()
        {
            Handheld.Vibrate();

            langaugeIndex = 4;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;
            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.Polish;
            TextManagerFind.instance.OnChangeLanguage();

            gamePlayLogo.SetActive(true);
            gamePlayLogo2.SetActive(false);
            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(false);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        public void IncrementToLanguage5()
        {
            Handheld.Vibrate();

            langaugeIndex = 5;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;
            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.Czech;
            TextManagerFind.instance.OnChangeLanguage();

            gamePlayLogo.SetActive(true);
            gamePlayLogo2.SetActive(false);
            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(false);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        public void IncrementToLangaue6()
        {
            Handheld.Vibrate();

            langaugeIndex = 6;
            activeLanguageButton.sprite = flags[langaugeIndex].sprite;
            DropDownMenu.SetActive(false);
            Handheld.Vibrate();
            PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            currentLanguage = LanguagesType.Slovak;
            TextManagerFind.instance.OnChangeLanguage();

            gamePlayLogo.SetActive(true);
            gamePlayLogo2.SetActive(false);
            MainLogoContainer.transform.GetChild(0).gameObject.SetActive(false);
            MainLogoContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        public void OpenDropDownMenu()
        {
            Handheld.Vibrate();
            DropDownMenu.SetActive(true);
            // activeLanguageButton.sprite = null;
            // ActiveLanguageBackground.SetActive(false);
        }



        void Start()
        {
            BgmusicSource = this.gameObject.transform.GetChild(0).GetComponent<AudioSource>();


            if (PlayerPrefs.HasKey("PLanguageIndex"))
            {
                langaugeIndex = PlayerPrefs.GetInt("PLanguageIndex");
            }
            else
            {
                langaugeIndex = 0;
                PlayerPrefs.SetInt("PLanguageIndex", langaugeIndex);
            }
            SetLangauage();
            TextManagerFind.instance.OnChangeLanguage();
        }

    }

}
[System.Serializable]
public class CustomFlag
{
    public string name;
    public Sprite sprite;
}