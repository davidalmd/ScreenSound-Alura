
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.Menus;

internal class MenuMostrarMusicasLancamento : Menu
{
    public override void Executar(DAL<Artista> artistaDAL)
    {
        base.Executar(artistaDAL);
        ExibirTituloDaOpcao("Exibir músicas por ano de lançamento");
        Console.Write("Digite o ano de lançamento que você deseja pesquisar: ");
        string anoLancamento = Console.ReadLine()!;
        var musicaDAL = new DAL<Musica>(new ScreenSoundContext());
        var musicasRecuperadas = musicaDAL.RecuperarPorLista(a => a.AnoLancamento.Equals(Convert.ToInt32(anoLancamento)));
        if (musicasRecuperadas.Any())
        {
            Console.WriteLine($"\nMusicas lançadas no ano {anoLancamento}:");
            foreach (var musica in musicasRecuperadas)
            {
                musica.ExibirFichaTecnica();
            }
            Console.WriteLine("\nDigite uma tecla para voltar ao menu principal");
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine($"\nNão existem músicas salvas no sistema que foram lançadas no ano {anoLancamento}");
            Console.WriteLine("Digite uma tecla para voltar ao menu principal");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
