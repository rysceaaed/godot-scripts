using Godot;
using Object = Godot.Object;

namespace Game1.Core.Scripts
{
    public class InteractableArea : Area2D
    {
        [Export] public string text = "Interacted";
        private RichTextLabel dialogText;
        private bool dialogActive = false;
        private KinematicBody2D player;
        private CollisionShape2D collisionShape;
        private RectangleShape2D rectangleShape;
        private Rect2 rect;

        public override void _Ready()
        {
            SetProcessInput(true);
            dialogText = GetTree().Root.GetNode<RichTextLabel>("MainScene/HUDLayer/DialogBox/DialogText");
            Connect(nameof(SendTextToDialog), dialogText, "_ReceiveText");
            dialogText.Connect("SignalDialogActive", this, "SetDialog");
            player = GetTree().Root.GetNode<KinematicBody2D>("MainScene/Player");
            collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
            rectangleShape = (RectangleShape2D) collisionShape.Shape;
            rect = new Rect2(collisionShape.GlobalPosition.x - rectangleShape.Extents.x,
                collisionShape.GlobalPosition.y - rectangleShape.Extents.y, rectangleShape.Extents.x * 2, rectangleShape.Extents.y * 2);
        }

        public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
        {
            if (dialogActive)
            {
                return;
            }

            if (@event.IsActionPressed("left_click"))
            {
                GetTree().SetInputAsHandled();
                if (GlobalPosition.DistanceTo(player.GlobalPosition) < 16)
                {
                    EmitSignal(nameof(SendTextToDialog), text);
                }
                else
                {
                    player.Set("targetX", new Vector2(GetGlobalMousePosition().x, 0));
                    player.Set("targetY", new Vector2(0, GetGlobalMousePosition().y));
                }
            }
        }

        public override void _Process(float delta)
        {
            if (rect.HasPoint(GetGlobalMousePosition()))
            {
                Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
            }
            else
            {
                Input.SetDefaultCursorShape();
            }
        }

        public void SetDialog(bool active)
        {
            dialogActive = active;
        }

        [Signal]
        delegate void SendTextToDialog(string text);
    }
}