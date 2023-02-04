using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// this class is a  
public class ObjectPool : MonoBehaviour
{

    [SerializeField]
    private Poolable Prefab;

    [SerializeField]
    private uint InitializeNumber;

    [SerializeField]
    private uint ExtendNumber;

    private List<Poolable> poolables = new List<Poolable>();

    private void ExtendPool(uint number)
    {
        for(int i = 0; i < number; ++i)
        {
            var newPoolable = Instantiate(Prefab, this.transform);
            newPoolable.gameObject.SetActive(false);
            newPoolable.SetPoolable(this);
            poolables.Add(newPoolable);
        }
    }

    private void Awake()
    {
        ExtendPool(InitializeNumber);
    }

    public void ReturnPoolable(Poolable p)
    {
        if (p == default)
        {
            Debug.LogError("you're doing some shit!!, poolable is null or destroyed bitch!");
            return;
        }

        if (poolables.Contains(p))
        {
            p.gameObject.SetActive(false);
            p.gameObject.transform.parent = this.transform;
        }

    }

    public Poolable GetObject()
    {
        var badPoolables = poolables.Where(x => x == default);
        if (badPoolables.Any())
        {
            Debug.LogError(" Please don't call destroy on poolable !!");
        }
        foreach (var poolable in badPoolables) poolables.Remove(poolable);

        if (poolables.All(x => x.isActiveAndEnabled)) ExtendPool(ExtendNumber);

        var newPoolable = poolables.Where(x => !x.gameObject.activeSelf).FirstOrDefault();
        newPoolable.gameObject.SetActive(true);
        return newPoolable;
    }
}
