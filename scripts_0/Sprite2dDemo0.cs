using Godot;

public partial class Sprite2dDemo0 : Sprite2D
{
    private int _speed = 400;
    // 角度默认以弧度为单位
    private float _angularSpeed = Mathf.Pi;

    public override void _Ready()
    {
        var timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
    }


    // delta 时间值让运动和帧速率无关
    public override void _Process(double delta)
    {
        // 应用于旋转时，将 delta 转换为 float
        Rotation += _angularSpeed * (float)delta;
        var velocity = Vector2.Up.Rotated(Rotation) * _speed;
        Position += velocity * (float)delta;

        // var direction = 0;
        // if (Input.IsActionPressed("ui_left"))
        // {
        //     direction = -1;
        // }
        // if (Input.IsActionPressed("ui_right"))
        // {
        //     direction = 1;
        // }
        // Rotation += _angularSpeed * direction * (float)delta;
        // var velocity = Vector2.Zero;
        // if (Input.IsActionPressed("ui_up"))
        // {
        //     velocity = Vector2.Up.Rotated(Rotation) * _speed;
        // }
        // Position += velocity * (float)delta;
    }

    private void OnButtonPressed()
    {   
        SetProcess(!IsProcessing());
    }

    private void OnTimerTimeout()
    {
        Visible = !Visible;
    }
}
