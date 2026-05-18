using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Xiaojiu.Scripts;

namespace Xiaojiu.Scripts.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class Heimao : ModCardTemplate
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Token;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = false;

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Xiaojiu/images/cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("StrengthLoss", 3),
        new PowerVar<WeakPower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        MyKeywords.Cat.GetModKeywordCardKeyword(),
        MyKeywords.Upbring.GetModKeywordCardKeyword(),
        CardKeyword.Ethereal
    ];

    public Heimao() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target!, DynamicVars.Weak.BaseValue, Owner.Creature, this);
        EnergyCost.AddThisCombat(1);
        DynamicVars["StrengthLoss"].BaseValue *= 2;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this)
        {
            foreach (Creature hittableEnemy in CombatState!.HittableEnemies)
            {
                await PowerCmd.Apply<PiercingWailPower>(choiceContext, hittableEnemy, base.DynamicVars["StrengthLoss"].BaseValue, base.Owner.Creature, this);
            }
        }
    }

    protected override void OnUpgrade()
	{
		DynamicVars.Weak.UpgradeValueBy(1);
	}
}
