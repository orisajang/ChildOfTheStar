using UnityEngine;

public class ShopExitButton : MonoBehaviour
{
    public void OnShopExit()
    {
        DungeonManager.Instance.RePlayCurrentDungeonBGM();
        //Debug.Log("상점 나가기 버튼 클릭됨");
        GameManager.Instance.GoToStageScene();
    }
}