using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public RectTransform maskImage;
    public float maskSpeed = 5f;

    public Button btn;
    public CanvasGroup canvasGroup;

    private Vector2 targetSize = Vector2.zero;
    private Vector2 initialSize;

    private void Start()
    {
        initialSize = maskImage.sizeDelta;
    }

    private void Awake()
    {
        // 하나만 생성되도록 관리
        if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    public void OnStartButtonClick()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; 
            canvasGroup.interactable = false; 
            canvasGroup.blocksRaycasts = false; 
        }
        StartCoroutine(maskLoadScene("MainScene")); 
    }
    private IEnumerator maskLoadScene(string sceneName)
    {
        while (maskImage.sizeDelta != targetSize)
        {
            maskImage.sizeDelta = Vector2.Lerp(maskImage.sizeDelta, targetSize, Time.deltaTime * maskSpeed);
            yield return null;
        }
        //SceneManager.LoadScene("SampleScene");
        //SceneManager.LoadScene(sceneName);
        GameManager.Instance.StartGame();
    }
}