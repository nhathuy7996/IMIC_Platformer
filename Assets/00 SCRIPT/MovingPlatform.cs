using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float Speed;
    int way = 1;
    [SerializeField] List<Vector3> Point = new List<Vector3>();
    [SerializeField] int CurrentIndex = 0;
    PathCreator Path;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("ChangeDir",3,3);
        Path = this.GetComponent<PathCreator>();
        Point = Path.List_Points;
        this.transform.position = Point[CurrentIndex];

    }

    // Update is called once per frame
    void Update()
    {
        if (Point.Count <= 1)
            return;
        for (int i =0; i<= Point.Count-2; i++)
        {
            Debug.DrawLine(Point[i], Point[i+1],Color.yellow);
        }

        if (CurrentIndex >= Point.Count - 1 && way == 1)
        {
            way = -1;
        }
        else if (CurrentIndex == 0 && way == -1)
        {
            way = 1;
        }
        if (Vector2.Distance(this.transform.position, Point[CurrentIndex + way]) <= 0.5f)
        {
            CurrentIndex += way;
        }
      
        this.transform.position = Vector2.MoveTowards(this.transform.position, Point[CurrentIndex + way], Speed * Time.deltaTime);
    }

    void ChangeDir()
    {
        way = way == 1 ?-1 : 1;
    }
}
