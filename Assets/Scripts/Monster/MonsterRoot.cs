using UnityEngine;

public class MonsterRoot : MonoBehaviour
{
    //몬스터의 부모에 스크립트를 넣어서 스케일을 관리하기 위해서 추가
    private Monster _monster;
    public Monster Monster => _monster;

    private void Awake()
    {
        _monster = GetComponentInChildren<Monster>();
    }
    public void SetScale(float value)
    {
        //스케일 조정  (값에 따라)
        transform.localScale = new Vector3(value, value, value);
    }
}
