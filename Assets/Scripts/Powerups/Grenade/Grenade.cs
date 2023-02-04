using System.Collections;
using Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace Powerups.Grenade
{
   public class Grenade : MonoBehaviour
   {
      [SerializeField] private Color aoeColor;
      [SerializeField] private LayerMask detectionLayer;
      [SerializeField] private GameObject explosionRef;
      [SerializeField] private float defaultFuseTime = 2;
      [SerializeField] private float aoe = 10;
      
      [Range(0f,1f)]
      [SerializeField] private float explosionScale = 1;
      
      private bool timerActive = false;

      private void OnTriggerEnter(Collider other)
      {
         var enemy = other.GetComponent<INumberEnemy>();
         if(enemy != null && !timerActive)
            Detonate();
      }

      private void OnCollisionEnter(Collision collision)
      {
         if (!collision.gameObject.layer.Equals(detectionLayer) && !timerActive)
            StartCoroutine(StartTimer());
      }

      private IEnumerator StartTimer()
      {
         timerActive = true;
         float timeStep = 0;
         while (timeStep <= 1)
         {
            timeStep += Time.deltaTime / (defaultFuseTime * Time.timeScale);
            yield return new WaitForEndOfFrame();   
         }
         
         Detonate();
      }

      private void Detonate()
      {
         var go = Instantiate(explosionRef, transform.position, quaternion.identity);
         go.transform.localScale *= explosionScale;
         DamageEnemy();
         Destroy(this.gameObject);
      }

      private void DamageEnemy()
      {
         var colliders = Physics.OverlapSphere(transform.position, aoe, detectionLayer);
         foreach (var collider in colliders)
         {
            var enemy = collider.GetComponentInChildren<INumberEnemy>();
                if (enemy.CanTakeDamage(this))
                {
                    enemy.TakeDamage(this);
                }
         }
      }

      private void OnDrawGizmos()
      {
         Gizmos.color = aoeColor;
         Gizmos.DrawSphere(transform.position, aoe);
      }
   }
}