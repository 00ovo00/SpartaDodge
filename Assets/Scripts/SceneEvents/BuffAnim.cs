using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI를 제어하려면 UnityEngine.UI 네임스페이스 필요

public class BuffAnim : MonoBehaviour
{
    public Image buffIconImage; // 버프 아이콘 UI 이미지
    private float time = 0;
    private float blinkTime = 0.1f;
    private float xtime = 0;
    private float waitTime = 0.2f;

    // 버프 지속 시간 설정
    private float buffDuration = 7f;

    private void Awake()
    {
        if (buffIconImage == null)
        {
            buffIconImage = GetComponent<Image>(); // Image 컴포넌트를 가져옴
        }
    }

    public void StartBuff(float duration)
    {
        buffDuration = duration;
        time = 0f;
        xtime = 0f;
        waitTime = 0.2f;
        buffIconImage.color = new Color(1, 1, 1, 1); // 처음엔 아이콘이 완전히 보이도록 설정
        buffIconImage.gameObject.SetActive(true); // 버프 아이콘을 활성화
    }

    private void Update()
    {
        if (time < buffDuration - 3f) // 버프가 끝나기 전까지 아이콘을 유지
        {
            buffIconImage.color = new Color(1, 1, 1, 1); // 아이콘이 완전히 보임
        }
        else // 마지막 3초 동안 깜빡이게 처리
        {
            if (xtime < blinkTime)
            {
                buffIconImage.color = new Color(1, 1, 1, 1 - xtime * 10); // 깜빡거리기
            }
            else if (xtime < waitTime + blinkTime)
            {
                // 대기 시간 동안 아무 변화 없음
            }
            else
            {
                buffIconImage.color = new Color(1, 1, 1, (xtime - (waitTime + blinkTime)) * 10); // 다시 켜짐
                if (xtime > waitTime + blinkTime * 2)
                {
                    xtime = 0;
                    waitTime *= 0.8f; // 깜빡이는 속도 증가
                    if (waitTime < 0.02f)
                    {
                        StopBuff(); // 버프가 끝나면 아이콘을 비활성화
                    }
                }
            }
            xtime += Time.deltaTime;
        }
        time += Time.deltaTime;
    }

    public void StopBuff()
    {
        buffIconImage.gameObject.SetActive(false); // 버프 아이콘을 비활성화
        time = 0f;
        xtime = 0f;
        waitTime = 0.2f; // 초기화
    }
}
