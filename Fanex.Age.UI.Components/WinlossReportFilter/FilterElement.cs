namespace Fanex.Age.UI.Components
{
	public class FilterElement
    {
		public FilterElement(int id, string name, string text, bool isChecked = false)
		{
			Id = id;
			Name = name;
			Text = text;
			Checked = isChecked;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public string Text { get; set; }

		public bool Checked { get; set; }
	}
}