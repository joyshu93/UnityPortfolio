using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    FlappyManager FM;

    [Header("[ Prefabs ]")]
    public GameObject preColumn; //장애물 프리팹

    GameObject[] preColumns; //미리 구성해 놓은 장애물집합
    int colPoolSize = 5; //개수
    int currentColIndex = 0; //재배치 시킬 개수
    float colSpawnRate = 3f; //재배치 시간 간격
    float spawnPositionX = 10f; //재배치 시킬 x좌표
    float colPositionMaxY = 3f; //재배치 시킬 y좌표의 최대값
    float colPositionMinY = -0.5f; //재배치 시킬 y좌표의 최소값

    private void Awake()
    {
        FM = GameObject.Find("FlappyManager").GetComponent<FlappyManager>();
    }

    public void InitColumnCreate()
    {
        preColumns = new GameObject[colPoolSize];
        for(int i = 0; i < preColumns.Length; i++)
        {
            preColumns[i] = Instantiate(preColumn, new Vector2(-15, -25), Quaternion.identity);
        }
        InvokeRepeating("Spawn", 0f, colSpawnRate);

    }

    void Spawn()
    {
        if (FM.flappyGameOver) return;
        float positionY = Random.Range(colPositionMinY, colPositionMaxY);
        preColumns[currentColIndex].transform.position = new Vector2(spawnPositionX, positionY);
        currentColIndex = (currentColIndex + 1) % colPoolSize;
    }
}
