using System;
namespace Diskretka_7
{
	public class Interface
	{
		public Interface()
		{
            static void resetWeights(HorizontalStackLayout parent)
            {
                foreach (Entry entry in parent.Children)
                {
                    entry.BackgroundColor = Color.FromArgb("#00FFFFFF");
                    entry.Text = String.Empty;
                }
            }
        }
	}
}

