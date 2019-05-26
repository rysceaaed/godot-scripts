using Godot;

namespace Game1.Core.Scripts
{
    public class Player : KinematicBody2D
    {
        [Export] public int Speed = 75;
        
        private Vector2 velocityX = new Vector2();
        private Vector2 velocityY = new Vector2();
        public Vector2 targetX { get; set; } = new Vector2();
        public Vector2 targetY { get; set; } = new Vector2();
        private RichTextLabel dialogText;
        private bool dialogActive = false;
        private AnimatedSprite anim;
        private Direction direction;

        private enum Direction
        {
            UP,
            DOWN,
            RIGHT,
            LEFT,
            NONE
        }

        public override void _Ready()
        {
            dialogText = GetTree().Root.GetNode<RichTextLabel>("MainScene/HUDLayer/DialogBox/DialogText");
            dialogText.Connect("SignalDialogActive", this, "SetDialog");
            anim = GetNode<AnimatedSprite>("AnimatedSprite");
            anim.Animation = "Back";
            anim.Playing = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (dialogActive) return;
            if (@event.IsActionPressed("left_click"))
            {
                targetX = new Vector2(GetGlobalMousePosition().x, 0);
                targetY = new Vector2(0, GetGlobalMousePosition().y);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            var positionX = new Vector2(Position.x, 0);
            var positionY = new Vector2(0, Position.y);
            velocityX = (targetX - positionX).Normalized() * Speed;
            velocityY = (targetY - positionY).Normalized() * Speed;
            if (dialogActive)
            {
                targetX = positionX;
                targetY = positionY;
                direction = Direction.NONE;
                return;
            }
            
            if ((targetX - positionX).Length() > 5)
            {
                if (MoveAndCollide(velocityX * delta) != null)
                {
                    targetX = positionX;
                    targetY = positionY;
                    direction = Direction.NONE;
                    return;
                }
                if (targetX.x - positionX.x > 0) direction = Direction.RIGHT;
                else if (targetX.x - positionX.x < 0) direction = Direction.LEFT;
            }
            else if ((targetY - positionY).Abs().Length() > 5)
            {
                if (MoveAndCollide(velocityY * delta) != null)
                {
                    targetX = positionX;
                    targetY = positionY;
                    direction = Direction.NONE;
                    return;
                }
                if (targetY.y - positionY.y > 0) direction = Direction.DOWN;
                else if (targetY.y - positionY.y < 0) direction = Direction.UP;
            }
            else
            {
                direction = Direction.NONE;
            }
        }

        public override void _Process(float delta)
        {
            anim.Playing = true;
            switch (direction)
            {
                case Direction.NONE:
                    anim.Playing = false;
                    anim.Frame = 0;
                    break;
                case Direction.UP:
                    anim.Animation = "Back";
                    break;
                case Direction.DOWN:
                    anim.Animation = "Forward";
                    break;
                case Direction.LEFT:
                    anim.Animation = "Left";
                    break;
                case Direction.RIGHT:
                    anim.Animation = "Right";
                    break;
            }
        }

        public void SetDialog(bool active)
        {
            dialogActive = active;
        }
    }
}