using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterStatHandler : MonoBehaviour
{
    private float maxSpeed = 20.0f;
    private float speedBoostDuration = 0f; 
    private bool isSpeedBoostActive = false;

    // 기본 스탯과 버프 스탯들의 능력치를 종합해서 스탯을 계산하는 컴포넌트
    [SerializeField] private CharacterStat baseStats;
    public CharacterStat CurrentStat { get; private set; }
    public List<CharacterStat> statsModifiers = new List<CharacterStat>();

    public BuffAnim speedBuffAnim;

    private void Awake()
    {
        UpdateCharacterStat();
        if (speedBuffAnim != null)
        {
            speedBuffAnim.gameObject.SetActive(false); 
        }
    }
    private void Update()
    {
        if (isSpeedBoostActive)
        {
            speedBoostDuration -= Time.deltaTime;
            if (speedBoostDuration <= 0f)
            {
                ResetSpeed();
            }
        }
    }

    public void AddStatModifier(CharacterStat modifier)
    {
        statsModifiers.Add(modifier);
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        CurrentStat = new CharacterStat
        {
            maxHealth = baseStats.maxHealth,
            speed = baseStats.speed,
            attackSO = baseStats.attackSO
        };

        foreach (CharacterStat modifier in statsModifiers)
        {
            switch (modifier.statsChangeType)
            {
                case StatsChangeType.Add:
                    CurrentStat.maxHealth += modifier.maxHealth;
                    CurrentStat.speed += modifier.speed;
                    break;
                case StatsChangeType.Multiple:
                    CurrentStat.maxHealth *= modifier.maxHealth;
                    CurrentStat.speed *= modifier.speed;
                    break;
                case StatsChangeType.Override:
                    CurrentStat.maxHealth = modifier.maxHealth;
                    CurrentStat.speed = modifier.speed;
                    break;
            }
        }
    }

    public void OverrideSpeed(float newSpeed, float duration)
    {
        CurrentStat.speed = Mathf.Clamp(newSpeed, 0, maxSpeed);
        speedBoostDuration = duration;
        isSpeedBoostActive = true;
        if (speedBuffAnim != null)
        {
            speedBuffAnim.StartBuff(duration); 
        }
    }
    private void ResetSpeed()
    {
        CurrentStat.speed = baseStats.speed;
        isSpeedBoostActive = false;
        if (speedBuffAnim != null)
        {
            speedBuffAnim.StopBuff();
        }
    }
}