namespace ScreenSound.Modelos;

internal class Musica
{
    public Musica(string nome, int? anoLancamento = null)
    {
        Nome = nome;
        AnoLancamento = anoLancamento;
    }

    public string Nome { get; set; }
    public int Id { get; set; }
    public int? AnoLancamento { get; set; }

    public void ExibirFichaTecnica()
    {
        Console.WriteLine($"Nome: {Nome}");
      
    }

    public override string ToString()
    {
        return @$"Id: {Id}
        Nome: {Nome}
        Ano de Lançamento: {AnoLancamento}";
    }
}