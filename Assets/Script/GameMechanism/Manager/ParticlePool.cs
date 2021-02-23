using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : SingletonBase<ParticlePool>
{
    private const string nameNormalPop = "Pop_Normal";
    private const string nameItemPop = "Pop_Item";

    public GameObject PNObj;
    public GameObject PIObj;
    public List<GameObject> Pool_PN;
    public List<GameObject> Pool_PI;

    void Awake()
    {
        Pool_PN = new List<GameObject>();
        Pool_PI = new List<GameObject>();
        InstantiateParticle();
    }
    void InstantiateParticle()
    {
        for (int i = 0; i < 81; i++)
        {
            GameObject newObj = Instantiate(PNObj);
            newObj.SetActive(false);
            newObj.name = nameNormalPop;
            Pool_PN.Add(newObj);
        }
        for (int i = 0; i < 81; i++)
        {
            GameObject newBlock = Instantiate(PIObj);
            newBlock.SetActive(false);
            newBlock.name = nameItemPop;
            Pool_PI.Add(newBlock);
        }
    }
    public GameObject GetParticle(int num,Vector3 pos)
    {
        List<GameObject> Pool;
        if (num == 0)
        {
            Pool = Pool_PN;
        }
        else
        {
            Pool = Pool_PI;
        }
        for (int i = 0; i < Pool.Count; i++)
        {
            if (!Pool[i].activeSelf)
            {
                Pool[i].SetActive(true);
                Pool[i].transform.position = pos;
                StartCoroutine(DisableParticle(Pool[i]));
                return Pool[i];
            }
        }
        return null;
    }
    private IEnumerator DisableParticle(GameObject p)
    {
        yield return YieldInstructionCache.WaitForSeconds(3.0f);
        p.SetActive(false);
    }
    // Update is called once per frame
}
