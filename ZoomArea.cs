using Godot;
using System;

public class ZoomArea : Area2D
{
    [Export] public bool ZoomY;
    [Export] public float Magnification = 0.5f;
    [Export] public float DefaultZoom = 1f;
    [Export] public bool Enabled = true;
    private KinematicBody2D _player;
    private Camera2D _camera;
    private CollisionShape2D _box;
    private RectangleShape2D _rectangle;
    private Rect2 _rect;
    private float _currentZoomFactor;
    
    public override void _Ready()
    {
        _currentZoomFactor = DefaultZoom;
        _player = GetNode<KinematicBody2D>("/root/MainScene/Player");
        _camera = _player.GetNode<Camera2D>("Camera2D");
        _box = GetNode<CollisionShape2D>("CollisionShape2D");
        _rectangle = _box.GetShape() as RectangleShape2D;
        _rect = new Rect2(_box.GlobalPosition - _rectangle.Extents, _rectangle.Extents * 2);
    }

    public override void _Process(float delta)
    {
        if (!Enabled) return;
        if (_rect.HasPoint(_player.GlobalPosition))
        {
            float zoomFactor;
            if (ZoomY)
            {
                if (Magnification < 1)
                {
                    zoomFactor = 1 - Magnification * Mathf.Abs(_rect.Position.y - _player.GlobalPosition.y) / Mathf.Abs(_rect.Position.y - _rect.End.y);
                }
                else
                {
                    zoomFactor = Mathf.Clamp(Magnification * Mathf.Abs(_rect.Position.y - _player.GlobalPosition.y) / Mathf.Abs(_rect.Position.y - _rect.End.y), DefaultZoom, Magnification);
                }
            }
            else
            {
                if (Magnification < 1)
                {
                    zoomFactor = 1 - Magnification * Mathf.Abs(_rect.Position.x - _player.GlobalPosition.x) / Mathf.Abs(_rect.Position.x - _rect.End.x);
                }
                else
                {
                    zoomFactor = Mathf.Clamp(Magnification * Mathf.Abs(_rect.Position.x - _player.GlobalPosition.x) / Mathf.Abs(_rect.Position.x - _rect.End.x), DefaultZoom, Magnification);
                }
            }
            zoomFactor = (float) Math.Round(zoomFactor, 2, MidpointRounding.AwayFromZero);
            if (Mathf.Abs(1 - zoomFactor - Magnification) <= 0.05) zoomFactor = 1 - Magnification;
            if (Mathf.Abs(zoomFactor - DefaultZoom) <= 0.05) zoomFactor = DefaultZoom;
            _currentZoomFactor = zoomFactor;
            if (_camera.Zoom == new Vector2(_currentZoomFactor, _currentZoomFactor)) return;
            _camera.Zoom = new Vector2(zoomFactor, zoomFactor);
        }
    }
}
