using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool _applicationQuit = false;

    public static T Instance
    {
        get
        {
            if (_applicationQuit)
            {
                return null;
            }
            
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));
                
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T; 
            
            if (transform.parent != null)
            {
                transform.parent = null;
            }

            DontDestroyOnLoad(gameObject);   // 씬 변경 시에도 유지
        }
        
        // 이미 싱글턴 인스턴스가 존재하면, 현재 객체를 삭제
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    protected virtual void OnApplicationQuit()
    {
        _applicationQuit = true;
    }
    

}