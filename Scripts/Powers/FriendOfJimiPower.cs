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
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Xiaojiu.Scripts.Powers;

[RegisterPower]
public class FriendOfJimiPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://icon.svg",
        BigIconPath: "res://icon.svg"
    );
	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player != Owner.Player)
		{
			return;
		}
		for (int i = 0; i < Amount; i++)
		{
			CardModel cardModel = CardFactory.GetDistinctForCombat(player, from c in ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
				where c.HasModKeyword(MyKeywords.Cat)
				select c, 1, player.RunState.Rng.CombatCardGeneration).FirstOrDefault();
			if (cardModel != null)
			{
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner.Player);
			}
		}
	}
}