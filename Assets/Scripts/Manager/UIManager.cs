using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // 플레이어
    [SerializeField] private PlayerStatusUI playerStatusUI;
    // 자원
    [SerializeField] private ResourceUI resourceUI;
    public PlayerStatusUI PlayerStatusUI => playerStatusUI;
    public ResourceUI ResourceUI => resourceUI;
    private void Start()
    {
        ResourceUI.UpdateResource(TileColor.Red, ColorResourceManager.Instance.GetResource(TileColor.Red));
        ResourceUI.UpdateResource(TileColor.Blue, ColorResourceManager.Instance.GetResource(TileColor.Blue));
        ResourceUI.UpdateResource(TileColor.Green, ColorResourceManager.Instance.GetResource(TileColor.Green));
        ResourceUI.UpdateResource(TileColor.White, ColorResourceManager.Instance.GetResource(TileColor.White));
        ResourceUI.UpdateResource(TileColor.Black, ColorResourceManager.Instance.GetResource(TileColor.Black));
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