using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;
using Xiaojiu.Scripts;
using Xiaojiu.Scripts.CardPools;
using Xiaojiu.Scripts.Powers;

namespace Xiaojiu.Scripts.Cards;

[RegisterCard(typeof(XiaojiuCardPool))]
public class TestYinYang : ModCardTemplate
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
    ];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Xiaojiu/images/cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
		new BlockVar(5, ValueProp.Move)
    ];

    public TestYinYang() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
			.FromCard(this)
			.Targeting(cardPlay.Target!)
			.Execute(choiceContext);
		if (Owner.Creature.GetPowerAmount<YangPower>() > Owner.Creature.GetPowerAmount<YinPower>())
		{
			await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
				.FromCard(this)
				.Targeting(cardPlay.Target!)
				.Execute(choiceContext);
		}
		else if (Owner.Creature.GetPowerAmount<YangPower>() < Owner.Creature.GetPowerAmount<YinPower>())
		{
			await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
		}
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
		DynamicVars.Block.UpgradeValueBy(3);
    }
}
