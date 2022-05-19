using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    private void Awake()
    {
        ObjectPool.instance = this;
        list = new Dictionary<string, List<GameObject>>();
        inActiveList = new Dictionary<string, List<GameObject>>();
    }

    GameObject bulletFactory = null;

    //Dictionary로 이름이 있는 프리팹을 담아보자.
    public Dictionary<string, List<GameObject>> list;
    public Dictionary<string, List<GameObject>> inActiveList;

    public int maxCount = 1;

    internal void CreateInstance(string prefabName, Transform parent, int amount)
    {
        string key = prefabName;
        maxCount = amount;
        //Resources 폴더에서 파일을 불러온다.
        bulletFactory = (GameObject)Resources.Load("Prefabs/" + prefabName);
        //안 보이는 목록을 만들어서 관리하고 싶다.

        for (int i = 0; i < maxCount; i++)
        {
            //미리 maxCount만큼 만들어 놓고 비활성화 하고 싶다.
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.parent = parent;
            bullet.name = key;
            bullet.SetActive(false);

            //만약에 Dictionary에 key가 존재하지 않는다면
            if (false == list.ContainsKey(key))
            {
                //key와 value를 추가하고 싶다.
                list.Add(key, new List<GameObject>());
                inActiveList.Add(key, new List<GameObject>());
            }
            list[key].Add(bullet);
            inActiveList[key].Add(bullet);
        }
    }
    public void AddInactiveList(GameObject obj)
    {
        string key = obj.name;
        obj.SetActive(false);
        if (ObjectPool.instance.list.ContainsKey(key))
        {
            if (false == inActiveList[key].Contains(obj))
            {
                inActiveList[key].Add(obj);
            }
        }

    }

    public GameObject GetInactiveBulletNew(String key)
    {
        //해당 key가 존재하지 않는다면
        if (false == inActiveList.ContainsKey(key))
        {
            return null;
        }

        //만약 비활성 목록이 0개 초과라면
        if (inActiveList[key].Count > 0)
        {
            //제일 앞에 있는 총알을 활성화하고
            inActiveList[key][0].SetActive(true);
            //비활성목록에서 제거하고
            GameObject temp = inActiveList[key][0];
            inActiveList[key].RemoveAt(0);
            //반환하고 싶다.
            return temp;
        }
        //그렇지 않다면

        return null;
    }

    public static bool IsObjectPoolObject(GameObject obj)
    {
        string key = obj.name;
        if (ObjectPool.instance.list.ContainsKey(key))
        {
            return ObjectPool.instance.list[key].Contains(obj);
        }
        return false;
    }
}
