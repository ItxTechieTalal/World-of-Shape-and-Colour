using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MiniGame.FindAndMatch
{
    public class GamePlayPanelFind : MonoBehaviour
    {
        public int currentGridSize;
        public GameObject MainContainer;
        public GameObject BottomContainer;
        public GameObject QPrefab;
        public GameObject APrefab;
        public List<GameObject> ImagePrefabs = new List<GameObject>();
        public List<FindAndMatchList> findAndMatchLists = new List<FindAndMatchList>();
        public static GamePlayPanelFind instance;
        public bool shouldPlay = false;
        public int miniLevel = 0;
        public bool isCreatingQuestion = false;
        private Coroutine createQuestionRoutine;
        private Coroutine shuffleCoRoutine;





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
        void Start()
        {
            // AreSpritesFromSameQuestion(findAndMatchLists[0].spriteQ, findAndMatchLists[0].spriteA1);


        }

        void Update()
        {
            // if (!isCreatingQuestion && BottomContainer.transform.childCount <= 0)
            // {
            //     StarsContainerFind.instance.MiniLevelCleared(GameManagerFind.instance.clearedLevels, miniLevel);
            //     miniLevel++;
            //     if (miniLevel >= StarsContainerFind.MINI_LEVEL_TARGET)
            //     {
            //         miniLevel = 0;
            //         StarsContainerFind.instance.LevelCleared(GameManagerFind.instance.clearedLevels);
            //         GameManagerFind.instance.clearedLevels++;
            //     }

            //     CreateQuestion();
            // }
        }

        // public IEnumerator MonitorContainer()
        // {
        //     while (true)
        //     {
        //         yield return new WaitForSeconds(0.2f);
        //         if (!isCreatingQuestion && BottomContainer.transform.childCount <= 0)
        //         {
        //             StarsContainerFind.instance.MiniLevelCleared(GameManagerFind.instance.clearedLevels, miniLevel);
        //             miniLevel++;
        //             if (miniLevel >= StarsContainerFind.MINI_LEVEL_TARGET)
        //             {
        //                 miniLevel = 0;
        //                 bool isLevelCleared = GameManagerFind.instance.IsLevelCleared(9);

        //                 if (isLevelCleared)
        //                 {
        //                     GameManagerFind.instance.LevelClearedPanelSize.gameObject.SetActive(true);
        //                     yield break; // Stop the coroutine since level is fully cleared
        //                 }
        //                 StarsContainerFind.instance.LevelCleared(GameManagerFind.instance.clearedLevels);
        //                 GameManagerFind.instance.clearedLevels++;
        //             }

        //             CreateQuestion();
        //         }
        //     }
        // }


        public void PlayMonitorContainer()
        {
            StartCoroutine(MonitorContainer());
        }
        public IEnumerator MonitorContainer()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);


                if (!isCreatingQuestion && BottomContainer.transform.childCount <= 0)
                {
                    StarsContainerFind.instance.MiniLevelCleared(GameManagerFind.instance.clearedLevels, miniLevel);
                    miniLevel++;
                    if (miniLevel >= StarsContainerFind.MINI_LEVEL_TARGET)
                    {
                        miniLevel = 0;
                        bool isLevelCleared = GameManagerFind.instance.IsLevelCleared(9);

                        if (isLevelCleared)
                        {
                            GameManagerFind.instance.LevelClearedPanelSize.gameObject.SetActive(true);
                            // yield break; // Stop coroutine when level finishes
                            continue;
                        }
                        StarsContainerFind.instance.LevelCleared(GameManagerFind.instance.clearedLevels);
                        GameManagerFind.instance.clearedLevels++;
                    }

                    CreateQuestion();
                }
            }
        }




        public void CreateQuestion()
        {
            if (shuffleCoRoutine != null)
            {
                StopCoroutine(shuffleCoRoutine);
                shuffleCoRoutine = null;
            }


            // if (isCreatingQuestion)
            // {
            //     Debug.Log("CreateQuestion was called while creating, stopping previous routine");
            //     if (createQuestionRoutine != null)
            //     {
            //         StopCoroutine(createQuestionRoutine);
            //     }
            //     isCreatingQuestion = false;
            //     SafeDestroy();
            //     return;
            // }

            if (isCreatingQuestion)
            {
                Debug.Log("CreateQuestion was called while creating, stopping previous routine");
                if (createQuestionRoutine != null)
                {
                    StopCoroutine(createQuestionRoutine);
                }
                isCreatingQuestion = false;
                SafeDestroy();
                // return;
            }


            isCreatingQuestion = true;

            // Instead of StopAllCoroutines(), stop only this coroutine if needed
            if (createQuestionRoutine != null)
            {
                StopCoroutine(createQuestionRoutine);
            }

            createQuestionRoutine = StartCoroutine(CreateQuestionQ());
        }

       

        public IEnumerator CreateQuestionQ()
        {
            yield return new WaitForEndOfFrame();

            isCreatingQuestion = true;

            List<GameObject> ImagePrefabsT = new List<GameObject>(ImagePrefabs);

            if (MainContainer.transform.childCount > 0)
            {
                yield return new WaitForSeconds(0.8f);
                Coroutine up = StartCoroutine(DestroyChildsUp());
                Coroutine bottom = StartCoroutine(DestroyChildsBottom());
                yield return up;
                yield return bottom;
                yield return new WaitForSeconds(0.2f);
            }

            SafeDestroy();

            bool use2Columns = Random.value > 0.5f;

            GridLayoutGroup mainGrid = MainContainer.GetComponent<GridLayoutGroup>();
            if (use2Columns)
            {
                mainGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                mainGrid.constraintCount = 2;
                BottomContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(177f, 0f);
                MainContainer.GetComponent<GridLayoutGroup>().padding = new RectOffset(234, 0, -36, 0);
            }
            else
            {
                mainGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                mainGrid.constraintCount = 3;
                BottomContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(75f, 0f);
                MainContainer.GetComponent<GridLayoutGroup>().padding = new RectOffset(53, 0, -36, 0);
            }

            yield return new WaitForEndOfFrame();

            int count = use2Columns ? 4 : 6;
            for (int i = 0; i < count; i++)
            {
                if (ImagePrefabsT.Count == 0)
                    break;

                int randomImg = Random.Range(0, ImagePrefabsT.Count);

                GameObject QContainer = Instantiate(ImagePrefabsT[randomImg], MainContainer.transform);
                if (QContainer == null)
                {
                    Debug.LogWarning("QContainer was null right after Instantiate.");
                    continue;
                }

                QContainer.transform.localScale = Vector3.zero;
                ImagePrefabsT.RemoveAt(randomImg);

                yield return new WaitForSeconds(0.2f);

                if (QContainer == null)
                {
                    Debug.LogWarning("QContainer destroyed during wait. Skipping.");
                    continue;
                }

                LeanTween.scale(QContainer, Vector3.one, 0.2f);

                GameObject childToUse = null;
                if (Random.value > 0.5f)
                {
                    childToUse = QContainer.transform.GetChild(1).gameObject;
                }
                else
                {
                    childToUse = QContainer.transform.GetChild(2).gameObject;
                }

                if (childToUse != null)
                {
                    childToUse.SetActive(false);

                    GameObject AContainer = Instantiate(APrefab, BottomContainer.transform);
                    if (AContainer != null)
                    {
                        AContainer.transform.localScale = Vector3.zero;
                        AContainer.GetComponent<Image>().sprite = childToUse.GetComponent<Image>().sprite;
                        AContainer.gameObject.name = i.ToString();
                    }
                }

                QContainer.gameObject.name = i.ToString();
            }

            isCreatingQuestion = false;

            ShuffleBottomContainerChildren();
        }






      

        public void PlayAnswerMusic()
        {
            int random = Random.Range(0, 6);
            if (random == 0)
            {
                AudioManagerFind.instance.PlayButtonSound(4);
            }
            else if (random == 1)
            {
                AudioManagerFind.instance.PlayButtonSound(10);
            }
            else if (random == 2)
            {
                AudioManagerFind.instance.PlayButtonSound(11);
            }
            else if (random == 3)
            {
                AudioManagerFind.instance.PlayButtonSound(7);
            }
            else if (random == 4)
            {
                AudioManagerFind.instance.PlayButtonSound(9);
            }
            else if (random == 5)
            {
                AudioManagerFind.instance.PlayButtonSound(12);
            }

        }



        public void LoadClearPanel()
        {
            GameManagerFind.instance.LevelClearedPanelSize.gameObject.SetActive(true);

        }



        IEnumerator DestroyChildsUp()
        {
            // isCreatingQuestion = true;
            MainContainer.GetComponent<GridLayoutGroup>().enabled = false;


            List<Transform> children = new List<Transform>();
            foreach (Transform child in MainContainer.transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                LeanTween.scale(child.gameObject, Vector3.zero, 0.2f);
                yield return new WaitForSeconds(0.23f);
                if (child != null)
                {
                    if (shuffleCoRoutine != null)
                    {
                        StopCoroutine(shuffleCoRoutine);
                        shuffleCoRoutine = null;
                    }

                    LeanTween.cancel(child.gameObject);

                    Destroy(child.gameObject);
                }
            }

            MainContainer.GetComponent<GridLayoutGroup>().enabled = true;
            // isCreatingQuestion = false;
        }



        IEnumerator DestroyChildsBottom()
        {
            // isCreatingQuestion = true;
            BottomContainer.GetComponent<GridLayoutGroup>().enabled = false;

            // ✅ Cache children
            List<Transform> children = new List<Transform>();
            foreach (Transform child in BottomContainer.transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                LeanTween.scale(child.gameObject, Vector3.zero, 0.2f);
                yield return new WaitForSeconds(0.23f);
                if (child != null)
                {
                    if (shuffleCoRoutine != null)
                    {
                        StopCoroutine(shuffleCoRoutine);
                        shuffleCoRoutine = null;
                    }

                    LeanTween.cancel(child.gameObject);

                    Destroy(child.gameObject);
                }
            }

            BottomContainer.GetComponent<GridLayoutGroup>().enabled = true;
            // isCreatingQuestion = false;
        }




        public void SafeDestroy()
        {
            if (MainContainer.transform.childCount > 0)
            {
                foreach (Transform child in MainContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if (BottomContainer.transform.childCount > 0)
            {
                foreach (Transform child in BottomContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        public void SafeDestroy2()
        {
            if (MainContainer.transform.childCount > 0)
            {
                foreach (Transform child in MainContainer.transform)
                {
                    DestroyImmediate(child.gameObject); // <-- changed
                }
            }

            if (BottomContainer.transform.childCount > 0)
            {
                foreach (Transform child in BottomContainer.transform)
                {
                    DestroyImmediate(child.gameObject); // <-- changed 
                }
            }
        }

        public bool AreSpritesFromSameQuestion(GameObject g1, GameObject g2)
        {
            if (g1.name == g2.name)
            {
                return true;
            }
            Debug.Log("Both sprites not found in same question set");
            return false;
        }


        public void SkipButton()
        {
            LeanTween.cancelAll();
            isCreatingQuestion = true;
            Handheld.Vibrate();
            // StopCoroutine(createQuestionRoutine);

            bool isLevelCleared = GameManagerFind.instance.IsLevelCleared(9);
            miniLevel = 0;
            if (isLevelCleared)
            {
                GameManagerFind.instance.LevelClearedPanelSize.gameObject.SetActive(true);
                return;
            }
            AudioManagerFind.instance.PlayButtonSound(6);

            StarsContainerFind.instance.LevelSkipped(GameManagerFind.instance.clearedLevels);
            GameManagerFind.instance.clearedLevels++;
            isCreatingQuestion = false;
            CreateQuestion();

        }
        private void ShuffleBottomContainerChildren()
        {
            shuffleCoRoutine = StartCoroutine(ShuffleBottomContainerChildrenQ());
        }

        private IEnumerator ShuffleBottomContainerChildrenQ()
        {
            // Get all children transforms
            List<Transform> children = new List<Transform>();
            foreach (Transform child in BottomContainer.transform)
            {
                children.Add(child);
            }

            if (children.Count == 0)
            {
                yield break; // Nothing to shuffle
            }

            // Fisher-Yates shuffle
            for (int i = 0; i < children.Count; i++)
            {
                int randomIndex = Random.Range(i, children.Count);
                // Swap
                Transform temp = children[i];
                children[i] = children[randomIndex];
                children[randomIndex] = temp;
            }

            // Re-assign sibling indices
            for (int i = 0; i < children.Count; i++)
            {
                children[i].SetSiblingIndex(i);

            }
            // yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();
            foreach (Transform child in BottomContainer.transform)
            {
                yield return new WaitForSeconds(0.1f);
                // child.transform.localScale = Vector3.one;
                LeanTween.scale(child.gameObject, Vector3.one, 0.2f);
            }

        }

    }




}

[System.Serializable]
public class FindAndMatchList
{
    public Sprite spriteQ;
    public Sprite spriteA1;
    public Sprite spriteA2;

}












