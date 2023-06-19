using AppCadastro.ViewModels.Usuarios;

namespace AppCadastro.Views.Usuarios;

public partial class CadastroUsuarioView : ContentPage
{
	private CadastroUsuarioViewModel cadViewModel;
	public CadastroUsuarioView()
	{
		InitializeComponent();

		cadViewModel = new CadastroUsuarioViewModel();
		BindingContext = cadViewModel;
		Title = "Novo Usuario";
	}
}