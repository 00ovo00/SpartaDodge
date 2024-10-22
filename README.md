# 프로젝트 소개

플레이어 공격 기능을 추가한 닷지 게임입니다. 키보드로 움직임을 조작하고 마우스 왼클릭으로 공격하여 적을 처치하고 적의 공격을 피하며 생존해야 합니다.

### InputSystem

InputSystem Package 활용, 키보드와 마우스로 조작

- 이동 : 키보드(WASD)
- 조준: 마우스 위치
- 공격 : 마우스 왼클릭

### Contents

- 시작 화면에서 게임 시작 버튼 클릭 시 플레이 화면으로 전환
- 적 처치하면 상단바에 점수 업데이트
- 플레이어 체력이 0이면 게임 오버, 재시작 버튼 및 점수판 UI 활성화
- 3가지 종류의 적(Bat, Crab, Golem)이 랜덤한 위치에서 처치한 적의 수에 따라  등장하며 난이도 상승
- 적을 처치하면 일정 확률로 아이템 등장
- 아이템 종류에 따라 다른 버프 발동(체력 회복, 이속 증가, 무적 모드)

# 플레이 화면

![image](https://github.com/user-attachments/assets/1b20fbaa-d160-4511-980c-eee6d5a59f9c)
TitleScene

![image](https://github.com/user-attachments/assets/9a81956f-0a66-4ba2-b542-4089aa839196)
MainScene

![image](https://github.com/user-attachments/assets/c58ed6dd-4a96-4f45-8109-bdff5e8882c8)
GameOver

# 와이어프레임

![image](https://github.com/user-attachments/assets/2c815c8d-4511-4012-93d2-fe41307dda46)

# 주요 기능

1. **체력 관리 시스템**
    - 체력 변동 시 UI에 체력 변화 반영
    - 변동값이 양수면 힐 이벤트, 음수면 데미지 이벤트 호출
    - 플레이어 체력이 0이면 게임오버 이벤트, 적 체력이 0이면 사망 이벤트 호출
2. **적 행동 패턴**
    - 플레이어가 추적 범위 내에 있고 공격 범위 내에 있으면 공격
    - 플레이어가 추적 범위 내에 있고 공격 범위 외에 있으면 추격
    - 플레이어가 추적 범위 외에 있으면 idle
3. **오브젝트 풀링**
    - 투사체 및 적과 같은 재사용 가능한 오브젝트를 관리
    - 태그, 프리팹, 크기를 사용해 풀을 정의하여 자원 최적화
    - 게임 내 필요 자원을 일정 주기로 생성하도록 관리
    - 적 캐릭터는 특정 포인트에서 스폰, 킬 카운트에 따라 더 강한 적들이 등장
4. **아이템**
    - 적 처치 시 해당 위치에 일정 확률로 아이템 생성
    - 아이템 정보를 Scriptable Object로 초기화
    - 이이템 종류에 따라 다른 효과 적용
5. **UI**
    - 플레이어의 체력, 점수, 게임 오버 정보를 화면에 표시
    - 화면 전환 시 UI 요소의 페이드 처리와 게임 오버 화면 구현

# 트러블슈팅
### 1. 오브젝트 풀 생성 문제

- **기존 접근**     
오브젝트풀 클래스에서 CreatePool()메서드를 통해 특정 태그를 가진 오브젝트 풀을 생성하고 SpawnFromPool() 메서드로 풀에서 오브젝트 소환하도록 관리
- **문제**
    
    동적으로 생성된 오브젝트와 비동적 오브젝트를 구분없이 풀에서 관리하여 랜덤소환 로직에서 의도하지 않은 오브젝트 소환되는 문제 발생
    
- **문제 해결**
    
    기존 오브젝트풀의 구조를 변경한뒤 MonsterObjectPool로 상속해 추가로직을 작성하여 유연하게 로직이 적용될 수있게 변경
    
    ```csharp
    /* 문제 발생 코드*/
    public class ObjectPool : MonoBehaviour
    {
        // 특정 오브젝트 풀을 생성하는 메서드
        public void CreatePool(string tag, GameObject prefab, int size) 
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); // 풀을 위한 새 큐 생성
            for (int i = 0; i < size; i++)
            {
                // 프리팹을 인스턴스화하고 비활성화한 후 풀 큐에 추가
                GameObject obj = Instantiate(prefab, transform); 
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(tag, objectPool);
        }
        // 특정 스폰 포인트에서 오브젝트를 생성하는 오버로딩 메서드
        public GameObject SpawnFromPool(string tag, GameObject spawnPoint)
        {
            if (!PoolDictionary.ContainsKey(tag))
                return null;
    
            GameObject obj = PoolDictionary[tag].Dequeue();
            PoolDictionary[tag].Enqueue(obj);
            obj.transform.position = spawnPoint.transform.position; 
            obj.SetActive(true);
            return obj;
        }
    }
    ```
    
    ```csharp
    /* 해결된 코드*/
    public class MonsterObjectPool : ObjectPool 
    {
       // 풀 이름 리스트 SpawnManager 에서 활용
        public List<string> PollNameList { get; private set; } 
    
        protected override void Awake()
        {
            base.Awake();
        }
        // 오버라이드된 풀 생성 메서드
        public override void CreatePool(string tag, GameObject prefab, int size) 
        {
            base.CreatePool(tag, prefab, size);
            UpdatePoolNameList(tag);
        }
        // 풀 이름 리스트를 업데이트하는 메서드
        private void UpdatePoolNameList(string tag) 
        {
            if (PollNameList == null)
            {
                PollNameList = new List<string>();
            }
            PollNameList.Add(tag);
        }
    }
    ```
    
### 2. 오디오 작업

- **기존 접근**     
적 사망 시 효과음을 적이 사망할 때 호출하는 함수인 DestroyOnDeath에서 사운드 이벤트 호출하도록 설정
- **문제**
    
    Scene 상에 적이 존재하지 않을 때 NullReferenceException 발생
    
- **문제 해결**
    
    DataManager의 IncrementKill(킬카운트 증가 메소드)가 실행될 때 적이 죽는 사운드 이벤트(EnemyDeath) 호출하도록 변경하여 정상 실행
    
    ```csharp
    /* 문제 발생 코드*/
    public class DestroyOnDeath : MonoBehaviour
    {
      private void Start()
      {
        healthSystem.OnDeath += OnDeath;
      }
      void OnDeath()
      {
        DataManager.Instance.IncrementKillCount();
      }
    }
    public class HealthSystem : MonoBehaviour
    {
      IEnumerator DeathSequence()
      {
        OnDeath?.Invoke();
      }
    }
    public class SoundManager : MonoBehaviour
    {
      public void ReSetBinding(Scene scene, LoadSceneMode mode)
      {
        healthSystem.OnDeath += PlayEnemyDeathSFX;
      }
    }
    ```
    
    ```csharp
    /* 해결된 코드*/
    public class DataManager : MonoBehaviour
    {
      public void IncrementKillCount()
      {
          killCount++;
          OnEnemyDeath?.Invoke();
      }
    }
    public class SoundManager : MonoBehaviour
    {
      public void ReSetBinding(Scene scene, LoadSceneMode mode)
      {
        DataManager.OnEnemyDeath += PlayEnemyDeathSFX;
      }
    }
    ```
    
### 3. Git 충돌 문제

- **기존 접근**     
기존에 깃허브 협업경험이 있어서 서로 다른 스크립트 파일을 작업
- **문제**
    
    다른 스크립트 파일을 작업하였으나 유니티 내 프로젝트에서 같은 씬 파일이 자주 수정되었고 병합할 때 충돌 자주 발생
    
- **문제 해결**
    
    씬 내부의 오브젝트를 프리팹화하고 테스트 할 때는 별도의 테스트 씬을 만들어 작업, 오브젝트 프리팹화 하여 별도의 파일로 저장했으므로 충돌 발생하지 않고 원할하게 협업 진행
