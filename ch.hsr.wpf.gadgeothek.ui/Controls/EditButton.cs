using System;

using System.Windows;
using System.Windows.Controls;

namespace ch.hsr.wpf.gadgeothek.ui.Controls
{
    class EditButton: Button
    {
        public bool EditBoxFilled { get; set; } = false;
        public bool Editing { get; set; } = false;
        public Button Button { get; }

        public EditButton(Button button)
        {
            Button = button;
        }

        public virtual void OnClick(ref object sender,ref RoutedEventArgs e, Func<bool,string> setFormEditability )
        {
            if (!Editing)
            {
                Editing = true;
                setFormEditability(true);
            }
            else
            {
                Editing = false;
                setFormEditability(false);
            }
            UpdateEditButton();
        }
        public void UpdateEditButton()
        {
            if (EditBoxFilled)
            {
                if (Editing)
                {
                    Button.IsEnabled = true;
                    Button.Content = "Discard";
                }
                else
                {
                    Button.IsEnabled = true;
                    Button.Content = "Edit";
                }
            }
            else
            {
                Button.IsEnabled = false;
                Button.Content = "Edit";
            }
        }
    }
}
