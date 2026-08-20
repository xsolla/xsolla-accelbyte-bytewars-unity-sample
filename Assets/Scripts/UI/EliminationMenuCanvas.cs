using UnityEngine;
using UnityEngine.UI;

public class EliminationMenuCanvas : MenuCanvas
{
    [SerializeField] private Button twoPlayersBtn;
    [SerializeField] private Button threePlayersBtn;
    [SerializeField] private Button fourPlayersBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private GameModeSO localMulti2player;
    [SerializeField] private GameModeSO localMulti3player;
    [SerializeField] private GameModeSO localMulti4player;
    void Start()
    {
        twoPlayersBtn.onClick.AddListener(Click2Players);
        threePlayersBtn.onClick.AddListener(Click3Players);
        fourPlayersBtn.onClick.AddListener(Click4Players);
        backBtn.onClick.AddListener(MenuManager.Instance.OnBackPressed);
    }

    private void Click2Players()
    {
        GameManager.Instance.StartGame(localMulti2player);
    }

    private void Click3Players()
    {
        GameManager.Instance.StartGame(localMulti3player);
    }

    private void Click4Players()
    {
        GameManager.Instance.StartGame(localMulti4player);
    }

    public override GameObject GetFirstButton()
    {
        return twoPlayersBtn.gameObject;
    }

    public override AssetEnum GetAssetEnum()
    {
        return AssetEnum.EliminationMenuCanvas;
    }
}
