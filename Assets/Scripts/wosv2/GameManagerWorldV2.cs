
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiniGame.WorldOfShape
{

    public class GameManagerWorldV2 : MonoBehaviour
    {

        public static GameManagerWorldV2 instance;
        public int clearedLevels = 0;
        public GameObject LevelClearedPanelSize;
        public GameObject GamePlayPanelSize;
        public GameObject InitialPanelSize;
        public GameObject HowToPlayPanelSize;
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



        public void StartTheGame()
        {
            Handheld.Vibrate();
            AudioManagerWorld.instance.PlayButtonSound(0);

            // GamePlayPanelSize.SetActive(true);
            InitialPanelSize.SetActive(false);
            // GamePlayPanelWorld.instance.CreateQuestion();
            // GamePlayPanelWorld.instance.PlayMonitorContainer();
            ApplySelectedGame();
        }

        public void ActiveHowToPlayPanel()
        {
            Handheld.Vibrate();
            HowToPlayPanelSize.SetActive(true);
            AudioManagerWorld.instance.PlayButtonSound(1);
        }
        public void SolverTheProblemSound()
        {
            Handheld.Vibrate();

            AudioManagerWorld.instance.PlayButtonSound(1);
        }
        #region ManageGamePlay
        [Header("Game Roots")]
        public GameObject Game1Root; // GamePlayPanelWorld whole UI/root
        public GameObject Game2Root; // GamePlayWorldV2 whole UI/root


        // public void ApplySelectedGame()
        // {
        //     // always hide both first
        //     if (Game1Root != null) Game1Root.SetActive(false);
        //     if (Game2Root != null) Game2Root.SetActive(false);

        //     int selected = GetSelectedGame();

        //     // fallback rules
        //     if (selected == 0 )
        //         selected = 1; // default Game 1

        //     // fix toggle UI so only one stays ON
        //     if (Game_One_Toggle != null) Game_One_Toggle.isOn = (selected == 1);
        //     if (Game_Two_Toggle != null) Game_Two_Toggle.isOn = (selected == 2);

        //     // open correct game
        //     if (selected == 1)
        //     {
        //         if (Game1Root != null)
        //             Game1Root.SetActive(true);

        //         // start Game 1 logic
        //         if (GamePlayPanelWorld.instance != null)
        //             GamePlayPanelWorld.instance.CreateQuestion();
        //     }
        //     else if (selected == 2)
        //     {
        //         if (Game2Root != null)
        //             Game2Root.SetActive(true);

        //         // start Game 2 logic
        //         if (GamePlayWorldV2.instance != null)
        //             GamePlayWorldV2.instance.SpawnRandom();
        //     }
        //     else if (selected == 3)
        //     {
        //         int random = Random.Range(0, 2);
        //         if (random == 0)
        //         {
        //             if (Game1Root != null)
        //                 Game1Root.SetActive(true);
        //             GamePlayPanelWorld.instance.CreateQuestion();


        //         }
        //         else
        //         {
        //             if (Game2Root != null)
        //                 Game2Root.SetActive(true);
        //             GamePlayWorldV2.instance.SpawnRandom();

        //         }
        //     }
        // }

        public void ApplySelectedGame()
        {
            // always hide both first
            if (Game1Root != null) Game1Root.SetActive(false);
            if (Game2Root != null) Game2Root.SetActive(false);

            int selected = GetSelectedGame();

            // fallback: none => game 1
            if (selected == 0)
                selected = 1;

            // both => random pick 1 or 2
            if (selected == 3)
                selected = Random.Range(1, 3); // 1 or 2

            // fix toggle UI (now selected is guaranteed 1 or 2)
            // if (Game_One_Toggle != null) Game_One_Toggle.isOn = (selected == 1);
            // if (Game_Two_Toggle != null) Game_Two_Toggle.isOn = (selected == 2);

            // open correct game
            if (selected == 1)
            {
                if (Game1Root != null) Game1Root.SetActive(true);
                GamePlayPanelWorld.instance?.CreateQuestion();
            }
            else // selected == 2
            {
                if (Game2Root != null) Game2Root.SetActive(true);
                GamePlayWorldV2.instance?.SpawnRandom();
            }
        }


        #endregion

        #region miniLevel
        public int miniLevel = 0;
        [ContextMenu("Increment Mini Level")]
        public void IncrMiniLevel()
        {
            if (miniLevel >= StarsContainerWorldV2.MINI_LEVEL_TARGET)
            {
                if (IsLevelCleared())
                {
                    LevelClearedPanelSize.SetActive(true);
                    return;
                }

                clearedLevels++;

                ResetMiniLevel();
            }
            StarsContainerWorldV2.instance.MiniLevelCleared(clearedLevels, miniLevel);
            miniLevel++;
            // GamePlayWorldV2.instance.SpawnRandom();
            ApplySelectedGame();
        }
        public int GetMiniLevel()
        {
            return miniLevel;
        }

        public void ResetMiniLevel()
        {
            miniLevel = 0;

        }
        public void CloseHowToPlayPanel()
        {
            Handheld.Vibrate();
            HowToPlayPanelSize.SetActive(false);
        }

        public void OpenInitialPanel()
        {
            Handheld.Vibrate();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion

        public bool IsLevelCleared(int count = 9)
        {
            return clearedLevels >= count;

        }


        #region SelectionPanel
        public GameObject SelectionPanel;
        public Toggle Game_One_Toggle;
        public Toggle Game_Two_Toggle;



        public void OpenSlectionPanel()
        {
            SelectionPanel.SetActive(true);
            Handheld.Vibrate();
        }
        public void CloseSelectionPanel()
        {
            SelectionPanel.SetActive(false);
            Handheld.Vibrate();

        }
        public int GetSelectedGame()
        {
            if (Game_One_Toggle.isOn && !Game_Two_Toggle.isOn)
            {
                return 1; // Game 1 selected
            }
            else if (Game_Two_Toggle.isOn && !Game_One_Toggle.isOn)
            {
                return 2; // Game 2 selected
            }
            else if (Game_One_Toggle.isOn && Game_Two_Toggle.isOn)
            {
                return 3; // both on (if you allow this)
            }
            else
            {
                return 0; // none selected
            }
        }




        #endregion

        #region SingleFuncs
        // public void SkipButton()
        // {
        //     // LeanTween.cancelAll();
        //     // isCreatingQuestion = true;
        //     Handheld.Vibrate();
        //     //  StopCoroutine(createQuestionRoutine);

        //     bool isLevelCleared = IsLevelCleared(9);
        //     ResetMiniLevel();
        //     // GamePlayWorldV2.instance.SpawnRandom();
        //     ApplySelectedGame();

        //     if (isLevelCleared)
        //     {
        //         LevelClearedPanelSize.gameObject.SetActive(true);
        //         // ClearChilds();
        //         // StartCoroutine(ClearChilds());
        //         return;
        //     }
        //     AudioManagerWorld.instance.PlayButtonSound(6);

        //     StarsContainerWorld.instance.LevelSkipped(clearedLevels);
        //     clearedLevels++;
        //     // isCreatingQuestion = false;
        //     // if (isCreatingQuestion == false && createQuestionRoutine == null)
        //     // {
        //     //     CreateQuestion();
        //     //  }

        // }

        public void SkipButton()
        {
            Handheld.Vibrate();

            // If already at end, just show cleared panel
            if (IsLevelCleared(9))
            {
                LevelClearedPanelSize.SetActive(true);
                return;
            }

            AudioManagerWorld.instance.PlayButtonSound(6);

            // UI / stars for skip (use current clearedLevels before increment)
            StarsContainerWorld.instance.LevelSkipped(clearedLevels);

            // Move to next level
            clearedLevels++;

            // Reset mini level progress for next level
            ResetMiniLevel();

            // Now load/spawn the next level content
            ApplySelectedGame();
        }

        #endregion

    }



}