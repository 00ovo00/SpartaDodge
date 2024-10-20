using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        // �ϳ��� �����ǵ��� ����
        if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    public void OnStartButtonClick()
    {
        //string sceneName = "TestSceneHSH";
        //SceneManager.LoadScene("SampleScene");
        //SceneManager.LoadScene(sceneName);
        GameManager.Instance.StartGame();
    }
}
