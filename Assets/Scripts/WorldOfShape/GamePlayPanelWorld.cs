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
        public static GamePlayPanelWorld instance;
        public GridLayoutGroup gridLayoutGroup;

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
        void Start()
        {
            CreateQuestion();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void CreateQuestion()
        {
            StartCoroutine(CreateQuestionCoroutine());
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
            gridLayoutGroup.enabled = true;

            if (shapes.Count < 3)
            {
                Debug.LogWarning("Not enough shape groups.");
                yield break;
            }

            List<ColorList> cloneList = new List<ColorList>(shapes);
            List<int> randomIndexList = Select3RandomNo();

            int[] spawnCounts = new int[] { 2, 3, 3 }; // you want 2, 3, and 3 from the 3 selected groups

            for (int group = 0; group < 3; group++)
            {
                int colorListIndex = randomIndexList[group];
                ColorList colorList = cloneList[colorListIndex];

                // 🟩 Set the basket color
                if (group < baskets.Count)
                {
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
                    ans.transform.GetChild(0).name = group.ToString();
                    ans.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

                }
            }
            yield return new WaitForSeconds(0.5f);
            gridLayoutGroup.enabled = false;
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
                return true;
            }
            Debug.Log("Both sprites not found in same question set");
            return false;
        }



    }

}


[System.Serializable]
public class ColorList
{
    public string color;
    public Color basketColor;

    public List<Sprite> shapes;
}



