using AppCadastro.ViewModels;
using AppCadastro.ViewModels.Usuarios;

namespace AppCadastro.Views.Usuarios;

public partial class ListagemView : ContentPage
{
	ListagemUsuarioViewModel viewModel;
	public ListagemView()
	{
		InitializeComponent();

		viewModel = new ListagemUsuarioViewModel();
		BindingContext = viewModel;
		Title = "Usuarios - App Programinds";
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _ = viewModel.ObterUsuario();
    }
}