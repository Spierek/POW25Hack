﻿using UnityEngine;
using Vuforia;

public class UIManager : MonoBehaviour, ITrackableEventHandler
{
    #region Variables
    [SerializeField]
    private TrackableBehaviour m_Trackable;

    [SerializeField]
    private CanvasGroup m_SearchingText;
    [SerializeField]
    private CanvasGroup m_PaintingInfo;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        if (m_Trackable != null)
        {
            m_Trackable.RegisterTrackableEventHandler(this);
        }

        OnTrackingLost();
    }

    private void Update()
    {
    }
    #endregion

    #region Methods
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    private void OnTrackingFound()
    {
        m_SearchingText.alpha = 0;
        m_PaintingInfo.alpha = 1;
    }

    private void OnTrackingLost()
    {
        m_SearchingText.alpha = 1;
        m_PaintingInfo.alpha = 0;
    }
    #endregion
}