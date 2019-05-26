using Godot;

namespace Game1.Core.Scripts
{
    public class RPGObstacle : StaticBody2D
    {
        private KinematicBody2D player;
        private Sprite sprite;
        private float spriteHeight, spriteWidth;
        [Export] public bool fade = true;

        public override void _Ready()
        {
            player = GetTree().Root.GetNode<KinematicBody2D>("MainScene/Player");
            sprite = GetNode<Sprite>("Sprite");
            spriteHeight = sprite.GetRect().End.y - sprite.GetRect().Position.y;
            spriteWidth = sprite.GetRect().End.x - sprite.GetRect().Position.x;
        }

        public override void _Process(float delta)
        {
            if (player.GlobalPosition.y < GlobalPosition.y && player.GlobalPosition.y > GlobalPosition.y - spriteHeight && player.GlobalPosition.x > GlobalPosition.x - spriteWidth/2 && player.GlobalPosition.x < GlobalPosition.x + spriteWidth/2)
            {
                sprite.ZIndex = 1;
                if (fade)
                {
                    float alpha = Mathf.Clamp(.5f + Mathf.Abs(player.GlobalPosition.x - GlobalPosition.x)/spriteWidth, 0.5f, 1.0f);
                    sprite.Modulate = new Color(1, 1, 1, alpha);
                }
            }
            else
            {
                sprite.Modulate = new Color(1, 1, 1, 1f);
                sprite.ZIndex = -1;
            }
        }
    }
}