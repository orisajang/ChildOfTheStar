using UnityEngine;

public class ShopEnterButton : MonoBehaviour
{
    public void OnShopEnter()
    {
        Debug.Log("상점 입장 버튼 클릭됨");
        GameManager.Instance.GoToShopScene();
    }
    private void OnEnable()
    {
        DungeonManager.Instance.SetShopButton(this);
    }
}