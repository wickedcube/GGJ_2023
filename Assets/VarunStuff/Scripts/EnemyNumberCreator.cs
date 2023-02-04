using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
public class EnemyNumberCreator : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPool> numberPools;

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

        individualNumbers.Reverse();
        return individualNumbers;
    }
    public List<IndependentNumber> CreateNumber(int value, Transform parent)
    {
        List < IndependentNumber > numberHolders = new List < IndependentNumber >();

        var individualNumbers = GetIndividualNumbers(value);
        float cumulativeX = 0;
        foreach (int i in individualNumbers)
        {
            var newNumber = numberPools[i].GetObject() as IndependentNumber;
            newNumber.transform.parent = parent;
            newNumber.transform.localPosition = new Vector3(cumulativeX + newNumber.transform.localScale.x / 2, 0, 0);
            cumulativeX = newNumber.transform.localPosition.x + newNumber.transform.localScale.x / 2;
            newNumber.transform.localRotation = Quaternion.identity;
            numberHolders.Add(newNumber);
        }

        parent.transform.localPosition -= new Vector3(cumulativeX / 2f, 0, 0);
        
        return numberHolders;
    }
    
}
