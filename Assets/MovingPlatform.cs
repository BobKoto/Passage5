using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    Vector3[] points = { };
    [SerializeField]
    float speed = 10f;

    int nextPoint = 0;
    Vector3 startPosition;

    public Vector3 velocity { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        if (points == null || points.Length < 2)
        {
            Debug.LogError(" Platform needs 2 or more points to work.");
            return;
        }
        startPosition = transform.position;
        transform.position = currentPoint;
    }
    Vector3 currentPoint
    {
        get
        {
            if (points == null || points.Length == 0)
            {
                return transform.position;
            }
            return points[nextPoint] + startPosition;
        }

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        var newPosition = Vector3.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);
        if (Vector3.Distance(newPosition, currentPoint) < 0.001)
        {
            newPosition = currentPoint;
            nextPoint += 1;
            nextPoint %= points.Length;

        }
        velocity = (newPosition - transform.position) / Time.deltaTime;
        transform.position = newPosition;
    }
    private void OnDrawGizmosSelected()
    {
        if (points == null || points.Length < 2)
        {
            return;
        }
        Vector3 offsetPosition = transform.position;
        if (Application.isPlaying)
        {
            offsetPosition = startPosition;
        }
        Gizmos.color = Color.blue;
        for (int p = 0; p < points.Length; p++)
        {
            var p1 = points[p];
            var p2 = points[(p + 1) % points.Length];
            Gizmos.DrawSphere(offsetPosition + p1, 0.1f);
            Gizmos.DrawLine(offsetPosition + p1, offsetPosition + p2);
        }
    }
}
