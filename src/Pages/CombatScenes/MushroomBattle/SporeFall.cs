using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class Snowfall : Node2D
{
    private List<Spore> _nodeSporeList = new List<Spore>();
    private Vector2 gravity;
    private float zOff = 0;

    private Texture spritesheet;
    private List<Texture> textures = new List<Texture>();

    public override void _Ready()
    {
        gravity = new Vector2(0, 0.3f);

        for (int i = 0; i < 400; i++)
        {
            float x = (float)GD.RandRange(0, GetViewportRect().Size.X);
            float y = 0;
            Texture design = textures[(int)GD.RandRange(0, textures.Count)];
            Spore spore = new Spore()
            {
                Position = new Vector2(x, y)
            };
            AddChild(spore);
            _nodeSporeList.Add(spore);
        }
    }

    public override void _Process(double _delta)
    {
        zOff += 0.1f;

        foreach (Spore spore in _nodeSporeList)
        {
            float xOff = spore.Position.X / GetViewportRect().Size.X;
            float yOff = spore.Position.Y / GetViewportRect().Size.Y;
            //float wAngle = Mathf.Noise(xOff, yOff, zOff) * Mathf.Tau;
            float wAngle = GetFastNoiseLite().GetNoise2D(xOff, yOff) * Mathf.Tau;

            Vector2 wind = new Vector2(Mathf.Cos(wAngle), Mathf.Sin(wAngle)) * 0.1f;

            spore.ApplyForce(gravity);
            spore.ApplyForce(wind);
            //spore.Update();
        }
    }

    private FastNoiseLite GetFastNoiseLite()
    {
        return new FastNoiseLite()
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
        };
    }
}