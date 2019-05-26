using Godot;
using System;

public class PlatformerPlayer : KinematicBody2D
{
    private enum State
    {
        IDLE,
        RUN,
        JUMP
    }

    [Export] public int RunSpeed = 200;
    [Export] public int JumpSpeed = 350;
    [Export] public int Gravity = 1200;
    private State _state = State.IDLE;
    private Vector2 _velocity;
    private AnimatedSprite _anim;
    private bool _facingLeft = false;

    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    private void _GetInput()
    {
        _velocity.x = 0;
        var left = Input.IsActionPressed("left");
        var right = Input.IsActionPressed("right");
        var jump = Input.IsActionPressed("jump");
        
        if (jump && IsOnFloor())
        {
            _velocity.y = -JumpSpeed;
            _state = State.JUMP;
        }
        if (left)
        {
            _state = State.RUN;
            _velocity.x -= RunSpeed;
            _facingLeft = true;
        }
        if (right)
        {
            _state = State.RUN;
            _velocity.x += RunSpeed;
            _facingLeft = false;
        }
        //_facingLeft = _velocity.x < 0;
        if (!right && !left && _state == State.RUN)
            _state = State.IDLE;
    }

    public override void _Process(float delta)
    {
        _GetInput();
        if (_state == State.IDLE)
        {
            _anim.Playing = false;
            _anim.Frame = 0;
        }
        else
        {
            _anim.Playing = true;
        }
        _anim.FlipH = _facingLeft;
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity.y += Gravity * delta;
        if (_state == State.JUMP && IsOnFloor())
            _state = State.IDLE;
        _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));

        if (Position.y > 600)
            GetTree().ReloadCurrentScene();
    }
}
