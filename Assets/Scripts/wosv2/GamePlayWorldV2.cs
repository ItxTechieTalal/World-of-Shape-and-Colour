


using System.Collections;
using System.Collections.Generic;
using MiniGame.WorldOfShape;
using UnityEngine;

public class GamePlayWorldV2 : MonoBehaviour
{
    [SerializeField] private RectTransform mainCont;
    [SerializeField] private RectTransform itemPrefab; // use RectTransform prefab

    [Header("Spacing (pixels)")]
    [SerializeField] private float minDistance = 120f;

    [Header("Spawn Settings")]
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private int maxTriesPerSpawn = 80;
    [SerializeField] private float padding = 0f; // keep away from edges if needed
    [SerializeField] private float borderGap = 40f; // distance from edges

    private readonly List<RectTransform> spawned = new();
    public List<int> selectedParents = new List<int>();
    public List<int> selectedChilds = new List<int>();
    public Transform sideContainer;
    public Transform bottomContainer;
    public static GamePlayWorldV2 instance;
    public bool levelSolved = false;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        // IMPORTANT: If you have LayoutGroup on mainCont, positions will be overridden.
        // Disable it for testing.
        // var lg = mainCont.GetComponent<UnityEngine.UI.LayoutGroup>();
        // if (lg) lg.enabled = false;

