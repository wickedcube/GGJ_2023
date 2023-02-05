using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
public class EnemyNumberCreator : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> numberPools;

    private Dictionary<int, float> BoundsCache = new Dictionary<int, float>(); 

    List<int> GetIndividualNumbers(int value)
    {
        int temp = value;
        List<int> individualNumbers = new List<int>();
        while (temp > 0)
        {
            var num = temp % 10;
            individualNumbers.Add(num);
            temp = temp / 10;
        }
        return individualNumbers;
    }


    private float GetBoundsValues(int i, IndependentNumber number)
    {
        float boundsSizeX = 0;

        if (i < 10)
        {
            if (BoundsCache.ContainsKey(i))
            {
                boundsSizeX = BoundsCache[i];
            }
            else
            {
                boundsSizeX = number.GetBoundsX();
                BoundsCache[i] = boundsSizeX;
            }
        }
        return boundsSizeX;
    }
    public List<IndependentNumber> CreateNumber(int value, Transform parent)
    {
        List < IndependentNumber > numberHolders = new List < IndependentNumber >();

        var individualNumbers = GetIndividualNumbers(value);
        float cumulativeX = 0;
        foreach (int i in individualNumbers)
        {
            var newNumber = numberPools[i].GetObject() as IndependentNumber;
            var boundsSizeX = GetBoundsValues(i, newNumber);

            cumulativeX += boundsSizeX / 2;
            newNumber.transform.parent = parent;
            newNumber.transform.localPosition = new Vector3(cumulativeX, -0.3f, 0);
            cumulativeX += boundsSizeX / 2;
            newNumber.transform.localRotation = Quaternion.identity;
            numberHolders.Add(newNumber);
        }

        parent.transform.localPosition -= new Vector3(cumulativeX / 2f, 0, 0);
        
        return numberHolders;
    }
    
}
