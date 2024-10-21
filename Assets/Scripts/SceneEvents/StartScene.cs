using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public GameObject[] backgroundImages; 
    public GameObject playerCharacter; 
    public CanvasGroup canvasGroup;


    public IEnumerator StartGameScene()
    {
        yield return StartCoroutine(SetDeactiveCanvasUI());
        yield return StartCoroutine(ScatterBackgrounds());
        yield return StartCoroutine(MovePlayerCharacter());

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator SetDeactiveCanvasUI()
    {
        float duration = 2f; // 페이드 아웃 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration); 
            canvasGroup.alpha = alpha; 

            yield return null;
        }
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

    }


    private IEnumerator ScatterBackgrounds()
    {
        float duration = 2f; // 흩어지는 시간
        float elapsed = 0f;

        // 각 배경 이미지에 대해 흩어짐 처리
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                foreach (GameObject backgroundImage in backgroundImages)
                {
                    if (backgroundImage != null)
                    {
                        SpriteRenderer backgroundSprite = backgroundImage.GetComponent<SpriteRenderer>();
                        if (backgroundSprite != null)
                        {
                            Color originalColor = backgroundSprite.color;
                            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration); // 점진적 알파값 변화
                            backgroundSprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                        }
                    }
                }

                yield return null;
            }
        }

        // 알파값이 0 이되면  비활성화
        foreach (GameObject backgroundImage in backgroundImages)
        {
            if (backgroundImage != null)
            {
                backgroundImage.SetActive(false);
            }
        }
    }

    private IEnumerator MovePlayerCharacter()
    {
        if (playerCharacter == null) yield break; // 플레이어 캐릭터가 없으면 중단

        float duration = 2f;
        float elapsed = 0f;

        Vector3 startPos = playerCharacter.transform.position;
        Vector3 endPos = startPos + new Vector3(0f, 5f, 0f); // Y축으로 5만큼 이동

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            playerCharacter.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
    }
}
