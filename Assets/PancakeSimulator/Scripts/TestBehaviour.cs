using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
    public ComputeShader shader;
    public Shader unlit;
    public int width;
    public Material[] panMat;
    public Texture2D testTex;
    public Camera observeCam;

    [Range(0, 2)]
    public float intensity;

    PanSim sim;

    void Start()
    {
        sim = new PanSim(width, shader);
        foreach(var mat in panMat)
        {
            mat.mainTexture = sim.renderingMap;
        }
        observeCam.targetTexture = sim.additionalMap;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Graphics.Blit(testTex, sim.color);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            sim.WithDataExtracted(() =>
            {
                for (int i = 440; i < 550; i++)
                {
                    for (int j = 440; j < 550; j++)
                    {
                        sim.SetHeight(i, j, 1);
                    }
                }
            });
        }
        
    }

    private void FixedUpdate()
    {
        observeCam.RenderWithShader(unlit, null);
        sim.intensity = intensity;
        sim.SimulateStep();
    }

    private void OnDestroy()
    {
        sim.Release();
    }
}
