using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneAreaBehaviour : MonoBehaviour
{

    #region Fields

    public TextMeshPro areaText;
    public ARPlane arPlane;
    private ARRaycastManager _rayManager;
    public GameObject theBox;
    private bool _objectSpawned;
    
    #endregion


    #region Methods

    void Start()
    {
        _rayManager = FindObjectOfType<ARRaycastManager>();
    }

    private void OnEnable()
    {
        
        arPlane.boundaryChanged += ArPlane_BoundaryChanged;
    }

    private void OnDisable()
    {
        arPlane.boundaryChanged -= ArPlane_BoundaryChanged;
    }

    void ArPlane_BoundaryChanged(ARPlaneBoundaryChangedEventArgs obj)
    {
        areaText.text = CalculatePlaneArea(arPlane).ToString();
    }

    private float CalculatePlaneArea(ARPlane plane)
    {
        return plane.size.x * plane.size.y;
    }

    public void ToggleAreaView()
    {
        if(areaText.enabled)
            areaText.enabled = false;
        else
            areaText.enabled = true;
    }

    private void Update()
    {
        //shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        _rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);
        
        
        if (hits.Count > 0 )
        {
            if (CalculatePlaneArea(arPlane) > 0.5f && _objectSpawned == false)
            {
                Instantiate(theBox, hits[0].pose.position, hits[0].pose.rotation);
                _objectSpawned = true;
            }
            
        }
    }

    #endregion
    
    
}
