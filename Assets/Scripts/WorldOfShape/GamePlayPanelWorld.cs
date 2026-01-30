using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.WorldOfShape
{

    public class GamePlayPanelWorld : MonoBehaviour
    {

        public List<ColorList> shapes = new List<ColorList>();
        public List<Image> baskets = new List<Image>();
        public GameObject parent;

        public GameObject AnsPrefab;
        public GameObject BottomContainer;
        public static GamePlayPanelWorld instance;
        public GridLayoutGroup gridLayoutGroup;
        private Coroutine shuffleCoRoutine;
        public int miniLevel = 0;

        public int ansSolvedCount = 0;
        public bool isCreatingQuestion = false;
        public bool currentQuestionSolved = false;
        public Coroutine createQuestionRoutine;





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
            gridLayoutGroup = parent.GetComponent<GridLayoutGroup>();
        }
        // Start is called before the first frame update
        void OnEnable()
        {
            // CreateQuestion();
            StartCoroutine(CheckIfSolveSafe());
        }
        void OnDisable()
        {
            StopAllCoroutines();
        }

        // Update is called once per frame
        void Update()
        {

        }


        // public void CreateQuestion()
        // {
        //     // if (isCreatingQuestion) return;
        //     if(createQuestionRoutine != null) StopCoroutine(createQuestionRoutine);
        //     if(createQuestionRoutine == null)
        //     createQuestionRoutine = StartCoroutine(CreateQuestionCoroutine());
        // }
        public void CreateQuestion()
        {
            if (createQuestionRoutine != null)
            {
                StopCoroutine(createQuestionRoutine);
                createQuestionRoutine = null;
            }

            createQuestionRoutine = StartCoroutine(CreateQuestionCoroutine());
        }

        // IEnumerator CreateQuestionCoroutine()
        // {

        //     List<ColorList> cloneList = new List<ColorList>(shapes); // shallow copy of list references

        //     List<int> randomIndexList = Select3RandomNo();


        //     int randomIndex = Random.Range(0, cloneList.Count);
        //     Debug.Log(cloneList.Count);
        //     int i=0, j=0, k=0;




        //     while (i < 2)
        //     {
        //         GameObject ans = Instantiate(AnsPrefab, transform);
        //         int radomeI= Random.Range(0, cloneList[randomIndexList[0]].shapes.Count);
        //         cloneList[randomIndexList[0]].shapes.RemoveAt(radomeI);
        //         ans.GetComponent<Image>().sprite = cloneList[randomIndexList[0]].shapes[randomIndex];
        //         i++;
        //     }
        //     while (j < 3)
        //     {
        //         GameObject ans = Instantiate(AnsPrefab, transform);
        //         int radomeI= Random.Range(0, cloneList[randomIndexList[1]].shapes.Count);
        //         cloneList[randomIndexList[1]].shapes.RemoveAt(radomeI);
        //         ans.GetComponent<Image>().sprite = cloneList[randomIndexList[1]].shapes[randomIndex];
        //         j++;
        //     }
        //     while (k < 3)
        //     {
        //         GameObject ans = Instantiate(AnsPrefab, transform);
        //         int radomeI= Random.Range(0, cloneList[randomIndexList[2]].shapes.Count);
        //         cloneList[randomIndexList[2]].shapes.RemoveAt(radomeI);
        //         ans.GetComponent<Image>().sprite = cloneList[randomIndexList[2]].shapes[randomIndex];
        //         k++;
        //     }


        // yield return new WaitForSeconds(0.5f);
        // }

        IEnumerator CreateQuestionCoroutine()
        {
            isCreatingQuestion = true;
            Coroutine up = StartCoroutine(ClearChilds());
            yield return up;

            yield return new WaitForSeconds(0.2f);

            if (parent.transform.childCount > 0)
            {
                Debug.LogWarning("Too many children in parent! Forcing cleanup.");
                for (int i = parent.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(parent.transform.GetChild(i).gameObject);
                }
            }
            gridLayoutGroup.enabled = true;

            if (shapes.Count < 3)
            {
                Debug.LogWarning("Not enough shape groups.");
                yield break;
            }

            // List<ColorList> cloneList = new List<ColorList>(shapes);
            List<ColorList> cloneList = new List<ColorList>();
            foreach (ColorList item in shapes)
            {
                ColorList newItem = new ColorList();
                newItem.color = item.color;
                newItem.basketColor = item.basketColor;
                newItem.shapes = new List<Sprite>(item.shapes); // New list of sprites
                newItem.musicIndex = item.musicIndex;
                cloneList.Add(newItem);
            }
            List<int> randomIndexList = Select3RandomNo();

            int[] spawnCounts = new int[] { 2, 3, 3 }; // you want 2, 3, and 3 from the 3 selected groups

            for (int group = 0; group < 3; group++)
            {
                int colorListIndex = randomIndexList[group];
                ColorList colorList = cloneList[colorListIndex];

                // 🟩 Set the basket color
                if (group < baskets.Count)
                {
                    baskets[group].GetComponent<BasketScriptWorld>().musicIndex = colorList.musicIndex;
                    Debug.Log("musicIndex: " + baskets[group].GetComponent<BasketScriptWorld>().musicIndex);
                    baskets[group].color = colorList.basketColor;
                    baskets[group].name = group.ToString();
                }

                for (int i = 0; i < spawnCounts[group]; i++)
                {
                    if (colorList.shapes.Count == 0)
                    {
                        Debug.LogWarning($"Color group {colorList.color} ran out of shapes.");
                        continue;
                    }

                    // Get a random sprite
                    int spriteIndex = Random.Range(0, colorList.shapes.Count);
                    Sprite sprite = colorList.shapes[spriteIndex];
                    colorList.shapes.RemoveAt(spriteIndex); // avoid repeat

                    // Instantiate answer
                    GameObject ans = Instantiate(AnsPrefab, parent.transform);
                    ans.transform.localScale = Vector3.zero;
                    ans.transform.GetChild(0).name = group.ToString();
                    ans.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
                    LeanTween.scale(ans, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);

                    yield return new WaitForSeconds(0.3f);
                }
            }
            ShuffleBottomContainerChildren();
            gridLayoutGroup.enabled = false;
            isCreatingQuestion = false;
            currentQuestionSolved = false;
            createQuestionRoutine = null;
        }

        List<int> Select3RandomNo()
        {
            List<int> list1 = new List<int>();
            while (list1.Count < 3)
            {

                int randomIndex = Random.Range(0, shapes.Count);
                if (!list1.Contains(randomIndex))
                {
                    list1.Add(randomIndex);
                }
            }

            return list1;

        }

        public bool AreSpritesFromSameQuestion(GameObject g1, GameObject g2)
        {
            if (g1.name == g2.name)
            {
                Debug.Log("Both sprites found in same question set");
                // ansSolvedCount++;
                isSolved(8);
                return true;
            }
            Debug.Log("Both sprites not found in same question set");
            return false;
        }
        private void ShuffleBottomContainerChildren()
        {
            shuffleCoRoutine = StartCoroutine(ShuffleBottomContainerChildrenQ());
        }

        // #region ClearHelper
        // public void ClearChilds()
        // {
        //     StartCoroutine(ClearChildsC());
        // }
        // public IEnumerator ClearChildsC()
        // {
        //     if (parent.transform.childCount > 0)
        //     {
        //         Debug.LogWarning("Too many children in parent! Forcing cleanup.");
        //         for (int i = parent.transform.childCount - 1; i >= 0; i--)
        //         {
        //             yield return new WaitForSeconds(0.1f);
        //             Destroy(parent.transform.GetChild(i).gameObject);
        //         }
        //     }
        // }
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
                children[i].localScale = Vector3.zero;
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
                yield return new WaitForSeconds(0.3f);
                // child.transform.localScale = Vector3.one;
                LeanTween.scale(child.gameObject, Vector3.one, 0.5f);
            }

        }



        public void SkipButton()
        {
            // LeanTween.cancelAll();
            // isCreatingQuestion = true;
            Handheld.Vibrate();
            //  StopCoroutine(createQuestionRoutine);

            bool isLevelCleared = GameManagerWorld.instance.IsLevelCleared(9);
            miniLevel = 0;
            if (isLevelCleared)
            {
                GameManagerWorld.instance.LevelClearedPanelSize.gameObject.SetActive(true);
                // ClearChilds();
                StartCoroutine(ClearChilds());
                return;
            }
            AudioManagerWorld.instance.PlayButtonSound(6);

            StarsContainerWorldV2.instance.LevelSkipped(GameManagerWorld.instance.clearedLevels);
            GameManagerWorld.instance.clearedLevels++;
            // isCreatingQuestion = false;
            if (isCreatingQuestion == false && createQuestionRoutine == null)
            {
                CreateQuestion();
            }

        }

