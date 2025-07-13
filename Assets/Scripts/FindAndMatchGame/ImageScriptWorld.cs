using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace MiniGame.WorldOfShape

{
    public class ImageScriptWorld : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas canvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Transform originalParent;
        private Vector2 initialAnchoredPos;
        public Image myImage;
        public bool canBeDraged = true;
        private bool isSnapping = false;



        private void Awake()
        {
            myImage = transform.GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>(); // ✅ Needed to enable/disable raycasts

            // initialPos = rectTransform;

            // Find the canvas automatically
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("No Canvas found in parent hierarchy!");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalParent = transform.parent;
            initialAnchoredPos = rectTransform.anchoredPosition;

            canvasGroup.blocksRaycasts = false;
            // Allow drop detection
            // Canvas localCanvas = GetComponent<Canvas>();
            // if (localCanvas != null)
            // {
            //     localCanvas.overrideSorting = true;
            //     localCanvas.sortingOrder = 1000;
            // }

            transform.parent.SetAsLastSibling();



        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!canBeDraged || isSnapping) return;

            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            GameObject[] containers = GameObject.FindGameObjectsWithTag("Container");

            foreach (GameObject container in containers)
            {
                RectTransform containerRect = container.GetComponent<RectTransform>();
                Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);

                // Check if close enough
                float distance = Vector2.Distance(rectTransform.position, containerRect.position);
                if (distance < 100f) // 💡 Adjust proximity threshold
                {
                    bool isSameQuestion = GamePlayPanelWorld.instance.AreSpritesFromSameQuestion(this.gameObject, container);

                    if (isSameQuestion)
                    {
                        isSnapping = true;
                        StartCoroutine(SmoothSnap(container));
                        break;
                    }
                }
            }
        }


        private IEnumerator SmoothSnap(GameObject container)
        {
            Canvas localCanvas = GetComponent<Canvas>();
            if (localCanvas != null)
            {
                localCanvas.overrideSorting = false;
            }

            canBeDraged = false;

            Transform slot = container.transform.GetChild(0); // Your target slot
                                                              // Vector3 targetPos = slot.TransformPoint(new Vector3(30, -76, 0));
            Vector3 targetPos = slot.position;

            // Optional: Shrink for realism
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = Vector3.one;
            // Vector3 targetScale = Vector3.one * 0.55f;

            // Move and scale smoothly (0.4s)
            float duration = 0.4f;
            float elapsed = 0f;
            Vector3 startPos = transform.position;

            transform.SetParent(slot);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }

            // Final placement
            transform.localPosition = new Vector3(0, -150, 0);
            transform.localScale = targetScale;
        }



        public void OnEndDrag(PointerEventData eventData)
        {

            // canvas.overrideSorting = false;

            canvasGroup.blocksRaycasts = true;

            // If dropped on a valid target (tagged "Container"), snap into place
            if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Container"))
            {
                Image img = eventData.pointerEnter.transform.GetComponent<Image>();
                // Sprite img2 = eventData.pointerEnter.transform.GetChild(2).GetComponent<Image>().sprite;
                bool isSameQuestion = GamePlayPanelWorld.instance.AreSpritesFromSameQuestion
                (
                this.gameObject,
                eventData.pointerEnter
                );

                if (isSameQuestion)
                {
                    // GamePlayPanelWorld.instance.PlayAnswerMusic();
                    foreach (Transform child in eventData.pointerEnter.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    // img.color = Color.green;
                    // myImage.fillAmount = 0.2f;
                    canvasGroup.blocksRaycasts = false;
                    canBeDraged = false;
                    this.transform.parent = eventData.pointerEnter.transform.GetChild(0).transform;
                    this.transform.localPosition = new Vector3(0, -150, 0);
                    // LeanTween.scale(img.gameObject, Vector3.one * 0.8f, 0.3f).setLoopPingPong(2);
                    // LeanTween.delayedCall(1f, () =>
                    // {
                    //     if (img != null)
                    //     {
                    //         // img.color = Color.white
                    //     ;
                    //     }
                    // });
                    // DestroyImmediate(this.gameObject);
                }
                else
                {
                    // img.color = Color.red;
                    // Shake left and right repeatedly for 1 sec
                    float originalX = img.transform.localPosition.x;

                    LeanTween.moveLocalX(img.gameObject, originalX + 10f, 0.1f)
                        .setLoopPingPong(3);

                    // AudioManagerFind.instance.PlayButtonSound(5);
                    // LeanTween.delayedCall(1f, () => img.color = Color.white);
                    transform.SetParent(originalParent);
                    rectTransform.anchoredPosition = initialAnchoredPos;
                }


            }
            else
            {
                if (isSnapping == false)
                {
                    transform.SetParent(originalParent);
                    rectTransform.anchoredPosition = initialAnchoredPos;
                }
            }
        }
    }

}