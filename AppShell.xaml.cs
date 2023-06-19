using AppCadastro.Views.Usuarios;

namespace AppCadastro;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("cadUsuarioView", typeof(CadastroUsuarioView));
	}
}
