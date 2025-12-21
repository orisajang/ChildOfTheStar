using System.Collections.Generic;
using UnityEngine;

public struct MonsterWaveInfo
{
    public int monsterNumber;
    public int monsterId;
}

public struct MonsterWaveCSVData
{
    //CSV파일에 저장되어있는 속성들
    //public string monsterWaveId;
    //public int monsterNumber;
    //public string monsterId1;
    //public string monsterId2;
    //public string monsterId3;
    //public string monsterId4;
    //public string monsterId5;

    //CSV파일에 저장되어있는 속성들
    public int monsterWaveId;
    //몬스터 Number과 id가 5개가 존재(함께 가져가야하는 구조이므로 sturct 새로 생성)
    public MonsterWaveInfo[] monsterWaveInfo;
    //public int[] monsterNumber;
    //public int[] monsterId;
    

    /// <summary>
    /// for루프 돌리면서 null값이 아닐때까지 정보를 set하기위해서 사용
    /// </summary>
    /// <returns></returns>
    public string[] GetMonsterIdArray()
    {
        string[] monstrtIdArray = new string[]
        {
            //monsterId1,
            //monsterId2,
            //monsterId3,
            //monsterId4,
            //monsterId5,
        };
        return monstrtIdArray;
    }
}
