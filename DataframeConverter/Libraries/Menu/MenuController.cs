namespace DataframeConverter.Libraries.Menu;

using DataframeConverter.Libraries.Menu.Entities;

class MenuController
{
    private List<MenuOption> _menuOptions;

    public MenuController()
    {
        _menuOptions = new List<MenuOption>();
    }

    public void AddMenuOption(MenuOption option)
    {
        _menuOptions.Add(option);
    }

    public void DisplayMenu()
    {
        Console.WriteLine("Menu Options:");
        Console.WriteLine("----------");
        for (int i = 0; i < _menuOptions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_menuOptions[i].Description}");
        }
        Console.WriteLine("0. Exit");
    }

    public void HandleUserInput()
    {
        while (true)
        {
            DisplayMenu();
            System.Console.WriteLine("----------");
            Console.Write("Select an option: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                if (choice == 0)
                {
                    break;
                }
                else if (choice > 0 && choice <= _menuOptions.Count)
                {
                    _menuOptions[choice - 1].Invoke();
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
}