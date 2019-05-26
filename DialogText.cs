using Godot;

namespace Game1.Core.Scripts
{
    public class DialogText : RichTextLabel
    {
        private Polygon2D box;
        private string[] dialog;
        private int page = 0;
        private bool active = false;

        public override void _Ready()
        {
            box = GetParent<Polygon2D>();
            SetProcessInput(true);
            SetVisibleCharacters(0);
            hide();
        }

        public override void _Input(InputEvent @event)
        {
            if (active && @event.IsActionPressed("left_click"))
            {
                GetTree().SetInputAsHandled();
                if (VisibleCharacters >= GetTotalCharacterCount())
                {
                    if (page < dialog.Length - 1)
                    {
                        page += 1;
                        SetBbcode(dialog[page]);
                        VisibleCharacters = 0;
                    }
                    else
                    {
                        hide();
                        page = 0;
                    }
                }
                else
                {
                    VisibleCharacters = GetTotalCharacterCount();
                }
            }
        }
        
        private void _on_Timer_timeout()
        {
            VisibleCharacters += 1;
        }

        public void _ReceiveText(string text)
        {
            page = 0;
            dialog = text.Split("\\n");
            SetBbcode(dialog[0]);
            SetVisibleCharacters(0);
            show();
        }

        private void hide()
        {
            active = false;
            EmitSignal(nameof(SignalDialogActive), active);
            Visible = false;
            box.Visible = false;
        }

        private void show()
        {
            active = true;
            EmitSignal(nameof(SignalDialogActive), active);
            Visible = true;
            box.Visible = true;
        }
        
        [Signal]
        delegate void SignalDialogActive(bool active);
    }
}