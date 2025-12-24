using UnityEngine;

[CreateAssetMenu(fileName = "ComparePlayerHp", menuName = "Scriptable Objects/SkillCondition/ComparePlayerHp")]
public class ComparePlayerHp : SkillConditionBase
{
        public enum CompareType
        {
            Over,    
            Under,   
            AtLeast, 
            AtMost,  
            Exactly  
        }

        [Header("체력 퍼센트 조건")]
        [Tooltip("비교할 퍼센트 값 (0 ~ 100)")]
        [Range(0, 100)] 
        [SerializeField] private int _targetPercent = 50;

        [Tooltip("Over 초과, Under 미만, AtLeast 이상, AtMost 이하, Exactly 정확히,")]
        [SerializeField] private CompareType _checkType = CompareType.Under;

        public override bool CanExecute(Tile[,] board, Tile casterTile)
        {
            if (casterTile == null) return false;

       
            int curHP = PlayerManager.Instance._player.CharacterHpCurrent * 100;
            int conditionHp = PlayerManager.Instance._player.CharacterHpMax * _targetPercent;

            switch (_checkType)
            {
                case CompareType.Over:    
                    return curHP > conditionHp;

                case CompareType.Under:   
                    return curHP < conditionHp;

                case CompareType.AtLeast: 
                    return curHP >= conditionHp;

                case CompareType.AtMost:  
                    return curHP <= conditionHp;

                case CompareType.Exactly: 
                    return curHP == conditionHp;

                default:
                    return false;
            }
        }
   
}
