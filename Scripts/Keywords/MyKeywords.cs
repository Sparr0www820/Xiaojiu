using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using Xiaojiu.Scripts;

[RegisterOwnedCardKeyword(nameof(Cat), IconPath = "res://icon.svg", CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Upbring), IconPath = "res://icon.svg", CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
// [RegisterOwnedCardKeyword(nameof(Unique2), IconPath = "res://icon.svg")] // 如果要加更多关键词，添加特性
public class MyKeywords
{
    public static readonly string Cat = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Cat));
    public static readonly string Upbring = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Upbring));
}