using Godot;
using System;

public class Settings : Node
{
    private bool _fullscreen;
    private readonly Vector2 _windowSize = new Vector2(960, 540);
    private readonly Vector2 _fullscreenSize = new Vector2(1920, 1080);

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("fullscreen"))
        {
            if (!_fullscreen)
            {
                OS.WindowBorderless = true;
                OS.WindowSize = _fullscreenSize;
                OS.CenterWindow();
                _fullscreen = true;
            }
            else
            {
                OS.WindowBorderless = false;
                OS.WindowSize = _windowSize;
                OS.CenterWindow();
                _fullscreen = false;
            }
        }
        else if (@event.IsActionPressed("quit"))
        {
            GetTree().Quit();
        }
    }
}
