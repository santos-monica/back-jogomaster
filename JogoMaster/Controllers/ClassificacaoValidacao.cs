using JogoMaster.Models;

namespace JogoMaster.Controllers
{
    public partial class ClassificacaoController : Validacao
    {
        private void ValidaClassificacao(ViewClassificacao dados)
        {
            Refute(string.IsNullOrEmpty(dados.Classificacao), "Informe a Classificação.");
            Refute(dados.PontuacaoMinima <= 0 || dados.PontuacaoMaxima <= 0, "Informe valores positivos.");
            Refute(dados.PontuacaoMinima >= dados.PontuacaoMaxima, "Pontuação mínima deve ser menor que a pontuação máxima.");
            //validar se esse range é desnecessário.
        }
    }
}