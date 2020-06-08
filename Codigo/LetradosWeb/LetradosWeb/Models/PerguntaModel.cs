namespace SeguradosAPI.Models
{
    public class PerguntaModel
    {
        public int IdPergunta { get; set; }
        public string Questao { get; set; }
        public string Alternativa1 { get; set; }
        public string Alternativa2 { get; set; }
        public string Alternativa3 { get; set; }
        public string Alternativa4 { get; set; }
        public int OpcaoCorreta { get; set; }
        public string Pontuacao { get; set; }
        public string Tempo { get; set; }
        public string Dificuldade { get; set; }
        public int TematicaIdTematica { get; set; }
    }
}
