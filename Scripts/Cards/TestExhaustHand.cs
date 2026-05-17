using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Xiaojiu.Scripts;
using Xiaojiu.Scripts.CardPools;

namespace Xiaojiu.Scripts.Cards;

[RegisterCard(typeof(XiaojiuCardPool))]
public class TestExhaustHand : ModCardTemplate
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: "res://Xiaojiu/images/cards/TestCard.png"
    );

    public TestExhaustHand() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, 99)
        {
            RequireManualConfirmation = true,
            Cancelable = true
        };

        var selectedCards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, _ => true, this)).ToList();

        foreach (var card in selectedCards)
        {
            await CardCmd.Exhaust(choiceContext, card, false, false);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
