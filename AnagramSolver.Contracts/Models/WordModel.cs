namespace AnagramSolver.Contracts.Models
{
    public class WordModel
    {
        public int Id { get; set; }
        public string Word { get; set; } = null!;
        public string PartOfSpeech { get; set; } = null!;
        public int Number { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WordModel);
        }

        public bool Equals(WordModel other)
        {
            return other != null &&
                   Word == other.Word &&
                   PartOfSpeech == other.PartOfSpeech;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Word, PartOfSpeech);
        }

        public override string? ToString()
        {
            return $"{Word}\t{PartOfSpeech}\t{Word}\t{Number}";
        }
    }
}
