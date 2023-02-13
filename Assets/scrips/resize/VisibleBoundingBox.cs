using System;
using UnityEngine;
using System.Text;

public class VisibleBoundingBox : MonoBehaviour
{
    public Camera mainCamera;
    public Transform objectTransform;

    private Vector3[] corners;
    private Vector3[] visibleCorners;

    void Start()
    {
        Bounds bounds = GetBounds();
        corners = GetCorners(bounds);
    }

    void Update()
    {
        visibleCorners = GetVisibleCorners();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Visible Corners:");
        for (int i = 0; i < visibleCorners.Length; i++)
        {
            sb.AppendLine("Corner " + i + ": " + visibleCorners[i].ToString("F4"));
        }
        Debug.Log(sb.ToString());
    }


    private Bounds GetBounds()
    {
        Renderer renderer = objectTransform.GetComponent<Renderer>();
        return renderer.bounds;
    }

    private Vector3[] GetCorners(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3[] corners = new Vector3[8];

        corners[0] = center + new Vector3(extents.x, extents.y, extents.z);
        corners[1] = center + new Vector3(extents.x, extents.y, -extents.z);
        corners[2] = center + new Vector3(extents.x, -extents.y, extents.z);
        corners[3] = center + new Vector3(extents.x, -extents.y, -extents.z);
        corners[4] = center + new Vector3(-extents.x, extents.y, extents.z);
        corners[5] = center + new Vector3(-extents.x, extents.y, -extents.z);
        corners[6] = center + new Vector3(-extents.x, -extents.y, extents.z);
        corners[7] = center + new Vector3(-extents.x, -extents.y, -extents.z);

        return corners;
    }

    private Vector3[] GetVisibleCorners()
    {
        Vector3[] visibleCorners = new Vector3[4];
        int visibleCornersCount = 0;

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 corner = objectTransform.TransformPoint(corners[i]);
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(corner);

            if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                screenPoint.y >= 0 && screenPoint.y <= Screen.height &&
                screenPoint.z > 0)
            {
                visibleCorners[visibleCornersCount++] = corner;
                if (visibleCornersCount == 4)
                {
                    break;
                }
            }
        }

        return visibleCorners;
    }
}