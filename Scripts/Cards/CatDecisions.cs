using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
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
using Xiaojiu.Scripts.CardPools;

namespace Xiaojiu.Scripts.Cards;

[RegisterCard(typeof(XiaojiuCardPool))]
public class CatDecisions : ModCardTemplate
{
    private CardModel? _mockGeneratedCard;
    
    private const int energyCost = 1;
    
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Xiaojiu/images/cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [
    ];

    public CatDecisions () : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < 2; i++)
        {
            CardModel cardModel;
            if (_mockGeneratedCard == null)
            {
                IEnumerable<CardModel> cards = from c in ModelDb.CardPool<TokenCardPool>()
                        .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                    where c.HasModKeyword(MyKeywords.Cat)
                    select c;
                List<CardModel> list = CardFactory.GetDistinctForCombat(Owner, cards, 3,
                    Owner.RunState.Rng.CombatCardGeneration).ToList();
                if (IsUpgraded)
                {
                    foreach (CardModel item in list)
                    {
                        CardCmd.Upgrade(item);
                    }
                }
                cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, list, Owner, canSkip: false);
            }
            else
            {
                cardModel = _mockGeneratedCard;
                if (base.IsUpgraded)
                {
                    CardCmd.Upgrade(cardModel);
                }
            }

            if (cardModel != null)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
            }
        }
    }
    
    public void MockGeneratedCard(CardModel card)
    {
        AssertMutable();
        _mockGeneratedCard = card;
    }
}
