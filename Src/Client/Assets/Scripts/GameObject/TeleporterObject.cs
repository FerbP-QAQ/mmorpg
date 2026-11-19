using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour 
{
	public int ID;
	Mesh mesh=null;
	void Start () 
	{
		mesh = GetComponent<MeshFilter>().sharedMesh;
	}
	
	void Update () 
	{
		
	}
#if UNITY_EDITOR
     void OnDrawGizmos()
    {
		Gizmos.color = Color.green;
		if (mesh!=null)
		{
			Gizmos.DrawWireMesh(mesh, transform.position + Vector3.up * transform.localPosition.y * .5f, transform.rotation, transform.localScale);
		}
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(0,transform.position,transform.rotation,1f,EventType.Repaint);
    }
#endif
     void OnTriggerEnter(Collider other)
     {
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
		if (playerInputController != null && playerInputController.isActiveAndEnabled)
		{
			TeleporterDefine td = DataManager.Instance.Teleporters[ID];
			if (td==null)
			{
				Debug.LogErrorFormat("TeleporterObject:Character:[{0}] Enter Teleporter:[{1}],But TeleporterDefine not existed", playerInputController.character.Info.Name, this.ID);
				return;
			}
			Debug.LogFormat("TeleporterObject:Character:[{0}] Enter Teleporter:[{1}:{2}]", playerInputController.character.Info.Name, td.ID, td.Name);
			if (td.LinkTo>0)
			{
				if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
				{
					MapService.Instance.SendMapTeleporter(this.ID);
				}
				else
				{
					Debug.LogErrorFormat("Teleporter ID:{0} LinkTo:{1} error !",td.ID,td.LinkTo);
				}
			}
		}
     }

}
