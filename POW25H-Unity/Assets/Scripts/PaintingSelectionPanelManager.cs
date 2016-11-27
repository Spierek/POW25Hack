using UnityEngine;
using System.Collections;
using MaterialUI;

public class PaintingSelectionPanelManager : MonoBehaviour {
    public PaintingManager PaintingManager;
    public GameObject PaintingSelectionPanelPrefab;
    public ScreenView ScreenViewManager;
    public GameObject PaintingSelectionPanelContainer;

	// Use this for initialization
	void Awake () {
        Debug.Log("PaintingSelectionPanelManager awake");
        PaintingManager.OnPaintingLoaded += PaintingManager_OnPaintingLoaded;
	}

    private void PaintingManager_OnPaintingLoaded(Painting p)
    {
        Debug.Log("Setting up a painting selection panel.");
        var newPanel = Instantiate<GameObject>(PaintingSelectionPanelPrefab);
        newPanel.transform.SetParent(PaintingSelectionPanelContainer.transform, false);
        //newPanel.transform.parent = this.transform;
        //newPanel.transform.localScale = new Vector3(1f, 1f, 1f);

        var newPanelController = newPanel.GetComponent<PaintingSelectionPanel>();
        newPanelController.PaintingManager = PaintingManager;
        newPanelController.Setup(p, ScreenViewManager);

        Debug.Log("Set up a painting selection panel.");
    }


    // Update is called once per frame
    void Update () {
	
	}
}
