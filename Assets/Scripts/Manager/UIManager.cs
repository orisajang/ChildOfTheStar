using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // 플레이어
    [SerializeField] private PlayerStatusUI playerStatusUI;
    // 자원
    [SerializeField] private ResourceUI resourceUI;
    // 스테이지
    [SerializeField] private StageUI stageUI;
    // 과충전
    [SerializeField] private OverchargeUI overchargeUI;

    public PlayerStatusUI PlayerStatusUI => playerStatusUI;
    public ResourceUI ResourceUI => resourceUI;
    public StageUI StageUI => stageUI;
    public OverchargeUI OverchargeUI => overchargeUI;
    private void Start()
    {
        ResourceUI.UpdateResource(TileColor.Red, ColorResourceManager.Instance.GetResource(TileColor.Red));
        ResourceUI.UpdateResource(TileColor.Blue, ColorResourceManager.Instance.GetResource(TileColor.Blue));
        ResourceUI.UpdateResource(TileColor.Green, ColorResourceManager.Instance.GetResource(TileColor.Green));
        ResourceUI.UpdateResource(TileColor.White, ColorResourceManager.Instance.GetResource(TileColor.White));
        ResourceUI.UpdateResource(TileColor.Black, ColorResourceManager.Instance.GetResource(TileColor.Black));
        // 과충전이랑 스테이지 테스트용
        OverchargeUI.UpdateOverCharge(30, 100);
        StageUI.testStageText("TEST");
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}