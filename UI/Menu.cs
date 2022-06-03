using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;

public class Menu : Panel
{
	private string Title { get; set; }
	private List<Item> MenuItems { get; set; } = new List<Item>();
	private int TimeToDisplay { get; set; }
	private RealTimeSince TimeSinceBorn = 0;
	public bool ExitButton { get; set; } = true;
	private MyPlayer DisplayTo { get; set; }

	public delegate void FireCallback( MenuAction action, MyPlayer player, Item item );
	public FireCallback Callback { get; set; }

	public Menu( FireCallback callback )
	{
		Callback = callback;


		Panel menu = Add.Panel( "menu" );
		{
			// Title
			menu.Add.Label( Title, "title" );

			// Items
			Panel items = menu.Add.Panel( "items" );
			{
				for ( int i = 0; i < MenuItems.Count; i++ )
				{
					items.Add.Label( MenuItems[i].Display, "item" );
				}
			}

			// Exit Button
			if( ExitButton )
			{
				menu.Add.Label( "Quitter", "exit" );
			}
		}
	}

	public void SetTitle( string title )
	{
		Title = title;
	}

	public void AddItem( string callback, string text)
	{
		MenuItems.Add( 
			new Item()
			{
				Callback = callback,
				Display = text
			}
		);
	}

	public void Fire(MenuAction action, MyPlayer player, int item)
	{
		Callback( action, player, MenuItems[item] );
	}

	public void Display(MyPlayer player, int time)
	{
		TimeToDisplay = time;
		DisplayTo = player;
	}

	public override void Tick()
	{
		if ( DisplayTo == null ) 
			return;
		
		if(DisplayTo.lastbutton != 0)
			Fire( MenuAction.Select, DisplayTo, (int)DisplayTo.lastbutton );

		if ( ExitButton && Input.Pressed( InputButton.Slot9 ) )
		{
			Fire( MenuAction.Cancel, DisplayTo, (int)DisplayTo.lastbutton );
			Delete();
		}

		//Input.Down()

		if ( TimeSinceBorn > TimeToDisplay && TimeToDisplay != 0 )
		{
			Fire( MenuAction.End, DisplayTo, (int)DisplayTo.lastbutton );
			Delete();
		}
	}
}

public class Item
{
	public string Callback { get; set; }
	public string Display { get; set; }
}

public enum MenuAction
{
	Start,
	Display,
	Select,
	Cancel,
	End
};
