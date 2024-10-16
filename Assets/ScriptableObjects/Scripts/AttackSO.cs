using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAttackSO", menuName = "TopDownController/Attacks/Default", order = 0)]
public class AttackSO : ScriptableObject
{
    // 공격에 대한 기준 데이터를 유니티 에디터 상에서 편하게 관리
    // SO로 들고 있으면 모두가 이 SO를 바라보게 되어 중복 데이터 없애고 일관성 유지
    // 헤더로 분류하여 가독성 향상
    [Header("Attack Info")]
    public float size;
    public float delay;
    public float power;
    public float speed;
    public LayerMask target;

    [Header("Knock Back Info")]
    public bool isOnKnockback;
    public float knockbackPower;
    public float knockbackTime;
}