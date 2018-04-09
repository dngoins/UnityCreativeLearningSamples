using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float GraphFunction(float x, float z, float t);
public delegate Vector3 Graph3DFunction(float u, float v, float t);

public enum GraphFunctionName
{
    Sine,
    Sine2D,
    SineIndependent2D,
    MultiSine,
    MultiSine2D,
    MultiSineIndependent2D,
        MultiSineIndependentSlow2D,
        Ripple,
        RippleSine
}

public enum Graph3DFunctionName
{
    Sine,
    Sine2D,
    MultiSine,
    MultiSine2D,
    Ripple,
    Cylinder,
    RandomCylinder,
    Vase,
    TwistingCylinder,
    Sphere,
    Torus,
    PulsatingSphere
}

public class Graph : MonoBehaviour {

    static GraphFunction[] functions = {
        SineFunction, Sine2DFunction, SineInd2DFunction, MultiSineFunction, MultiSine2DFunction, MultiSineInd2DFunction, MultiSineIndSlow2DFunction, Ripple, RippleSine
    };

    static Graph3DFunction[] functions3D =
    {
        Sine3DFunction, Sine3D2DFunction, MultiSine3DFunction, MultiSine3D2DFunction,  Ripple3D, Cylinder, RandomCylinder, RandomVaseCylinder, TwistingCylinder, Sphere, Torus, PulsingSphere
        

    };

    public GraphFunctionName function;
    public Graph3DFunctionName function3D;

    public Transform pointsPrefab;

    public bool is3D;

    [Range(5, 250)]
    public int resolution = 50;


    Transform[] points;

    void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        points = new Transform[resolution * resolution];

        Vector3 position;
        position.y = 0f;
        position.z = 0f;

        if (is3D)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = Instantiate(pointsPrefab);
                point.localScale = scale;
                point.SetParent(transform, false);
                points[i] = point;
            }

        }
        else
        {
            for (int i = 0, z = 0; z < resolution; z++)
            {
                position.z = (z + 0.5f) * step - 1f;

                for (int x = 0; x < resolution; x++, i++)
                {
                    Transform point = Instantiate(pointsPrefab);
                    position.x = (x + 0.5f) * step - 1f;
                    point.localPosition = position;
                    point.localScale = scale;
                    point.SetParent(transform, false);
                    points[i] = point;
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
       
    }

    // Update is called once per frame
    void Update () {

        float t = Time.time;
        GraphFunction f = functions[(int)function];
        Graph3DFunction f3d = functions3D[(int)function3D];

        if (is3D)
        {
            float step = 2f / resolution;
            for (int i = 0, z = 0; z < resolution; z++)
            {
                float v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < resolution; x++, i++)
                {
                    float u = (x + 0.5f) * step - 1f;
                    points[i].localPosition = f3d(u, v, t);
                }
            }

        }
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i];
                Vector3 position = point.localPosition;
                position.y = f(position.x, position.z, t);
                point.localPosition = position;
            }
        }
    }

    const float pi = Mathf.PI;

    static float SineFunction(float x, float z, float t)
    {
        return Mathf.Sin(pi * (x + t));
    }

    static float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) * 0.5f; // / 2f;
        y *= 0.66f; // 2f / 3f;
        return y;
    }

    static float Sine2DFunction (float x, float z, float t)
    {
        return Mathf.Sin(pi * (x + z + t));
    }

    static float MultiSine2DFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + z + t));
        y += Mathf.Sin(2f * pi * (x + z + 2f * t)) * 0.5f;
        y *= 0.66f; // 2f / 3f;
        return y;
    }

    static float SineInd2DFunction(float x, float z, float t)
    {
        var y = (SineFunction(x,0, t) + SineFunction(z, 0, t)) *.5f;
        return y;
    }

    static float MultiSineInd2DFunction(float x, float z, float t)
    {
        float y =  SineInd2DFunction(x, z, t);
        y += Mathf.Sin(2f * pi * (x + z + 2f * t)) * 0.5f;
        y *= 0.66f; // 2/3 
        return y;
    }

    static float MultiSineIndSlow2DFunction(float x, float z, float t)
    {
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        return y;

    }

    static float Ripple(float x, float z, float t)
    {
        float d = Mathf.Sqrt(x * x + z * z);
        float y = d;
        return y;
    }

    static float RippleSine(float x, float z, float t)
    {
        float d = Ripple(x, z, t);
        float y = Mathf.Sin(pi * (4f * d - t));
        y /= 1f + 10f * d;
        return y;
    }

    #region "3D functions"
    static Vector3 Sine3DFunction(float x, float z, float t)
    {
        //        return Mathf.Sin(pi * (x + t));
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 Sine3D2DFunction(float x, float z, float t)
    {
        //        float y = Mathf.Sin(pi * (x + t));
        //        y += Mathf.Sin(pi * (z + t));
        //        y *= 0.5f;
        //        return y;

        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;
    }
    

    static Vector3 MultiSine3DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSine3D2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        p.y *= 1f / 5.5f;
        p.z = z;
        return p;
    }

    static Vector3 Ripple3D(float x, float z, float t)
    {
        Vector3 p;
        float d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(pi * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = z;
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;

        float r = 1f;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);

        return p;
    }

    static Vector3 RandomCylinder(float u, float v, float t)
    {
        Vector3 p;
        //    float r = 1f + Mathf.Sin(2f * pi * v) * 0.2f;
        float r = 1f + Mathf.Sin(6f * pi * u) * 0.2f;
        p.x = r * Mathf.Sin(pi * u) ;
        p.y = r * Mathf.Sin(pi * u - t) * Mathf.Cos(pi * v + t);
        p.z = r * Mathf.Cos(pi * u);

        return p;
    }

    static Vector3 RandomVaseCylinder(float u, float v, float t)
    {
        Vector3 p;
        // float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
        float r = 1f + Mathf.Sin(2f * pi * v) * 0.2f;
        //float r = 1f + Mathf.Sin(6f * pi * u) * 0.2f;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);

        return p;
    }

    static Vector3 TwistingCylinder(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);

        return p;
    }

    static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        float r = Mathf.Cos(pi * 0.5f * v);
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        // p.y = Mathf.Sin(pi * 0.5f * v);
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 PulsingSphere(float u, float v, float t)
    {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = r * Mathf.Cos(pi * 0.5f * v);
        p.x = s * Mathf.Sin(pi * u);
        p.y = r * Mathf.Sin(pi * 0.5f * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        //  float s = Mathf.Cos(pi * 0.5f * v) + 0.5f;
        //float s = Mathf.Cos(pi * 0.5f * v);
        //p.x = s * Mathf.Sin(pi * u);
        //p.y = Mathf.Sin(pi * 0.5f * v);
        //p.z = s * Mathf.Cos(pi * u);

        //self intersecting
        //float s = Mathf.Cos(pi * v) + 0.5f;
        //p.x = s * Mathf.Sin(pi * u);
        //p.y = Mathf.Sin(pi * v);
        //p.z = s * Mathf.Cos(pi * u);

        //horn torus
        //float r1 = 1f;
        //float s = Mathf.Cos(pi * v) + r1;

        //ring torus
        float r1 = 1f;
        float r2 = 0.5f;
        float s = r2 * Mathf.Cos(pi * v) + r1;
        p.x = s * Mathf.Sin(pi * u);
        p.y = r2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);

        // star torus
        //float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        //float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;

        return p;
    }

    #endregion
}
