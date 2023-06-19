using AppCadastro.Views.Usuarios;

namespace AppCadastro;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
