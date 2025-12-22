using UnityEngine;
using UnityEngine.UI;

public class OverchargeUI : MonoBehaviour
{
    // 과충전이 어떻게 되어 있는지 몰라서 임시로 틀만
    [SerializeField] private Image overChargeImage;

    public void UpdateOverCharge(int charge, int maxCharge)
    {
        float ratio = (float)charge / maxCharge;
        overChargeImage.fillAmount = ratio;
    }
}
