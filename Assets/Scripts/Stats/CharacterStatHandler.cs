using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterStatHandler : MonoBehaviour
{
    private float maxSpeed = 20.0f;

    // �⺻ ���Ȱ� ���� ���ȵ��� �ɷ�ġ�� �����ؼ� ������ ����ϴ� ������Ʈ
    [SerializeField] private CharacterStat baseStats;
    public CharacterStat CurrentStat { get; private set; }
    public List<CharacterStat> statsModifiers = new List<CharacterStat>();

    private void Awake()
    {
        UpdateCharacterStat();
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
            CurrentStat.speed = (CurrentStat.speed > maxSpeed) ? maxSpeed : CurrentStat.speed;
        }
    }

    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(CountdownInvincibility(duration));
    }

    private IEnumerator CountdownInvincibility(float duration)
    {
        Debug.Log("���� ����");
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        healthSystem.EnableInvincibility();

        yield return new WaitForSeconds(duration);

        healthSystem.DisableInvincibility(); 
        Debug.Log("���� ��");
    }

    public void OverrideSpeed(float newSpeed)
    {
        CurrentStat.speed = Mathf.Clamp(newSpeed, 0, maxSpeed); 
    }
}