using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

public class IndependentNumber : Poolable
{
    private MeshRenderer meshRenderer;

    public BoxCollider boxCollider { set; get; }

    public float HeightOffset => this.transform.localScale.y;
    public Bounds GetMeshRendererBounds
    {
        get
        {
            var bounds = new Bounds();
            var boxColliderBounds = this.boxCollider.bounds;
            bounds.center = boxCollider.center + this.transform.localPosition;
            bounds.size = boxColliderBounds.size * this.transform.localScale.x;
            return bounds;

        }
    }
    public float GetBoundsX() => meshRenderer.bounds.extents.x * 0.45f; // jugaad number..

    protected override void OnReturnedToPool()
    {
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        if(meshRenderer == default)
        {
            Debug.LogError("mesh filter isn't getting recongized");
        }
        boxCollider = this.GetComponentInChildren<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
