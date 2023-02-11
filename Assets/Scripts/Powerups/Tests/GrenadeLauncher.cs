using UnityEngine;
using Unity.Mathematics;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float forceMagnitude = 1f;

    private PlayerStats stats;

    private System.Func<bool> CanThrowGrenade;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        CanThrowGrenade = () =>
        {
            if (stats.ComboMeterLocked)
                return false;
            
            if(stats.ComboMeterValue <= 0)
                return false;

            return stats.ComboMeterValue >= PlayerStats.COMBO_STEP;
        };
    }

    void Update()
    {
        PlayerHealthUI.Instance.ShowGrenadeKey(CanThrowGrenade());
        
        if (KeyMappings.GetGrenadeKeyDown() && CanThrowGrenade())
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade() 
    {
		Debug.Log("EL GRENADO!!");
		stats.ComboMeterLocked=true;
		var go = Instantiate(prefab, spawnPoint.position, quaternion.identity);
		var rb = go.GetComponentInChildren<Rigidbody>();
		rb.AddForce(transform.forward*forceMagnitude);
		stats.ConsumeCombo(PlayerStats.COMBO_STEP);
		stats.ComboMeterLocked=false;
	}
}
