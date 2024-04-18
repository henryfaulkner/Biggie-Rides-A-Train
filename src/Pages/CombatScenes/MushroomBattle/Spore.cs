using Godot;
using System;

public partial class Spore : Node2D
{
    public static readonly Vector2I _windowSize = new Vector2I(2048, 1024);
    private Vector2 pos;
    private Vector2 vel;
    private Vector2 acc;
    private float angle;
    private int dir;
    private float xOff;
    private float r;

    public override void _Ready()
    {
        float x = Mathf.Lerp(0f, _windowSize.X, (float)GD.Randf());
        float y = Mathf.Lerp(-100f, -10f, (float)GD.Randf());
        this.pos = new Vector2(x, y);
        this.vel = Vector2.Zero;
        this.acc = Vector2.Zero;
        this.angle = Mathf.Lerp(0f, Mathf.Pi * 2, (float)GD.Randf());
        this.dir = GD.Randf() < 0.5 ? -1 : 1;
        this.xOff = 0;
        this.r = getRandomSize();
    }

    private float getRandomSize()
    {
        float r = Mathf.Pow((float)GD.Randf(), 3);
        return Mathf.Clamp(r * 32f, 2f, 32f);
    }

    public void ApplyForce(Vector2 force)
    {
        Vector2 f = force * this.r;
        this.acc += f;
    }

    public void Randomize()
    {
        float x = Mathf.Lerp(0f, _windowSize.X, (float)GD.Randf());
        float y = Mathf.Lerp(-100f, -10f, (float)GD.Randf());
        this.pos = new Vector2(x, y);
        this.vel = Vector2.Zero;
        this.acc = Vector2.Zero;
        this.r = getRandomSize();
    }

    public override void _Process(double _delta)
    {
        this.xOff = Mathf.Sin(this.angle * 2) * 2 * this.r;

        this.vel += this.acc;
        this.vel = this.vel.LimitLength(this.r * 0.2f);

        if (this.vel.Length() < 1)
        {
            this.vel = this.vel.Normalized();
        }

        this.pos += this.vel;
        this.acc *= 0;

        if (this.pos.Y > _windowSize.Y + this.r)
        {
            Randomize();
        }

        // Wrapping Left and Right
        if (this.pos.X < -this.r)
        {
            this.pos.X = _windowSize.X + this.r;
        }
        if (this.pos.X > _windowSize.X + this.r)
        {
            this.pos.X = -this.r;
        }

        this.angle += this.dir * this.vel.Length() / 200;
    }

    public override void _Draw()
    {
        DrawTextureRect(
            GD.Load<Texture2D>("res://your_texture.png"),
            new Rect2(this.pos.X, this.pos.Y, this.r, this.r),
            false,
            new Color(0, 0, 1, 1)
        );
    }
}