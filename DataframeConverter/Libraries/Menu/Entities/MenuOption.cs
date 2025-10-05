namespace DataframeConverter.Libraries.Menu.Entities
{
    public class MenuOption
    {
        public Action _action;

        public string Description { get; set; }

        public MenuOption(string description, Action action)
        {
            Description = description;
            _action = action;
        }

        public void Invoke()
        {
            _action();
        }
    }
}