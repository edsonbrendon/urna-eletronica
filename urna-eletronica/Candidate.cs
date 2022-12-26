using System.Drawing;

namespace urna_eletronica
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PoliticalParty { get; set; }        
        public Image Image { get; set; }
    }
}
