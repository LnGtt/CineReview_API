namespace CineReview.Models
{
    public interface IAvaliavel
    {
        List<Avaliacao> Avaliacoes { get; }
        double NotaMediaGeral { get; }
        double NotaMediaNarrativa { get; }
        double NotaMediaExecucao { get; }
        double NotaMediaVisual { get; }
        double NotaMediaAuditiva { get; }

        void AdicionarAvaliacao(Avaliacao avaliacao);
    }
}
