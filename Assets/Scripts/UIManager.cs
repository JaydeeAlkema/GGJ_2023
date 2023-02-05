using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject playerUiPanelPrefab;
    [SerializeField] private Transform playerPanelParent;

    private List<PlayerController> playerControllers = new List<PlayerController>();

    List<GameObject> uiPanels = new List<GameObject>();

    public List<PlayerController> PlayerControllers { get => playerControllers; set => playerControllers = value; }

    public void UpdateUI(List<PlayerController> players)
    {
        if (players.Count == 0) return;
        RemoveAllUiPanels();
        for (int i = 0; i < players.Count; i++)
        {
            GameObject uiPanel = Instantiate(playerUiPanelPrefab, playerPanelParent);
            PlayerUIPanel panel = uiPanel.GetComponent<PlayerUIPanel>();
            Debug.Log(uiPanel.GetComponent<PlayerUIPanel>());
            
            panel.PlayerText.text = "Player: " + (i + 1);
            panel.HealthbarFillImage.fillAmount = players[i].Health / 100;
            uiPanels.Add(uiPanel);
        }
    }

    //TODO: use this
    public void UpdateHealthBars()
    {
        for (int i = 0; i < playerControllers.Count; i++)
        {
            uiPanels[i].GetComponent<PlayerUIPanel>().HealthbarFillImage.fillAmount = playerControllers[i].Health / 100;
        }
    }

    public void RemoveAllUiPanels()
    {
        for (int i = 0; i < uiPanels.Count; i++)
        {
            Destroy(uiPanels[i]);
        }
        uiPanels.Clear();
    }
}
