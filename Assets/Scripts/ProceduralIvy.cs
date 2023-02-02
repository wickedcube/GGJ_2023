using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProceduralIvy : MonoBehaviour
{

    public Camera cam;
    [Space]
    public float recycleInterval = 30;
    [Space]
    public int branches = 3;
    public int maxPointsForBranch = 20;
    public float segmentLength = .002f;
    public float branchRadius = 0.02f;
    [Space]
    public Material branchMaterial;
    public Material leafMaterial;
    public Material flowerMaterial;
    [Space]
    public Blossom leafPrefab;
    public Blossom flowerPrefab;
    [Space]
    public bool wantBlossoms;
    private List<IvyNode> previousNodes = new List<IvyNode>();
    public Transform startPoint;
    int ivyCount = 0;
    private Vector3 startPos;
    private Vector3 startNormal;
    public Transform effect;
    public Animator animator;

    void Start()
    {
        startPos = startPoint.position;
        startNormal = startPoint.up;
    }

    void Update()
    {

        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     // call this method when you are ready to group your meshes
        //     combineAndClear();
        // }

        // if (Input.GetMouseButtonDown(0)) {
        //     Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit, 100)) {
        //         createIvy(hit);
        //     }
        // }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            createBranchedIvy(effect.transform.position, effect.transform.up);
            effect.gameObject.SetActive(true);
            // combineAndClear();
            StartCoroutine(PlayVFX());

        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            createIvy(startPos, startNormal, (Input.GetAxis("Vertical") * Vector3.forward + Input.GetAxis("Horizontal") * Vector3.right).normalized);
            // combineAndClear();
        }
    }

    IEnumerator PlayVFX()
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("death");
    }

    Vector3 findTangentFromArbitraryNormal(Vector3 normal)
    {
        Vector3 t1 = Vector3.Cross(normal, Vector3.forward);
        Vector3 t2 = Vector3.Cross(normal, Vector3.up);
        if (t1.magnitude > t2.magnitude)
        {
            return t1;
        }
        return t2;
    }

    // public void createIvy(RaycastHit hit) {
    //     Vector3 tangent = findTangentFromArbitraryNormal(hit.normal);
    //     GameObject ivy = new GameObject("Ivy " + ivyCount);
    //     ivy.transform.SetParent(transform);
    //     for (int i = 0; i < branches; i++) {
    //         Vector3 dir = Quaternion.AngleAxis(360 / branches * i + Random.Range(0, 360 / branches), hit.normal) * tangent;
    //         List<IvyNode> nodes = createBranch(maxPointsForBranch, hit.point, hit.normal, dir);
    //         previousNodes.AddRange(nodes);
    //         GameObject branch = new GameObject("Branch " + i);
    //         Branch b = branch.AddComponent<Branch>();
    //         if (!wantBlossoms) {
    //             b.init(previousNodes, branchRadius, branchMaterial);
    //         } else {
    //             b.init(nodes, branchRadius, branchMaterial, leafMaterial, leafPrefab, flowerMaterial, flowerPrefab, i == 0);
    //         }
    //         branch.transform.SetParent(ivy.transform);
    //     }

    //     ivyCount++;
    // }

    public void createIvy(Vector3 point, Vector3 normal, Vector3 inputDir)
    {
        Vector3 tangent = findTangentFromArbitraryNormal(normal);
        GameObject ivy = new GameObject("Ivy " + ivyCount);
        ivy.transform.SetParent(transform);
        Vector3 dir = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.right, inputDir, Vector3.up), normal) * tangent;
        List<IvyNode> nodes = createBranch(maxPointsForBranch, point, normal, dir);
        previousNodes.AddRange(nodes);
        GameObject branch = new GameObject("Branch ");
        Branch b = branch.AddComponent<Branch>();
        if (!wantBlossoms)
        {
            b.init(previousNodes, branchRadius, branchMaterial);
        }
        else
        {
            b.init(nodes, branchRadius, branchMaterial, leafMaterial, leafPrefab, flowerMaterial, flowerPrefab, false);
        }
        branch.transform.SetParent(ivy.transform);

        ivyCount++;
        IvyNode last = previousNodes[previousNodes.Count - 1];
        startPos = last.getPosition();
        startNormal = last.getNormal();
    }


    public void createBranchedIvy(Vector3 point, Vector3 normal)
    {
        int branches = 8;
        Vector3 tangent = findTangentFromArbitraryNormal(normal);
        GameObject ivy = new GameObject("Ivy " + ivyCount);
        ivy.transform.SetParent(transform);
        for (int i = 0; i < branches; i++)
        {
            Vector3 dir = Quaternion.AngleAxis(360 / branches * i + Random.Range(0, 360 / branches), normal) * tangent;
            List<IvyNode> nodes = createBranch(maxPointsForBranch, point, normal, dir);
            GameObject branch = new GameObject("Branch " + i);
            Branch b = branch.AddComponent<Branch>();
            if (!wantBlossoms)
            {
                b.init(nodes, branchRadius, branchMaterial);
            }
            else
            {
                b.init(nodes, branchRadius, branchMaterial, leafMaterial, leafPrefab, flowerMaterial, flowerPrefab, i == 0);
            }
            branch.transform.SetParent(ivy.transform);
        }

        ivyCount++;
    }

    Vector3 calculateTangent(Vector3 p0, Vector3 p1, Vector3 normal)
    {
        var heading = p1 - p0;
        var distance = heading.magnitude;
        var direction = heading / distance;
        return Vector3.Cross(normal, direction).normalized;
    }

    Vector3 applyCorrection(Vector3 p, Vector3 normal)
    {
        return p + normal * 0.01f;
    }

    bool isOccluded(Vector3 from, Vector3 to)
    {
        Ray ray = new Ray(from, (to - from) / (to - from).magnitude);
        return Physics.Raycast(ray, (to - from).magnitude);
    }

    bool isOccluded(Vector3 from, Vector3 to, Vector3 normal)
    {
        return isOccluded(applyCorrection(from, normal), applyCorrection(to, normal));
    }

    Vector3 calculateMiddlePoint(Vector3 p0, Vector3 p1, Vector3 normal)
    {
        Vector3 middle = (p0 + p1) / 2;
        var h = p0 - p1;
        var distance = h.magnitude;
        var dir = h / distance;
        return middle + normal * distance;
    }

    List<IvyNode> createBranch(int count, Vector3 pos, Vector3 normal, Vector3 dir)
    {

        if (count == maxPointsForBranch)
        {
            IvyNode rootNode = new IvyNode(pos, normal);
            return new List<IvyNode> { rootNode }.join(createBranch(count - 1, pos, normal, dir));
        }
        else if (count < maxPointsForBranch && count > 0)
        {

            if (count % 2 == 0)
            {
                dir = Quaternion.AngleAxis(Random.Range(-20.0f, 20.0f), normal) * dir;
            }

            RaycastHit hit;
            Ray ray = new Ray(pos, normal);
            Vector3 p1 = pos + normal * segmentLength;

            if (Physics.Raycast(ray, out hit, segmentLength))
            {
                p1 = hit.point;
            }
            ray = new Ray(p1, dir);

            if (Physics.Raycast(ray, out hit, segmentLength))
            {
                Vector3 p2 = hit.point;
                IvyNode p2Node = new IvyNode(p2, -dir);
                return new List<IvyNode> { p2Node }.join(createBranch(count - 1, p2, -dir, normal));
            }
            else
            {
                Vector3 p2 = p1 + dir * segmentLength;
                ray = new Ray(applyCorrection(p2, normal), -normal);
                if (Physics.Raycast(ray, out hit, segmentLength))
                {
                    Vector3 p3 = hit.point;
                    IvyNode p3Node = new IvyNode(p3, normal);

                    if (isOccluded(p3, pos, normal))
                    {
                        Vector3 middle = calculateMiddlePoint(p3, pos, (normal + dir) / 2);

                        Vector3 m0 = (pos + middle) / 2;
                        Vector3 m1 = (p3 + middle) / 2;

                        IvyNode m0Node = new IvyNode(m0, normal);
                        IvyNode m1Node = new IvyNode(m1, normal);

                        return new List<IvyNode> { m0Node, m1Node, p3Node }.join(createBranch(count - 3, p3, normal, dir));
                    }

                    return new List<IvyNode> { p3Node }.join(createBranch(count - 1, p3, normal, dir));
                }
                else
                {
                    Vector3 p3 = p2 - normal * segmentLength;
                    ray = new Ray(applyCorrection(p3, normal), -normal);

                    if (Physics.Raycast(ray, out hit, segmentLength))
                    {
                        Vector3 p4 = hit.point;
                        IvyNode p4Node = new IvyNode(p4, normal);

                        if (isOccluded(p4, pos, normal))
                        {
                            Vector3 middle = calculateMiddlePoint(p4, pos, (normal + dir) / 2);
                            Vector3 m0 = (pos + middle) / 2;
                            Vector3 m1 = (p4 + middle) / 2;

                            IvyNode m0Node = new IvyNode(m0, normal);
                            IvyNode m1Node = new IvyNode(m1, normal);

                            return new List<IvyNode> { m0Node, m1Node, p4Node }.join(createBranch(count - 3, p4, normal, dir));
                        }

                        return new List<IvyNode> { p4Node }.join(createBranch(count - 1, p4, normal, dir));
                    }
                    else
                    {
                        Vector3 p4 = p3 - normal * segmentLength;
                        IvyNode p4Node = new IvyNode(p4, dir);

                        if (isOccluded(p4, pos, normal))
                        {
                            Vector3 middle = calculateMiddlePoint(p4, pos, (normal + dir) / 2);

                            Vector3 m0 = (pos + middle) / 2;
                            Vector3 m1 = (p4 + middle) / 2;

                            IvyNode m0Node = new IvyNode(m0, dir);
                            IvyNode m1Node = new IvyNode(m1, dir);

                            return new List<IvyNode> { m0Node, m1Node, p4Node }.join(createBranch(count - 3, p4, dir, -normal));
                        }
                        return new List<IvyNode> { p4Node }.join(createBranch(count - 1, p4, dir, -normal));
                    }
                }
            }

        }
        return null;
    }

    void combineAndClear()
    {
        MeshManager.instance.combineAll();
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

}