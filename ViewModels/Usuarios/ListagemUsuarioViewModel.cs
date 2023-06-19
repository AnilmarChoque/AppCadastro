using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCadastro.Models;
using AppCadastro.Services.Usuarios;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppCadastro.ViewModels.Usuarios
{
    public class ListagemUsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;

        public ObservableCollection<Usuario> Usuarios { get; set; }

        public ListagemUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            Usuarios = new ObservableCollection<Usuario>();

            _ = ObterUsuario();

            NovoUsuario = new Command(async () => { await ExibirCadastroUsuario(); });
            RemoverUsuarioCommand = new Command<Usuario>(async (Usuario u) => { await RemoverUsuario(u); });
        }

        public ICommand NovoUsuario { get; }
        public ICommand RemoverUsuarioCommand { get; }

        public async Task ObterUsuario()
        {
            try
            {
                Usuarios = await uService.GetUsuariosAsync();
                OnPropertyChanged(nameof(Usuarios));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroUsuario()
        {
            try
            {
                await Shell.Current.GoToAsync("cadUsuarioView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Usuario usuarioSelecionado;

        public Usuario UsuarioSelecionado
        {
            get { return usuarioSelecionado; }
            set
            {
                if(value != null)
                {
                    usuarioSelecionado = value;

                    Shell.Current
                        .GoToAsync($"cadUsuarioView?uId={usuarioSelecionado.Id}");
                }
            }
        }

        public async Task RemoverUsuario(Usuario u)
        {
            try
            {
                if (await Application.Current.MainPage
                    .DisplayAlert("Confirmação", $"Confirma a remoção de {u.Nome}?", "Sim", "Não"))
                {
                    await uService.DeleteUsuarioAsync(u.Id);

                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Usuario removido com sucesso!", "Ok");

                    _ = ObterUsuario();
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
