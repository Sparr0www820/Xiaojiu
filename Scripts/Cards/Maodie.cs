using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using Xiaojiu.Scripts;

namespace Xiaojiu.Scripts.Cards;

[RegisterCard(typeof(TokenCardPool))]
public class Maodie : ModCardTemplate
{
    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Token;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = false;

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Xiaojiu/images/cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5, ValueProp.Move),
        new DynamicVar("DamageAfterExhausted", 3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        MyKeywords.Cat.GetModKeywordCardKeyword(),
        MyKeywords.Upbring.GetModKeywordCardKeyword(),
        CardKeyword.Ethereal
    ];

    public Maodie() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        EnergyCost.AddThisCombat(1);
        DynamicVars["DamageAfterExhausted"].BaseValue *= 2;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);
            await DamageCmd.Attack(DynamicVars["DamageAfterExhausted"].BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState!)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public static async Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
	{
		return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
	}

	public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, ICombatState combatState)
	{
		if (count == 0)
		{
			return Array.Empty<CardModel>();
		}
		if (CombatManager.Instance.IsOverOrEnding)
		{
			return Array.Empty<CardModel>();
		}
		List<CardModel> cats = new List<CardModel>();
		for (int i = 0; i < count; i++)
		{
			cats.Add(combatState.CreateCard<Maodie>(owner));
		}
		await CardPileCmd.AddGeneratedCardsToCombat(cats, PileType.Hand, owner);
		return cats;
	}
}
