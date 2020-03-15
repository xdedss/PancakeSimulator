using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanSim
{
    public ComputeShader shader;
    public int width;
    public ComputeBuffer data;
    public ComputeBuffer data_;
    public RenderTexture color;
    public RenderTexture color_;
    public RenderTexture renderingMap;
    public RenderTexture additionalMap;
    public PixelData[] managedData;
    public float intensity = 1;

    public struct PixelData
    {
        public const int LEN = 12;

        public float height;
        public Vector2 velocity;
    };


    int XY2I(int x, int y)
    {
        return y * width + x;
    }

    bool PosValid(int x, int y)
    {
        return x >= 0 && x < (uint)width && y >= 0 && y < (uint)width;
    }

    public PanSim(int width, ComputeShader shader)
    {
        this.width = width;
        this.shader = shader;

        managedData = new PixelData[width * width];
        data = new ComputeBuffer(width * width, PixelData.LEN);
        data_ = new ComputeBuffer(width * width, PixelData.LEN);
        color = new RenderTexture(width, width, 0, RenderTextureFormat.ARGB32);
        color.enableRandomWrite = true;
        color.Create();
        color_ = new RenderTexture(width, width, 0, RenderTextureFormat.ARGB32);
        color_.enableRandomWrite = true;
        color_.Create();
        renderingMap = new RenderTexture(width, width, 0, RenderTextureFormat.ARGB32);
        renderingMap.enableRandomWrite = true;
        renderingMap.Create();
        additionalMap = new RenderTexture(width, width, 0, RenderTextureFormat.ARGB32);
        additionalMap.enableRandomWrite = true;
        additionalMap.Create();
        int kernel = shader.FindKernel("CSMain");
        shader.SetTexture(kernel, "ColorMap", color);
        shader.SetTexture(kernel, "ColorMap_", color_);
        shader.SetTexture(kernel, "RenderingMap", renderingMap);
        shader.SetTexture(kernel, "AdditionalMap", additionalMap);
        shader.SetBuffer(kernel, "Data", data);
        shader.SetBuffer(kernel, "Data_", data_);
        shader.SetInt("width", width);
    }

    public void SetHeight(int x, int y, float height)
    {
        managedData[XY2I(x, y)].height = height;
    }

    public void WithDataExtracted(Action action)
    {
        data.GetData(managedData);
        action();
        data.SetData(managedData);
    }

    public void SimulateStep()
    {
        //Debug.Log("step");
        int kernel = shader.FindKernel("CSMain");
        uint x, y, z;
        shader.GetKernelThreadGroupSizes(kernel, out x, out y, out z);
        shader.SetFloat("seed", UnityEngine.Random.value);
        shader.SetFloat("intensity", intensity);
        shader.SetBool("back", false);
        shader.Dispatch(kernel, width / (int)x + 1, width / (int)y + 1, 1);
        shader.SetBool("back", true);
        shader.Dispatch(kernel, width / (int)x + 1, width / (int)y + 1, 1);
    }

    public void Release()
    {
        data.Dispose();
        data_.Dispose();
        color.Release();
        color_.Release();
        renderingMap.Release();
        additionalMap.Release();
    }
}