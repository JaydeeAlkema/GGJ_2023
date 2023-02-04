using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerUiPanelPrefab;
    [SerializeField] private Transform playerPanelParent;

    private int playerAmount = 0;

    public int PlayerAmount { get => playerAmount; set => playerAmount = value; }
    List<GameObject> uiPanels = new List<GameObject>();

    public void UpdateUI(int players)
    {
        Debug.Log(players);
        if (players == 0) return;
        Debug.Log(players);
        RemoveAllUiPanels();
        for (int i = 0; i < players; i++)
        {
            GameObject uiPanel = Instantiate(playerUiPanelPrefab, playerPanelParent);
            PlayerUIPanel panel = uiPanel.GetComponent<PlayerUIPanel>();
            panel.PlayerText.text = "Player: " + i;
            uiPanels.Add(uiPanel);
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
