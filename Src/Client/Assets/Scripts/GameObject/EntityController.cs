using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour ,IEntityNotify 
{

	public Animator anim;
	public Rigidbody rb;
	private AnimatorStateInfo currentBaseState;

	public Entity entity ;

	public UnityEngine.Vector3 position;
	public UnityEngine.Vector3 direction;
	Quaternion rotation;

	public UnityEngine.Vector3 lastPosition;
	Quaternion lastRotation;

	public float speed;
	public float animSpeed = 1.5f;
	public float jumpPower = 3.0f;

	public bool isPlayer = false;

	// Use this for initialization
	void Start () 
	{
		if (entity!=null)
		{
			EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
			this.UpdataTransform();
		}
		// 所有角色（本地玩家 + 远程玩家）的位置都由网络逻辑坐标通过 rb.MovePosition 驱动，
		// 不应受物理重力影响。之前只对本地玩家(this.isPlayer)关闭了重力，远程玩家保留了
		// Rigidbody 默认的 useGravity=true，于是在“别的客户端”的视角里，这个被观察的角色
		// 会一直受重力下坠（表现为下沉/掉进地面）。
		rb.useGravity = false;
	}

    void UpdataTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
		this.direction= GameObjectTool.LogicToWorld(entity.direction);

		this.rb.MovePosition(this.position);
		this.transform.forward=this.direction;
		this.lastPosition = this.position;
		this.lastRotation = this.rotation;
    }

	void OnDestroy()
	{
		if (entity != null)
		{
			Debug.LogFormat("{0}OnDestroy : ID:{1} POS:{2} DIR:{3} SPD:{4}", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
		}

		if (UIWorldElementManager.Instance != null)
		{
			UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);

		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (this.entity == null)
		{
			return;
		}

		if (!this.isPlayer)
		{
			this.UpdataTransform() ;
		}
	}
	    public void OnEntityRemove()
    {
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(transform);
        }
        Destroy(gameObject);
    }


    public void OnEntityEvent(EntityEvent entityEvent)
	{
		switch (entityEvent)
		{
			case EntityEvent.Idle:
				anim.SetBool("Move", false);
				anim.SetTrigger("Idle");
				break;
			case EntityEvent.MoveFwd:
				anim.SetBool("Move", true);
				break;
			case EntityEvent.MoveBack:
				anim.SetBool("Move", true);
				break;
			case EntityEvent.Jump:
				anim.SetTrigger("Jump");
				break;
		}
	}

    public void OnEntityChanged(Entity entity)
    {
		Debug.LogFormat("OnEntityChanged :ID:{0} POS:{1} DIR:{2} SPD:{3}", entity.entityId, entity.position, entity.direction, entity.speed);
    }
}
