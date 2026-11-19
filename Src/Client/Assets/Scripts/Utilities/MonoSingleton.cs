using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();
            }
            // 场景里没挂这个组件时自动创建，避免 Instance 为 null 导致 NullReferenceException
            // （典型场景：直接从 Loading/注册界面进游戏，场景里没有 NetClient 物体）
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(T).Name);
                instance = go.AddComponent<T>();
                Debug.LogWarning("[MonoSingleton] 场景中不存在 " + typeof(T).Name + " 物体，已自动创建一个。建议在场景里手动挂上以免配置丢失。");
            }
            return instance;
        }

    }

    void Awake()
    {   
        Debug.LogWarningFormat("{0}[{1}] Awake", typeof(T), GetInstanceID());
        if (global) 
        {
            if (instance!=null&&instance!=gameObject.GetComponent<T>())
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject); 
            instance = gameObject.GetComponent<T>();
        }
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}