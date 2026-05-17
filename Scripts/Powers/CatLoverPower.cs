using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Xiaojiu.Scripts.Powers;

[RegisterPower]
public class CatLoverPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://icon.svg",
        BigIconPath: "res://icon.svg"
    );

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && card.HasModKeyword(MyKeywords.Upbring.GetModKeywordCardKeyword()))
        {
            await CardPileCmd.Add(card, PileType.Play);
            await CardCmd.Exhaust(choiceContext, card, fromHandDraw);
            CardModel card2 = card.CreateClone();
            await CardPileCmd.AddGeneratedCardToCombat(card2, PileType.Hand, card.Owner);
        }
    }
}