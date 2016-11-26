using UnityEngine;
using System.Collections;

public class PaintingSelectionPanelManager : MonoBehaviour {
    public PaintingManager PaintingManager;
    public GameObject PaintingSelectionPanelPrefab;

	// Use this for initialization
	void Start () {
        PaintingManager.OnPaintingLoaded += PaintingManager_OnPaintingLoaded;
	}

    private void PaintingManager_OnPaintingLoaded(Painting p)
    {
        var newPanel = Instantiate<GameObject>(PaintingSelectionPanelPrefab);
        newPanel.transform.parent = this.transform;

        var newPanelController = newPanel.GetComponent<PaintingSelectionPanel>();
        newPanelController.PaintingManager = PaintingManager;
        newPanelController.Setup(p);
    }


    // Update is called once per frame
    void Update () {
	
	}
}
