using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour 
{
	public int npcID;

	SkinnedMeshRenderer renderer;

	Animator anim;
	Color orignColor;

	private bool inInteractive = false;

	NpcDefine npc;
	
	void Start () 
	{
		renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		anim = GetComponent<Animator>();
		orignColor = renderer.sharedMaterial.color;
		npc = NpcManager.Instance.GetNpcDefine(npcID);
		StartCoroutine(Actions());
	}
	IEnumerator Actions()
	{
        while (true)
        {
			if (inInteractive)
			{
				yield return new WaitForSeconds(2f);
			}
			else
			{
				yield return new WaitForSeconds(Random.Range(5f, 10f));
			}
			this.Relax();
        }
    }
	
	void Update () {
		
	}
	void Relax()
	{
		anim.SetTrigger("Relax");
	}

	void Interactive()
	{
		if (!inInteractive) 
		{
			inInteractive = true;
			StartCoroutine(DoInteractive());
		}
	}
	IEnumerator DoInteractive()
	{
		yield return FaceToPlayer();
		if (NpcManager.Instance.Interactive(npc))
		{
			anim.SetTrigger("Talk");
		}
		yield return new WaitForSeconds(3f);
		inInteractive = false;
	}
	IEnumerator FaceToPlayer()
	{
		Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - transform.position).normalized;
		while (Mathf.Abs(Vector3.Angle(gameObject.transform.forward,faceTo))>5)
		{
			gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
			yield return null;
		}
	}

    void OnMouseDown()
    {
        Interactive();
		
    }

    void OnMouseOver()
    {
        Highlight(true);
    }
     void OnMouseEnter()
    {
		Highlight(true );
    }
     void OnMouseExit()
    {
        Highlight(false );
    }

    private void Highlight(bool highlight)
    {
		if (highlight)
		{
            if (renderer.sharedMaterial.color != Color.black)
            {
                renderer.sharedMaterial.color = Color.black;
            }
        }
		else
		{
			if (renderer.sharedMaterial.color!=orignColor)
			{
				renderer.sharedMaterial.color = orignColor;
			}
		}
    }

}
