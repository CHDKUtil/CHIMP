using Chimp.Model;
using Net.Chdk.Model.Card;

namespace Chimp.ViewModels
{
    public sealed class CardItemViewModel
    {
        public CardInfo Info { get; set; }
        public string DisplayName { get; set; }
        public string Bootable { get; set; }
        public bool Scriptable { get; set; }
        public PartitionType[] PartitionTypes { get; set; }
        public bool? Switched { get; set; }
        public string FileSystem { get; set; }
    }
}