public void CallClearChilds()
        {
            StartCoroutine(ClearChilds());
        }
        private IEnumerator ClearChilds()
        {
            for (int i = parent.transform.childCount - 1; i >= 0; i--)
            {
                LeanTween.scale(parent.transform.GetChild(i).gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
                {
                    Destroy(parent.transform.GetChild(i).gameObject);
                });
                yield return new WaitForSeconds(0.3f);
            }
            // yield return new WaitForSeconds(0.3f);
            for (int i = BottomContainer.transform.childCount - 1; i >= 0; i--)
            {
                LeanTween.scale(BottomContainer.transform.GetChild(i).gameObject, Vector3.zero, 0.5f);
                yield return new WaitForSeconds(0.3f);
                // foreach (Transform child in BottomContainer.transform.GetChild(i).transform.GetChild(0).transform)
                // {
                //     Transform myChild = child;
                //     DestroyImmediate(myChild.gameObject);
                //     yield return new WaitForEndOfFrame();
                // }
                for (int j = BottomContainer.transform.GetChild(i).transform.GetChild(0)
                .transform.childCount - 1; j >= 0; j--)
                {
                    Transform myChild = BottomContainer.transform.GetChild(i).transform.GetChild(0).transform.GetChild(j);
                    Destroy(myChild.gameObject);
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(0.3f);
            }
        }



        public IEnumerator CheckIfSolveSafe()
        {
            while (true)
            {

                yield return new WaitForSeconds(1f);
                Debug.Log("CheckIfSolveSafe");
                isSolved(8);
            }
        }


        public void isSolved(int ans)
        {
            if (isCreatingQuestion || currentQuestionSolved)
                return;

            int count = 0;
            for (int i = BottomContainer.transform.childCount - 1; i >= 0; i--)
            {
                Transform basket = BottomContainer.transform.GetChild(i).GetChild(0);
                Debug.Log($"Basket {i} contains {basket.childCount} children");
                count += basket.childCount;
            }

            if (count >= ans)
            {
                currentQuestionSolved = true; // 🔐 Lock this question as solved
                // StarsContainerWorldV2.instance.MiniLevelCleared(GameManagerWorld.instance.clearedLevels, miniLevel);
                // miniLevel++;
                GameManagerWorldV2.instance.IncrMiniLevel();

                // if (miniLevel >= StarsContainerWorld.MINI_LEVEL_TARGET)
                // {
                //     miniLevel = 0;

                //     bool isLevelCleared = GameManagerWorld.instance.IsLevelCleared(9);
                //     StarsContainerWorldV2.instance.LevelCleared(GameManagerWorld.instance.clearedLevels);
                //     if (isLevelCleared)
                //     {
                //         GameManagerWorld.instance.LevelClearedPanelSize.gameObject.SetActive(true);
                //         return;
                //     }
                //     GameManagerWorld.instance.clearedLevels++;
                // }

                // CreateQuestion();
            }
        }

    }
}

[System.Serializable]
public class ColorList
{
    public string color;
    public Color basketColor;

    public List<Sprite> shapes;
    public int musicIndex;
}