        SpawnRandom();
    }

    #region  SpawnRandom
    Coroutine spawnRandomCoroutine;
    [ContextMenu("Spawn Random")]
    public void SpawnRandom()
    {
        if (spawnRandomCoroutine != null)
        {
            StopCoroutine(spawnRandomCoroutine);
            spawnRandomCoroutine = null;
        }
        if (spawnRandomCoroutine == null)
        {

            spawnRandomCoroutine = StartCoroutine(SpawnRandomC());
        }


    }
    IEnumerator SpawnRandomC()
    {
        CleanAll();        //select random frames
        selectedParents.Clear();
        selectedChilds.Clear();

        //select Parents
        while (selectedParents.Count < 3)
        {
            int random = Random.Range(0, ItemManagerWorldV2.instance.parents.Count);
            if (!selectedParents.Contains(random))
            {
                selectedParents.Add(random);
            }
        }
        while (selectedChilds.Count < 3)
        {
            int random = Random.Range(0, ItemManagerWorldV2.instance.children.Count);
            if (!selectedChilds.Contains(random))
            {
                selectedChilds.Add(random);
            }
        }

        yield return new WaitForSeconds(1.7f);
        //Cleaning
        spawned.Clear();
        foreach (Transform child in mainCont.transform) Destroy(child.gameObject);
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (i < 3)
                SpawnRandomHelper(selectedChilds[0], 0);
            else if (i >= 3 && i <= 5)
                SpawnRandomHelper(selectedChilds[1], 1);
            else
            {
                SpawnRandomHelper(selectedChilds[2], 2);

            }


        }

        SpawnRightSideElements();
        yield return new WaitForSeconds(0.5f);
        BottomElements();
        yield return new WaitForSeconds(0.5f);
        spawnRandomCoroutine = null;


    }
    #endregion

    #region Bottom Elements
    Coroutine bottomElementsCoroutine;
    public void BottomElements()
    {
        if (bottomElementsCoroutine != null)
        {
            StopCoroutine(bottomElementsCoroutine);
            bottomElementsCoroutine = null;
        }
        if (bottomElementsCoroutine == null)
            bottomElementsCoroutine = StartCoroutine(BottomElementsC());
    }
    IEnumerator BottomElementsC()
    {
        foreach (Transform child in bottomContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            RectTransform item = Instantiate(itemPrefab, bottomContainer);
            Wos2Item itemS = item.GetComponent<Wos2Item>();
            Sprite parent = ItemManagerWorldV2.instance.parents[selectedParents[i]];
            Sprite child = ItemManagerWorldV2.instance.children[selectedChilds[i]];
            itemS.SetParentAndChild(parent, child);


        }
        bottomElementsCoroutine = null;
    }
    #endregion
    #region Right Side Elements

    Coroutine rightSideElementsCoroutine;
    public void SpawnRightSideElements()
    {
        if (rightSideElementsCoroutine != null)
        {
            StopCoroutine(rightSideElementsCoroutine);
            rightSideElementsCoroutine = null;
        }
        if (rightSideElementsCoroutine == null)
            rightSideElementsCoroutine = StartCoroutine(SpawnRightSideElementsC());
    }
    IEnumerator SpawnRightSideElementsC()
    {
        foreach (Transform child in sideContainer)
        {
            Destroy(child.gameObject);
            Debug.Log("i am destroying");
        }
        int i = 0;

        foreach (int parentsIndex in selectedParents)
        {
            yield return new WaitForSeconds(0.1f);
            RectTransform item = Instantiate(itemPrefab, sideContainer);
            item.gameObject.name = i.ToString();
            item.GetChild(0).gameObject.name = i.ToString();
            item.GetChild(0).tag = "Parent";
            Wos2Item itemS = item.GetComponent<Wos2Item>();
            itemS.SetParentOnly(ItemManagerWorldV2.instance.parents[parentsIndex]);
            i++;
        }
    }
    #endregion
    public void SpawnRandomHelper(int i, int j)
    {
        // Debug.LogError("spawnRandomHelper");
        RectTransform item = Instantiate(itemPrefab, mainCont);
        item.name = j.ToString();
        item.gameObject.layer = LayerMask.NameToLayer("Child");
        Wos2Item itemS = item.GetComponent<Wos2Item>();
        // Sprite parent = ItemManagerWorldV2.instance.parents[Random.Range(0, ItemManagerWorldV2.instance.parents.Count)];
        Sprite child = ItemManagerWorldV2.instance.children[i];
        item.tag = "Child";


        itemS.SetChildOnly(child);

        item.gameObject.SetActive(true);

        item.anchorMin = new Vector2(0.5f, 0.5f);
        item.anchorMax = new Vector2(0.5f, 0.5f);
        item.pivot = new Vector2(0.5f, 0.5f);

        if (TryGetFreePosition(item, out Vector2 pos))   // ✅ pass item
        {
            item.anchoredPosition = pos;
            spawned.Add(item);
        }
        else
        {
            item.anchoredPosition = Vector2.zero;
            spawned.Add(item);
            Debug.LogWarning("No free spot found. Reduce minDistance/spawnCount or increase container size.");
        }
    }


    public void CheckIfAnyChildQuestionSolved()
    {
        //  all true at start
        bool isZeroSolved = true;
        bool isOneSolved = true;
        bool isTwoSolved = true;

        //false them if any not solved 
        foreach (Transform child in mainCont.transform)
        {
            Wos2Item itemS = child.GetComponent<Wos2Item>();
            if (itemS.name == "0")
            {
                if (!itemS.solved)
                {
                    isZeroSolved = false;
                }

            }
            else if (itemS.name == "1")
            {
                if (!itemS.solved)
                {
                    isOneSolved = false;
                }

            }
            else if (itemS.name == "2")
            {
                if (!itemS.solved)
                {
                    isTwoSolved = false;
                }
            }



        }
        if (isZeroSolved && isOneSolved && isTwoSolved)
        {
            levelSolved = true;
            GameManagerWorldV2.instance.IncrMiniLevel();
        }
        if (isZeroSolved)
        {
            sideContainer.GetChild(0).gameObject.SetActive(false);
        }
        if (isOneSolved)
        {
            sideContainer.GetChild(1).gameObject.SetActive(false);
        }
        if (isTwoSolved)
        {
            sideContainer.GetChild(2).gameObject.SetActive(false);
        }
    }

    #region CleanCOntianer
    IEnumerator ClearContainer(Transform container)
    {
        if (container == null)
            yield break;

        for (int i = container.childCount - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(container.GetChild(i).gameObject);
        }
    }
    public void CleanAll()
    {
        levelSolved = false;

        spawned.Clear();
        selectedParents.Clear();
        selectedChilds.Clear();

        ClearContainer(mainCont);
        ClearContainer(sideContainer);
        ClearContainer(bottomContainer);
    }
    #endregion

    // public void SpawnRandomHelper()
    // {

    //     // clearing before proceeding


    //     //Instantiating
    //     RectTransform item = Instantiate(itemPrefab, mainCont);
    //     item.gameObject.SetActive(true);

    //     // Ensure predictable UI positioning
    //     item.anchorMin = new Vector2(0.5f, 0.5f);
    //     item.anchorMax = new Vector2(0.5f, 0.5f);
    //     item.pivot     = new Vector2(0.5f, 0.5f);

    //     if (TryGetFreePosition(out Vector2 pos))
    //     {
    //         item.anchoredPosition = pos;
    //         spawned.Add(item);
    //     }
    //     else
    //     {
    //         // Could not find room (container too small or minDistance too big)
    //         item.anchoredPosition = Vector2.zero;
    //         spawned.Add(item);
    //         Debug.LogWarning($"No free spot found. Try reducing minDistance or spawnCount, or increase container size.");
    //     }
    // }

    // private bool TryGetFreePosition(out Vector2 pos)
    // {
    //     Vector2 size = mainCont.rect.size;

    //     // keep items inside bounds (with padding)
    //     float halfW = size.x * 0.5f - padding;
    //     float halfH = size.y * 0.5f - padding;

    //     for (int t = 0; t < maxTriesPerSpawn; t++)
    //     {
    //         float x = Random.Range(-halfW, halfW);
    //         float y = Random.Range(-halfH, halfH);
    //         Vector2 candidate = new Vector2(x, y);

    //         if (!HasCollision(candidate))
    //         {
    //             pos = candidate;
    //             return true;
    //         }
    //     }

    //     pos = Vector2.zero;
    //     return false;
    // }

    private bool TryGetFreePosition(RectTransform item, out Vector2 pos)
    {
        Vector2 contSize = mainCont.rect.size;
        Vector2 itemSize = item.rect.size; // item width/height in pixels

        // bounds so item stays fully inside + extra border gap
        float halfW = contSize.x * 0.5f - padding - (itemSize.x * 0.5f) - borderGap;
        float halfH = contSize.y * 0.5f - padding - (itemSize.y * 0.5f) - borderGap;

        // if container too small for this gap
        if (halfW <= 0f || halfH <= 0f)
        {
            pos = Vector2.zero;
            return false;
        }

        for (int t = 0; t < maxTriesPerSpawn; t++)
        {
            float x = Random.Range(-halfW, halfW);
            float y = Random.Range(-halfH, halfH);
            Vector2 candidate = new Vector2(x, y);

            if (!HasCollision(candidate))
            {
                pos = candidate;
                return true;
            }
        }

        pos = Vector2.zero;
        return false;
    }

    private bool HasCollision(Vector2 candidate)
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (Vector2.Distance(spawned[i].anchoredPosition, candidate) < minDistance)
                return true;
        }
        return false;
    }
}
