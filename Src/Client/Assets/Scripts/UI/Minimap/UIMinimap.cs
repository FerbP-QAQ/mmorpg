using Manager;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour {

	public Collider MinimapBoundingBox;

	public Image minimap;
	public Image arrow;
	public Text mapName;

	private Transform playerTransform;
	// Use this for initialization
	void Start () 
	{
		MiniMapManager.Instance.minimap = this;
        UpdateMap();
	}
	public void UpdateMap()
	{
		mapName.text = User.Instance.CurrentMapData.Name;
		minimap.overrideSprite = MiniMapManager.Instance.LoadCurrentMinimap();

		minimap.SetNativeSize();
		minimap.transform.localPosition = Vector3.zero;
		MinimapBoundingBox = MiniMapManager.Instance.MinimapBoundingBox;
		playerTransform = null;
		
	}

	// Update is called once per frame
	void Update () 
	{
        if (playerTransform==null)
        {
			playerTransform = MiniMapManager.Instance.PlayerTransform;
        }
        if (MinimapBoundingBox == null || playerTransform == null )
        {
			return;
        }
        float realWidth = MinimapBoundingBox.bounds.size.x;
		float realHight = MinimapBoundingBox.bounds.size.z;

		float relaX = playerTransform.position.x - MinimapBoundingBox.bounds.min.x;
		float relaY = playerTransform.position.z-MinimapBoundingBox.bounds.min.z;

		float pivotX = relaX/realWidth;
		float pivotY = relaY/realHight;

		minimap.rectTransform.pivot=new Vector2 (pivotX, pivotY);
		minimap.rectTransform.localPosition=Vector2.zero;

		arrow.transform.localEulerAngles=new Vector3 (0f,0f,-playerTransform.eulerAngles.y);
	}
}
